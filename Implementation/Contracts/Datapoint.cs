using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Integration.Contracts;

namespace Implementation.Contracts
{
    /// <summary>
    /// A datapoint is a container which wraps one or more datapoint values.
    /// </summary>
    [DataContract]
    public class Datapoint : IDatapoint
    {
        /// <summary/>
        public Datapoint()
        {
            Values = new List<IDatapointValue>();
        }

        /// <summary>
        /// A module identifies a collection of one or multiple datapoint values.
        /// For example: "ping to 10.0.0.1"
        /// </summary>
        [DataMember]
        public string Module { get; set; }

        /// <summary>
        /// A type defines how the structure, of the datapoint values, should be.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Timestamp, when the data has been written by the module.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// List of the datapoint values.
        /// </summary>
        [DataMember]
        public IList<IDatapointValue> Values { get; set; }
    }
}