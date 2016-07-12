using System.Collections.Generic;
using System.Runtime.Serialization;
using Integration.Contracts;

namespace Implementation.Contracts
{
    /// <summary>
    /// A message, which can be send over the wire.
    /// </summary>
    [DataContract]
    public class Message : IMessage
    {
        /// <summary/>
        public Message()
        {
            Datapoints = new List<IDatapoint>();
        }

        /// <summary>
        /// See: <see cref="Datapoint"/>
        /// </summary>
        [DataMember]
        public IList<IDatapoint> Datapoints { get; set; }

        /// <summary>
        /// Identifies the agent.
        /// </summary>
        [DataMember]
        public string Agent { get; set; }

        /// <summary/>
        public ulong Id { get; set; }
    }
}