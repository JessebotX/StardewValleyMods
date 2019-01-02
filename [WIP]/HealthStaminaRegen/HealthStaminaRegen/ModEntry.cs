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

        private int IncrementHealth;

        private int IncrementStamina;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.OneSecondUpdateTicked += this.oneSecond;
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void oneSecond(object sender, EventArgs e)
        {
            var player = Game1.player;

            if (player.health + IncrementHealth > Game1.player.maxHealth)
            {

            }

            if (player.stamina + IncrementStamina > Game1.player.MaxStamina)
            {

            }
        } 
    }
}
