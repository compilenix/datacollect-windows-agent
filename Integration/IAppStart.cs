using System.Collections.Generic;

namespace Integration
{
    /// <summary>
    /// Initialize the enviroment, check config values etc.
    /// </summary>
    public interface IAppStart
    {
        /// <summary>
        /// Initialize the enviroment, check config values etc.
        /// </summary>
        /// <returns>If everthing went OK</returns>
        bool InitEnviroment(IApplicationConfiguration applicationConfiguration, out IList<IPeer> outPeer);
    }
}
