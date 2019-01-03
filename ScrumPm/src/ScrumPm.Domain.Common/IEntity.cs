using System.Collections.Generic;

namespace ScrumPm.Domain.Common
{
    /// <summary>
    /// Defines an entity. It's primary key may not be "Id" or it may have a composite primary key.
    /// </summary>
    public interface IEntity
    {
        IEnumerable<object> GetIdentityComponents();
    }

   
}
