using System.Runtime.Serialization;
using Integration.Contracts;

namespace Implementation.Contracts
{
    /// <summary>
    /// A datapoint value is an key-value like object, which holds the type of this datapoint and the actual data (value).
    /// </summary>
    [DataContract]
    public class DatapointValue : IDatapointValue
    {
        /// <summary>
        /// This contains the type of this datapoint.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// This holds the actual data (value).
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}