namespace ScrumPm.Domain.Teams
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using ScrumPm.Common;
    using ScrumPm.Domain.Tenants;

    public class Team : Entity
    {
        public Team(TenantId tenantId, string name, ProductOwner productOwner = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenantId must be provided.");

            this._tenantId = tenantId;
            this.Name = name;
            if (productOwner != null)
            {
                this.ProductOwner = productOwner;
            }

            this._teamMembers = new HashSet<TeamMember>();
        }

        readonly TenantId _tenantId;
        string _name;
        ProductOwner _productOwner;
        readonly HashSet<TeamMember> _teamMembers;

        public TenantId TenantId
        {
            get { return this._tenantId; }
        }

        public string Name
        {
            get => _name;

            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "The name must be provided.");
                AssertionConcern.AssertArgumentLength(value, 100, "The name must be 100 characters or less.");
                this._name = value;
            }
        }

        public ProductOwner ProductOwner
        {
            get => this._productOwner;

            private set
            {
                AssertionConcern.AssertArgumentNotNull(value, "The productOwner must be provided.");
                AssertionConcern.AssertArgumentEquals(this._tenantId, value.TenantId, "The productOwner must be of the same tenant.");
                _productOwner = value;
            }
        }

        public ReadOnlyCollection<TeamMember> AllTeamMembers => new ReadOnlyCollection<TeamMember>(this._teamMembers.ToArray());

        public void AssignProductOwner(ProductOwner productOwner)
        {
            this.ProductOwner = productOwner;
        }

        public void AssignTeamMember(TeamMember teamMember)
        {
            AssertValidTeamMember(teamMember);
            this._teamMembers.Add(teamMember);
        }

        public bool IsTeamMember(TeamMember teamMember)
        {
            AssertValidTeamMember(teamMember);
            return GetTeamMemberByUserName(teamMember.Username) != null;
        }

        public void RemoveTeamMember(TeamMember teamMember)
        {
            AssertValidTeamMember(teamMember);
            var existingTeamMember = GetTeamMemberByUserName(teamMember.Username);
            if (existingTeamMember != null)
            {
                this._teamMembers.Remove(existingTeamMember);
            }
        }

        void AssertValidTeamMember(TeamMember teamMember)
        {
            AssertionConcern.AssertArgumentNotNull(teamMember, "A team member must be provided.");
            AssertionConcern.AssertArgumentEquals(this.TenantId, teamMember.TenantId, "Team member must be of the same tenant.");
        }

        TeamMember GetTeamMemberByUserName(string userName)
        {
            return this._teamMembers.FirstOrDefault(x => x.Username.Equals(userName));
        }
    }
}
