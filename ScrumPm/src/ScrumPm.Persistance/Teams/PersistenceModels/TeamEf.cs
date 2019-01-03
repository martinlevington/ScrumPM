using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public class TeamEf : IDataEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid TenantId  { get; set; }
        public string Name { get; set; }

        public Guid ProductOwnerId { get; set; }
        public virtual ProductOwnerEf ProductOwner { get; set; }
    }
}
