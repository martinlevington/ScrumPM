using System;
using System.Collections.Generic;
using ScrumPm.Domain.Tenants;

namespace ScrumPm.Domain.Teams
{
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
            ProductOwnerId = Guid.NewGuid();
        }

        public Guid ProductOwnerId { get; private set; }

        public override string ToString()
        {
            return "ProductOwner [productOwnerId()=" + ProductOwnerId + ", emailAddress()=" + EmailAddress + ", isEnabled()="
                   + Enabled + ", firstName()=" + FirstName + ", lastName()=" + LastName + ", tenantId()=" + TenantId
                   + ", username()=" + UserName + "]";
        }

        public override IEnumerable<object> GetIdentityComponents()
        {
            yield return TenantId;
            yield return UserName;
        }
    }
}
