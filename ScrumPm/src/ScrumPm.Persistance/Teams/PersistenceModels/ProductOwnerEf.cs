using System;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public class ProductOwnerEf : IDataEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public virtual Guid TeamId { get; set; }
        public virtual TeamEf Team { get; set; }
    }
}
