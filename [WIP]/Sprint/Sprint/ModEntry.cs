using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewModdingAPI.Framework;
using StardewModdingAPI.Utilities;

namespace Sprint
{
    class ModEntry : Mod
    {
        //reference ModConfig class
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                this.Helper.Input.Suppress(this.Config.SprintKey);
                bool sprintKeyPressed = this.Helper.Input.IsDown(this.Config.SprintKey);
                if (sprintKeyPressed)
                {
                    Game1.player.addedSpeed = 4;
                }
            }
        }
    }
}
