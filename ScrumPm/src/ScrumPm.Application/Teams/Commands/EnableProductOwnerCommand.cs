using System;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams.Commands
{
    public class EnableProductOwnerCommand : EnableMemberCommand
    {
        public EnableProductOwnerCommand()
        {
        }

        public EnableProductOwnerCommand(TenantId tenantId, string username, string firstName, string lastName, string emailAddress, DateTime occurredOn)
            : base(tenantId, username, firstName, lastName, emailAddress, occurredOn)
        {
        }
    }
}
