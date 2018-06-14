namespace ScrumPm.Domain.Teams
{
    using System;
    using ScrumPm.Domain.Tenants;

    public class ProductOwner : Member
    {
        public ProductOwner(
            TenantId tenantId,
            string username,
            string firstName,
            string lastName,
            string emailAddress,
            DateTime initializedOn)
            : base(tenantId, username, firstName, lastName, emailAddress, initializedOn)
        {
        }

        public ProductOwnerId ProductOwnerId { get; private set; }

        public override string ToString()
        {
            return "ProductOwner [productOwnerId()=" + ProductOwnerId + ", emailAddress()=" + EmailAddress + ", isEnabled()="
                   + Enabled + ", firstName()=" + FirstName + ", lastName()=" + LastName + ", tenantId()=" + TenantId
                   + ", username()=" + Username + "]";
        }

        protected override System.Collections.Generic.IEnumerable<object> GetIdentityComponents()
        {
            yield return this.TenantId;
            yield return this.Username;
        }
    }
}
