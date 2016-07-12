using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Implementation;
using Implementation.Contracts;
using Implementation.Crypto;
using Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Collector_Agent
{
    /// <summary/>
    public partial class CollectorAgentService : ServiceBase
    {
        /// <summary>
        /// Simple application config interface.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ApplicationConfiguration Configuration = new ApplicationConfiguration();

        /// <summary>
        /// contains the list of one or more peers, to which messages can be send.
        /// </summary>
        public static IList<IPeer> Peers;

        private DateTime _lastRun = DateTime.Now.AddDays(-1);
        private Timer _timer;

        /// <summary></summary>
        public CollectorAgentService()
        {
            _timer = new Timer(1 * 60 * 1000); // every 1 minutes
            InitializeComponent();
        }

        /// <summary>
        /// This is intended to execute within an interactive execution enviroment, like a debugger or via shell exec.
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM)
        /// or when the operating system starts (for a service that starts automatically).
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            var appStart = new AppStart();
            var initSucceded = appStart.InitEnviroment(Configuration, out Peers);

            if (initSucceded)
            {
#if DEBUG
                ProcessInput(null, null);
#else
                _timer.Elapsed += ProcessInput;
                _timer.Start();
#endif
            }
            else
            {
                OnStop();
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            // TODO Implement
            _timer?.Stop();
        }

#if !DEBUG
        private void ProcessInput(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_lastRun.Date >= DateTime.Now.Date) return;
            _timer.Stop();
#else
        private static void ProcessInput(object sender, ElapsedEventArgs elapsedEventArgs)
        {
#endif
            var inputPath = Configuration["InputPath"];
            var outputPath = Configuration["OutputPath"];

            var inputFiles = Directory.GetFiles(inputPath, "*.txt");

            if (inputFiles.Length > 0)
            {
                var message = new Message { Agent = Configuration["AgentId"] };
                foreach (var inputFile in inputFiles)
                {
                    if (!File.Exists(inputFile)) continue;

                    var counter = 0;
                    var line = string.Empty;
                    var hasName = false;
                    var hasType = false;
                    var isKey = true;

                    DatapointValue datapointValue = null;

                    using (var sr = new StreamReader(inputFile))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            switch (counter)
                            {
                                case 0:
                                    message.Datapoints.Add(new Datapoint { Module = line });
                                    message.Datapoints.Last().TimeStamp = File.GetCreationTime(inputFile);
                                    hasName = true;
                                    break;
                                case 1:
                                    message.Datapoints.Last().Type = line;
                                    hasType = true;
                                    break;
                                default:
                                    if (hasName && hasType)
                                    {
                                        if (isKey)
                                        {
                                            datapointValue = new DatapointValue { Type = line };
                                            isKey = false;
                                        }
                                        else
                                        {
                                            datapointValue.Value = line;
                                            message.Datapoints.Last().Values.Add(datapointValue);
                                            isKey = true;
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Module name or Module type not found");
                                    }
                                    break;
                            }

                            counter++;
                        }
                    }
                }

                ulong lastMessageId;
                var getLastMessageIdSucceded = ulong.TryParse(Configuration["LastMessageId"], out lastMessageId);

                if (!getLastMessageIdSucceded)
                {
                    try
                    {
                        lastMessageId = Convert.ToUInt64(Configuration["LastMessageId"] = "0");
                    }
                    catch
                    {
                        // TODO Logging / Exeption-handling
                        throw;
                    }
                }

                lastMessageId++;
                Configuration["LastMessageId"] = lastMessageId.ToString(new CultureInfo("en-us", true));
                message.Id = lastMessageId;

                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                string finalMessage;
                byte[] signature;

                using (var cryptoProvider = new CryptoProvider(Configuration["Certificate"], Configuration["AsymmetricAlgorithm"], Configuration["HashAlgorithm"], Configuration["CertificatePassword"]))
                {
                    signature = cryptoProvider.Sign(Encoding.UTF8.GetBytes(jsonMessage));
                    finalMessage = new Regex("{").Replace(jsonMessage, "{\"signature\":\"" + Convert.ToBase64String(signature) + "\",", 1);

                    using (var sw = File.OpenWrite(Path.Combine(outputPath + string.Format("{0}.json", message.Id))))
                    using (var wbs = new BufferedStream(sw, int.Parse(Configuration["IOBufferSize"])))
                    {
                        wbs.Write(Encoding.UTF8.GetBytes(finalMessage), 0, finalMessage.Length);
                    }
                }

//                using (var httpClient = new HttpClient())
//                {
//                    using (var content = new MultipartFormDataContent("Upload----"))
//                    {
//                        content.Add(new StringContent(finalMessage, Encoding.UTF8, "application/json"), "\"data\"");
//
//                        using (var messageData = httpClient.PostAsync("http://" + Configuration["Peer"] + "/input/log", content).Result)
//                        {
//                            var input = messageData.Content.ReadAsStringAsync().Result;
//                            Console.WriteLine(input);
//                            Console.WriteLine(messageData);
//                        }
//                    }
//                }
            }
#if !DEBUG
            _lastRun = DateTime.Now;
            _timer.Start();
#endif
        }
    }
}