using System;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Models.Teams.PersistenceModels
{
    public sealed class TeamEf : IDataEntity<Guid>
    {

        public Guid Id { get; set; }

        public Guid TenantId  { get; set; }
        public string Name { get; set; }

        public Guid ProductOwnerId { get; set; }
        public ProductOwnerEf ProductOwnerEf { get; set; }
    }
}
