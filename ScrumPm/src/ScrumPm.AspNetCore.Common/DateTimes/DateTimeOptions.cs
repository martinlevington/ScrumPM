using System;

namespace ScrumPm.AspNetCore.Common.DateTimes
{
    public class DateTimeOptions
    {
        /// <summary>
        /// Default: <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        public DateTimeKind Kind { get; set; }

        public DateTimeOptions()
        {
            Kind = DateTimeKind.Unspecified;
        }
    }
}
