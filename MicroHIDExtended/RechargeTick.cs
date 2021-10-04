// -----------------------------------------------------------------------
// <copyright file="RechargeTick.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroHIDExtended
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using InventorySystem.Items.MicroHID;
    using MEC;
    using MicroHIDExtended.Models;
    using UnityEngine;

    /// <summary>
    /// Handles the recharging of all <see cref="MicroHIDItem"/> and <see cref="MicroHIDPickup"/> objects.
    /// </summary>
    public class RechargeTick
    {
        private readonly Plugin plugin;
        private readonly Dictionary<MicroHIDPickup, float> pickupTimers = new Dictionary<MicroHIDPickup, float>();
        private readonly Dictionary<MicroHIDItem, float> itemTimers = new Dictionary<MicroHIDItem, float>();
        private readonly Dictionary<HidState, ICharge> chargeRates;
        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RechargeTick"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RechargeTick(Plugin plugin)
        {
            this.plugin = plugin;
            chargeRates = new Dictionary<HidState, ICharge>
            {
                [HidState.Idle] = plugin.Config.Charging.Idle,
                [HidState.Firing] = plugin.Config.Charging.Discharging,
                [HidState.Primed] = plugin.Config.Charging.Using,
                [HidState.PoweringDown] = plugin.Config.Charging.Using,
                [HidState.PoweringUp] = plugin.Config.Charging.Using,
            };
        }

        /// <summary>
        /// Starts the <see cref="Update"/> of all MicroHID entities.
        /// </summary>
        public void Start()
        {
            if (!coroutineHandle.IsRunning)
                coroutineHandle = Timing.RunCoroutine(Update());
        }

        /// <summary>
        /// Stops and cleans the updating of micros.
        /// </summary>
        public void Stop()
        {
            if (!coroutineHandle.IsRunning)
                return;

            Timing.KillCoroutines(coroutineHandle);
            pickupTimers.Clear();
            itemTimers.Clear();
        }

        private IEnumerator<float> Update()
        {
            while (true)
            {
                ChargePickups();
                ChargeItems();
                yield return Timing.WaitForOneFrame;
            }
        }

        private void ChargePickups()
        {
            foreach (Pickup pickup in Map.Pickups)
            {
                if (pickup.Base is MicroHIDPickup microHidPickup)
                {
                    ChargePickup(microHidPickup);
                }
            }
        }

        private void ChargePickup(MicroHIDPickup microHidPickup)
        {
            if (!pickupTimers.ContainsKey(microHidPickup))
                pickupTimers.Add(microHidPickup, 0f);

            pickupTimers[microHidPickup] += Time.deltaTime;
            if (pickupTimers[microHidPickup] < plugin.Config.Charging.Idle.Interval)
                return;

            pickupTimers[microHidPickup] = 0f;
            microHidPickup.NetworkEnergy += plugin.Config.Charging.Idle.Amount;
        }

        private void ChargeItems()
        {
            foreach (Player player in Player.List)
            {
                foreach (Item item in player.Items)
                {
                    if (item.Base is MicroHIDItem microHidItem)
                    {
                        ChargeItem(microHidItem);
                    }
                }
            }
        }

        private void ChargeItem(MicroHIDItem microHidItem)
        {
            if (!chargeRates.TryGetValue(microHidItem.State, out ICharge charge))
                return;

            if (!itemTimers.ContainsKey(microHidItem))
                itemTimers.Add(microHidItem, 0f);

            itemTimers[microHidItem] += Time.deltaTime;
            if (itemTimers[microHidItem] < charge.Interval)
                return;

            itemTimers[microHidItem] = 0f;
            microHidItem.RemainingEnergy += charge.Amount;
            microHidItem.ServerSendStatus(HidStatusMessageType.EnergySync, microHidItem.EnergyToByte);
        }
    }
}