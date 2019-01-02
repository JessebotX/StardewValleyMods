﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace HealthStaminaRegen
{
    class ModEntry : Mod
    {
        private ModConfig Config;

        private int secondsUntilHealthRegen = 3;
        private int secondsUntilStaminaRegen = 3;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.OneSecondUpdateTicked += this.oneSecond;

            /* Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void oneSecond(object sender, EventArgs e)
        {
            var player = Game1.player;

            if (secondsUntilHealthRegen == 0)
            {
                if (!Context.IsWorldReady || !Context.IsPlayerFree)
                {
                    return;
                }

                if (player.health < player.maxHealth)
                {
                    player.health = Math.Min(player.maxHealth, player.health + this.Config.HealthRegenRate);
                }
            }

            if (secondsUntilStaminaRegen == 0)
            {
                if (!Context.IsWorldReady || !Context.IsPlayerFree)
                {
                    return;
                }

                if (player.Stamina < player.MaxStamina)
                {
                    player.Stamina = Math.Min(player.MaxStamina, player.Stamina + this.Config.StaminaRegenRate);
                }
            }
        } 
    }
}