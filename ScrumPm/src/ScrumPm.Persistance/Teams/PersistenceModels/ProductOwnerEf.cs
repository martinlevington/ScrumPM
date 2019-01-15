using System;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public sealed class ProductOwnerEf : IDataEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public TeamEf TeamEf { get; set; }
    }
}
