using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Implementation.Crypto;
using Integration;

namespace Implementation
{
    /// <summary>
    /// Initialize the enviroment, check config values etc.
    /// </summary>
    public class AppStart : IAppStart
    {
        /// <summary>
        /// Initialize the enviroment, check config values etc.
        /// </summary>
        /// <returns>If everthing went OK</returns>
        public bool InitEnviroment(IApplicationConfiguration applicationConfiguration, out IList<IPeer> outPeer)
        {
            outPeer = null;
            try
            {
                if (string.IsNullOrEmpty(applicationConfiguration["AgentId"]))
                {
                    throw new ArgumentNullException("AgentId", "MUST be not null or empty");
                }

                int resultInt;
                if (!int.TryParse(applicationConfiguration["IOBufferSize"], out resultInt))
                {
                    throw new ArgumentOutOfRangeException("IOBufferSize", "MUST be an signed integer (32 bit)");
                }

                ulong resultUlong;
                if (!ulong.TryParse(applicationConfiguration["LastMessageId"], out resultUlong))
                {
                    applicationConfiguration["LastMessageId"] = "0";
                }

                #region FileSystem

                if (string.IsNullOrEmpty(applicationConfiguration["InputPath"]))
                {
                    applicationConfiguration["InputPath"] = @".\Input\";
                }

                if (string.IsNullOrEmpty(applicationConfiguration["OutputPath"]))
                {
                    applicationConfiguration["OutputPath"] = @".\Output\";
                }

                var inputPath = applicationConfiguration["InputPath"];
                var outputPath = applicationConfiguration["OutputPath"];

                if (!Directory.Exists(inputPath))
                {
                    Directory.CreateDirectory(inputPath);
                }

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                #endregion

                #region Crypto

                if (string.IsNullOrEmpty(applicationConfiguration["AsymmetricAlgorithm"]))
                {
                    throw new ArgumentNullException("AsymmetricAlgorithm", "MUST be not null or empty");
                }

                if (string.IsNullOrEmpty(applicationConfiguration["HashAlgorithm"]))
                {
                    throw new ArgumentNullException("HashAlgorithm", "MUST be not null or empty");
                }

                if (applicationConfiguration["CertificatePassword"] == null)
                {
                    applicationConfiguration["CertificatePassword"] = string.Empty;
                }

                try
                {
                    File.OpenRead(applicationConfiguration["Certificate"]).Dispose();
                }
                catch (Exception)
                {
                    if (File.Exists(applicationConfiguration["Certificate"]))
                    {
                        throw new FileLoadException("Certificate exists but is not readable", applicationConfiguration["Certificate"]);
                    }
                    throw new FileNotFoundException("Certificate");
                }

                using (var cryptoProvider = new CryptoProvider(applicationConfiguration["Certificate"], applicationConfiguration["AsymmetricAlgorithm"], applicationConfiguration["HashAlgorithm"], applicationConfiguration["CertificatePassword"]))
                {
                    var signature = cryptoProvider.Sign(Encoding.UTF8.GetBytes(applicationConfiguration["AgentId"]));

                    if (!(signature?.Length > 0))
                    {
                        throw new CryptographicException("Messages cannot be signed!");
                    }
                }

                #endregion

                #region Network

                if (string.IsNullOrEmpty(applicationConfiguration["Peer"]))
                {
                    throw new ArgumentNullException("Argument MUST be not null or empty: Peer");
                }
                var peers = applicationConfiguration["Peer"].Split(';');

                foreach (var peer in peers)
                {
                    Uri url;
                    IPAddress ip;
                    IPEndPoint endPoint = null;
//                    HttpResponseMessage httpResult;
                    var scheme = "http";
                    outPeer = new List<IPeer>();

                    if (Uri.TryCreate(string.Format("{1}://{0}", new object[] { peer, scheme }), UriKind.Absolute, out url))
                    {
                        IPAddress.TryParse(url.Host, out ip);
                        endPoint = new IPEndPoint(ip, url.Port);
                    }

                    var requestMessage = new HttpRequestMessage { Version = new Version("1.1"), Method = HttpMethod.Get, RequestUri = url };

                    //try
                    //{
                    //    using (var httpClient = new HttpClient())
                    //    {
                    //        httpResult = httpClient.SendAsync(requestMessage).Result;
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //    scheme += "s";
                    //    Uri.TryCreate(string.Format("{1}://{0}", new object[] { peer, scheme }), UriKind.Absolute, out url);
                    //    endPoint = new IPEndPoint(ip, url.Port);
                    //    requestMessage = new HttpRequestMessage { Version = new Version("1.1"), Method = HttpMethod.Get, RequestUri = url };
                    //
                    //    try
                    //    {
                    //        using (var httpClient = new HttpClient())
                    //        {
                    //            httpResult = httpClient.SendAsync(requestMessage).Result;
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        // TODO Add logging
                    //        throw;
                    //    }
                    //}

                    //if (httpResult.IsSuccessStatusCode)
                    //{
                    outPeer.Add(new Peer { Uri = url, ProtocolScheme = scheme, IpEndPoint = endPoint });
                    //}
                }

                #endregion

                return true;
            }
            catch (Exception)
            {
                // TODO Add logging
                return false;
                //throw;
            }
        }
    }
}