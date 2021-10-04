// -----------------------------------------------------------------------
// <copyright file="Idle.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended.Charges
{
    using MicroHIDExtended.Models;

    /// <summary>
    /// Defines the charging configs to use while a micro is idling.
    /// </summary>
    public class Idle : ICharge
    {
        /// <inheritdoc />
        public float Amount { get; set; } = 0.015f;

        /// <inheritdoc />
        public float Interval { get; set; } = 0.5f;
    }
}