using System.Collections.Generic;

namespace Integration.Contracts
{
    /// <summary>
    /// A message, which can be send over the wire.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// See: <see cref="IDatapoint"/>
        /// </summary>
        IList<IDatapoint> Datapoints { get; set; }

        /// <summary>
        /// Identifies the agent.
        /// </summary>
        string Agent { get; set; }

        /// <summary/>
        ulong Id { get; set; }
    }
}