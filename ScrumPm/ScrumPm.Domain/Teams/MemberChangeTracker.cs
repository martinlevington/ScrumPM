namespace ScrumPm.Domain.Teams
{
    using System;
    using ScrumPm.Common;

    public class MemberChangeTracker : ValueObject
    {
        private readonly DateTime _enablingOnDate;
        private readonly DateTime _nameChangedOnDate;
        private readonly DateTime _emailAddressChangedOnDate;

        internal MemberChangeTracker(DateTime enablingOn, DateTime nameChangedOn, DateTime emailAddressChangedOn)
        {
            this._emailAddressChangedOnDate = emailAddressChangedOn;
            this._enablingOnDate = enablingOn;
            this._nameChangedOnDate = nameChangedOn;
        }

        public bool CanChangeEmailAddress(DateTime asOfDateTime)
        {
            return this._emailAddressChangedOnDate < asOfDateTime;
        }

        public bool CanChangeName(DateTime asOfDateTime)
        {
            return this._nameChangedOnDate < asOfDateTime;
        }

        public bool CanToggleEnabling(DateTime asOfDateTime)
        {
            return this._enablingOnDate < asOfDateTime;
        }

        public MemberChangeTracker EmailAddressChangedOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(this._enablingOnDate, this._nameChangedOnDate, asOfDateTime);
        }

        public MemberChangeTracker EnablingOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(asOfDateTime, this._nameChangedOnDate, this._emailAddressChangedOnDate);
        }

        public MemberChangeTracker NameChangedOn(DateTime asOfDateTime)
        {
            return new MemberChangeTracker(this._enablingOnDate, asOfDateTime, this._emailAddressChangedOnDate);
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return this._enablingOnDate;
            yield return this._nameChangedOnDate;
            yield return this._emailAddressChangedOnDate;
        }
    }
}
