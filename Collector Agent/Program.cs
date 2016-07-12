#if DEBUG
using System;
#endif
using System.Diagnostics.CodeAnalysis;
#if !DEBUG
using System.ServiceProcess;
#endif

namespace Collector_Agent
{
    internal static class Program
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static void Main()
        {
#if DEBUG
            var service = new CollectorAgentService();
            service.OnDebug();
            //Thread.Sleep(Timeout.Infinite);
            Environment.Exit(0);
#else
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] { new CollectorAgentService() };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}