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

        private bool sprintBuffExists = false;
        private bool playerSprinting = false;
        private int secondsUntilIncrement = 4;
        private int buffDuration = 1000;

        //reference buff
        private Buff sprintingBuff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, "Sprinting", "Sprinting");

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            helper.Events.Input.ButtonPressed += this.ButtonPressed;
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
                    if (sprintBuffExists)
                    {
                        return;
                    }

                    else
                    {
                        playerSprinting = true;

                        sprintingBuff.millisecondsDuration = buffDuration;
                    }
                }

                else
                {
                    playerSprinting = false;
                }
            }
        }
        private void ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                if (playerSprinting)
                {
                    Game1.buffsDisplay.addOtherBuff(sprintingBuff);
                    sprintBuffExists = true;
                }
            }
        }
    }
}
