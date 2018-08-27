using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPm.Persistence.Teams.PersistenceModels
{
    public class ProductOwner
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }


    }
}
