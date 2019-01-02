using System;
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

        /* When you lose health/used stamina, it takes this many seconds until you start to regen */
        private int secondsUntilHealthRegen = 3;
        private int secondsUntilStaminaRegen = 2;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.OneSecondUpdateTicked += this.oneSecond;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void Timer (int secondsUntilHealthRegen)
        {

        }

        private void oneSecond(object sender, EventArgs e)
        {
            /* Variables */
            var player = Game1.player;

            if (!Context.IsWorldReady || !Context.IsPlayerFree || !Game1.paused)
            {
                 return;
            }

            /* Health Regen */ 
            if (this.secondsUntilHealthRegen == 0)
            {
                if (player.health < player.maxHealth)
                {
                    player.health = Math.Min(player.maxHealth, player.health + this.Config.HealthRegenRate);
                }
            }

            if (this.secondsUntilStaminaRegen == 0)
            {
                /* Stamina Regen */
                if (player.Stamina < player.MaxStamina)
                {
                    player.Stamina = Math.Min(player.MaxStamina, player.Stamina + this.Config.StaminaRegenRate);
                }
            }
        } 
    }
}
