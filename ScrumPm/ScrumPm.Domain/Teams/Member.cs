namespace ScrumPm.Domain.Teams
{
    using System;
    using ScrumPm.Common;
    using ScrumPm.Domain.Tenants;

    public abstract class Member : EntityWithCompositeId
    {
        private MemberChangeTracker changeTracker;
        private string emailAddress;
        private string firstName;
        private string lastName;

        private string userName;

        public Member(
            TenantId tenantId,
            string userName,
            string firstName,
            string lastName,
            string emailAddress,
            DateTime initializedOn)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenant id must be provided.");

            TenantId = tenantId;
            EmailAddress = emailAddress;
            Enabled = true;
            FirstName = firstName;
            LastName = lastName;
            changeTracker = new MemberChangeTracker(initializedOn, initializedOn, initializedOn);
        }

        public TenantId TenantId { get; }

        public string Username
        {
            get => userName;
            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "The username must be provided.");
                AssertionConcern.AssertArgumentLength(value, 250, "The username must be 250 characters or less.");
                userName = value;
            }
        }

        public string EmailAddress
        {
            get => emailAddress;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(emailAddress, 100,
                        "Email address must be 100 characters or less.");
                }

                emailAddress = value;
            }
        }

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(value, 50, "First name must be 50 characters or less.");
                }

                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(value, 50, "Last name must be 50 characters or less.");
                }

                lastName = value;
            }
        }

        public bool Enabled { get; private set; }

        public void ChangeEmailAddress(string emailAddress, DateTime asOfDate)
        {
            if (changeTracker.CanChangeEmailAddress(asOfDate)
                && !EmailAddress.Equals(emailAddress))
            {
                EmailAddress = emailAddress;
                changeTracker = changeTracker.EmailAddressChangedOn(asOfDate);
            }
        }

        public void ChangeName(string firstName, string lastName, DateTime asOfDate)
        {
            if (!changeTracker.CanChangeName(asOfDate))
            {
                return;
            }

            FirstName = firstName;
            LastName = lastName;
            changeTracker = changeTracker.NameChangedOn(asOfDate);
        }

        public void Disable(DateTime asOfDate)
        {
            if (!changeTracker.CanToggleEnabling(asOfDate))
            {
                return;
            }

            Enabled = false;
            changeTracker = changeTracker.EnablingOn(asOfDate);
        }

        public void Enable(DateTime asOfDate)
        {
            if (!changeTracker.CanToggleEnabling(asOfDate))
            {
                return;
            }

            Enabled = true;
            changeTracker = changeTracker.EnablingOn(asOfDate);
        }
    }
}