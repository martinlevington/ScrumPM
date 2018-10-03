﻿namespace ScrumPm.Domain.Common
{


    using System.Collections.Generic;

    internal static class HashCodeHelper
    {
        public static int CombineHashCodes(IEnumerable<object> objs)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objs)
                {
                    hash = (hash * 23) + (obj?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }
    }
}