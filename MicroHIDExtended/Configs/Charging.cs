// -----------------------------------------------------------------------
// <copyright file="Charging.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended.Configs
{
    using MicroHIDExtended.Charges;
    using MicroHIDExtended.Models;

    /// <summary>
    /// A container for all config classes which inherit <see cref="ICharge"/>.
    /// </summary>
    public class Charging
    {
        /// <summary>
        /// Gets or sets the configs to use while the micro is idle.
        /// </summary>
        public Idle Idle { get; set; } = new Idle();

        /// <summary>
        /// Gets or sets the configs to use while the micro is being used.
        /// </summary>
        public Using Using { get; set; } = new Using();

        /// <summary>
        /// Gets or sets the configs to use while the micro is discharging.
        /// </summary>
        public Discharging Discharging { get; set; } = new Discharging();
    }
}