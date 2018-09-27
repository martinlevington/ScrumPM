using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Teams
{
    using System;
    using Tenants;

    public abstract class Member : EntityWithCompositeId
    {
        private MemberChangeTracker _changeTracker;
        private string _emailAddress;
        private string _firstName;
        private string _lastName;

        private string _userName;

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
            UserName = userName;

            _changeTracker = new MemberChangeTracker(initializedOn, initializedOn, initializedOn);
        }

        public TenantId TenantId { get; }

        public string UserName
        {
            get => _userName;
            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "The username must be provided.");
                AssertionConcern.AssertArgumentLength(value, 250, "The username must be 250 characters or less.");
                _userName = value;
            }
        }

        public string EmailAddress
        {
            get => _emailAddress;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(value, 100,
                        "Email address must be 100 characters or less.");
                }

                _emailAddress = value;
            }
        }

        public string FirstName
        {
            get => _firstName;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(value, 50, "First name must be 50 characters or less.");
                }

                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            private set
            {
                if (value != null)
                {
                    AssertionConcern.AssertArgumentLength(value, 50, "Last name must be 50 characters or less.");
                }

                _lastName = value;
            }
        }

        public bool Enabled { get; private set; }

        public void ChangeEmailAddress(string emailAddress, DateTime asOfDate)
        {
            if (_changeTracker.CanChangeEmailAddress(asOfDate)
                && !EmailAddress.Equals(emailAddress))
            {
                EmailAddress = emailAddress;
                _changeTracker = _changeTracker.EmailAddressChangedOn(asOfDate);
            }
        }

        public void ChangeName(string firstName, string lastName, DateTime asOfDate)
        {
            if (!_changeTracker.CanChangeName(asOfDate))
            {
                return;
            }

            FirstName = firstName;
            LastName = lastName;
            _changeTracker = _changeTracker.NameChangedOn(asOfDate);
        }

        public void Disable(DateTime asOfDate)
        {
            if (!_changeTracker.CanToggleEnabling(asOfDate))
            {
                return;
            }

            Enabled = false;
            _changeTracker = _changeTracker.EnablingOn(asOfDate);
        }

        public void Enable(DateTime asOfDate)
        {
            if (!_changeTracker.CanToggleEnabling(asOfDate))
            {
                return;
            }

            Enabled = true;
            _changeTracker = _changeTracker.EnablingOn(asOfDate);
        }
    }
}