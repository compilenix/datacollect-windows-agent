using System;
using System.Collections.Generic;

namespace Integration.Contracts
{
    /// <summary>
    /// A datapoint is a container which wraps one or more datapoint values.
    /// </summary>
    public interface IDatapoint
    {
        /// <summary>
        /// A module identifies a collection of one or multiple datapoint values.
        /// For example: "ping to 10.0.0.1"
        /// </summary>
        string Module { get; set; }

        /// <summary>
        /// A type defines how the structure, of the datapoint values, should be.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Timestamp, when the data has been written by the module.
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// List of the datapoint values.
        /// </summary>
        IList<IDatapointValue> Values { get; set; }
    }
}