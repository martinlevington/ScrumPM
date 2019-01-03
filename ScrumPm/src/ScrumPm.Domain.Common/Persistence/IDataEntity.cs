namespace ScrumPm.Domain.Common.Persistence
{
   
    public interface IDataEntity
    {
        
    }

    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IDataEntity<TKey> : IDataEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; set; }
    }
   
}
