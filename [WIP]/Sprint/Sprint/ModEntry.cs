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

        private bool playerSprinting = false;
        private int sprintSpeed = 0;
        private int secondsUntilIncrement = 4;

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OneSecond;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void OneSecond(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                if (playerSprinting)
                {
                    if (secondsUntilIncrement > 0)
                    {
                        secondsUntilIncrement--;
                    }
                }
            }
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
                    playerSprinting = true;
                    Game1.player.Speed += sprintSpeed;

                    if (secondsUntilIncrement == 4 )
                    {
                        sprintSpeed = 3;
                    }
                    else if (secondsUntilIncrement <= 3 && secondsUntilIncrement > 1)
                    {
                        sprintSpeed = 4;
                    }
                    else if (secondsUntilIncrement <= 1)
                    {
                        sprintSpeed = 5;
                    }
                }
            }
        }
    }
}
