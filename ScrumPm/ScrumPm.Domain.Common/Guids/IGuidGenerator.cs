﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPm.Domain.Common.Guids
{
    /// <summary>
    /// Used to generate Ids.
    /// </summary>
    public interface IGuidGenerator
    {
        /// <summary>
        /// Creates a new <see cref="Guid"/>.
        /// </summary>
        Guid Create();
    }
}
