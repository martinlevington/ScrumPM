using System;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Application.Teams.Commands
{
    public class EnableMemberCommand
    {
        public EnableMemberCommand()
        {
        }

        public EnableMemberCommand(TenantId tenantId, string username, string firstName, string lastName, string emailAddress, DateTime occurredOn, Guid teamId)
        {
            TenantId = tenantId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            OccurredOn = occurredOn;
            TeamId = teamId;
        }

        public TenantId TenantId { get; private set; }
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public DateTime OccurredOn { get; private set; }
        public Guid TeamId { get; private set; }
    }
}
