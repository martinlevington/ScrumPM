using System;
using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Teams
{
    public class TeamId : Identity
    {
        public TeamId(string id)
            : base(id)
        {
        }

        public TeamId(Guid id)
            : base(id)
        {
        }
      
    }
}
