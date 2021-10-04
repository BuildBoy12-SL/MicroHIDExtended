// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended
{
    using Exiled.API.Interfaces;
    using MicroHIDExtended.Configs;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets all charging related configs.
        /// </summary>
        public Charging Charging { get; set; } = new Charging();
    }
}