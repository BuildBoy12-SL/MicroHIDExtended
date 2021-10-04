// -----------------------------------------------------------------------
// <copyright file="ICharge.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended.Models
{
    /// <summary>
    /// Defines the contract for how charging configs should be implemented.
    /// </summary>
    public interface ICharge
    {
        /// <summary>
        /// Gets or sets the amount of charge to add.
        /// </summary>
        float Amount { get; set; }

        /// <summary>
        /// Gets or sets the duration between each charge.
        /// </summary>
        float Interval { get; set; }
    }
}