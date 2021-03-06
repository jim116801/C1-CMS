using Composite.Core.Serialization;


namespace Composite.Data
{
    /// <summary>
    /// Base interface for data types in Composite C1.
    /// </summary>
    [SerializerHandler(typeof(DataSerializerHandler))]
    public interface IData
    {
        /// <summary>
        /// Uniquely identify this data element, its type and what provider it came from.
        /// </summary>
        DataSourceId DataSourceId { get; }
    }
}
