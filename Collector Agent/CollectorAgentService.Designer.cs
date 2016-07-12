using System.ComponentModel;

namespace Collector_Agent
{
    public partial class CollectorAgentService
    {
        private IContainer components = null;

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="System.ServiceProcess.ServiceBase" />.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ServiceName = "Collector Agent";
        }
    }
}
