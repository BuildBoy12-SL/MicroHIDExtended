// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;
        private EventHandlers eventHandlers;

        /// <summary>
        /// Gets an instance of the <see cref="MicroHIDExtended.RechargeTick"/> class.
        /// </summary>
        public RechargeTick RechargeTick { get; private set; }

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            RechargeTick = new RechargeTick(this);
            if (Round.IsStarted)
                RechargeTick.Start();

            eventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.RoundStarted += eventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += eventHandlers.OnWaitingForPlayers;

            harmony = new Harmony($"build.microHidExtended.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            harmony.UnpatchAll();
            harmony = null;

            Exiled.Events.Handlers.Server.RoundStarted -= eventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= eventHandlers.OnWaitingForPlayers;
            eventHandlers = null;

            RechargeTick.Stop();
            RechargeTick = null;
            base.OnDisabled();
        }
    }
}