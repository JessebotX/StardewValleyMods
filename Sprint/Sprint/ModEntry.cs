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

        //reference buff
        private Buff sprintingBuff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 1, "Sprinting", "Sprinting");

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            helper.Events.Input.ButtonPressed += this.InputButtonPressed;
            helper.Events.Input.ButtonReleased += this.InputButtonReleased;
            helper.Events.GameLoop.UpdateTicked += this.GameLoopUpdateTicked;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.GameLoopOneSecond;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        //when button is pressed
        private void InputButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if Context
            if (e.Button == this.Config.SprintKey)
            {
                playerSprinting = true;
                Game1.buffsDisplay.addOtherBuff(sprintingBuff);
            }


        }

        //when button is released
        private void InputButtonReleased(object sender, ButtonReleasedEventArgs e)
        {

        }

        private void GameLoopUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                this.Helper.Input.Suppress(this.Config.SprintKey); //suppresses game keybind so that you can use the sprint key
                this.Helper.Input.Suppress(this.Config.WalkKey); //suppresses game keybind so that you can use the walk key
                if (playerSprinting)
                {
                    sprintingBuff.millisecondsDuration = 5000;
                }
            }
        }    

        private void GameLoopOneSecond(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (playerSprinting && !Game1.paused && Game1.player.isMoving())
            {
                Game1.player.Stamina = Math.Min(Game1.player.MaxStamina, Game1.player.Stamina - 0.5f);
            }
        }
    }
}
