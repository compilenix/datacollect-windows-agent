namespace Integration.Contracts
{
    /// <summary>
    /// A datapoint value is an key-value like object, which holds the type of this datapoint and the actual data (value).
    /// </summary>
    public interface IDatapointValue
    {
        /// <summary>
        /// This contains the type of this datapoint.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// This holds the actual data (value).
        /// </summary>
        string Value { get; set; }
    }
}