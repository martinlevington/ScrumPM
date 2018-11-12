using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public class TeamEf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid TenantId  { get; set; }
        public string Name { get; set; }

        public int ProductOwnerId { get; set; }
        public ProductOwnerEf ProductOwner { get; set; }
    }
}
