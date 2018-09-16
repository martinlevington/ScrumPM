using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Teams
{
    using System;

    public class MemberChangeTracker : ValueObject
    {
        private readonly DateTime _enablingOnDate;
        private readonly DateTime _nameChangedOnDate;
        private readonly DateTime _emailAddressChangedOnDate;

        internal MemberChangeTracker(DateTime enablingOn, DateTime nameChangedOn, DateTime emailAddressChangedOn)
        {
            _emailAddressChangedOnDate = emailAddressChangedOn;
            _enablingOnDate = enablingOn;
            _nameChangedOnDate = nameChangedOn;
        }

        public bool CanChangeEmailAddress(DateTime asOfDateTime)
        {
            return _emailAddressChangedOnDate < asOfDateTime;
        }

        public bool CanChangeName(DateTime asOfDateTime)
        {
            return _nameChangedOnDate < asOfDateTime;
        }

        public bool CanToggleEnabling(DateTime asOfDateTime)
        {
            return _enablingOnDate < asOfDateTime;
        }

        public MemberChangeTracker EmailAddressChangedOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(_enablingOnDate, _nameChangedOnDate, asOfDateTime);
        }

        public MemberChangeTracker EnablingOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(asOfDateTime, _nameChangedOnDate, _emailAddressChangedOnDate);
        }

        public MemberChangeTracker NameChangedOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(_enablingOnDate, asOfDateTime, _emailAddressChangedOnDate);
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return _enablingOnDate;
            yield return _nameChangedOnDate;
            yield return _emailAddressChangedOnDate;
        }
    }
}
