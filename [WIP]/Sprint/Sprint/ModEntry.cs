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
        /************
         ***Fields***
         ************/
        private ModConfig Config;

        private bool playerSprinting = false;

        private int addedSprintSpeed = 0;

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            this.Helper.Events.GameLoop.OneSecondUpdateTicked += this.OneSecond;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            //if player isn't free to act in the world, do nothing
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                //suppress game keybinds depending on config values
                this.Helper.Input.Suppress(this.Config.SprintKey);
                this.Helper.Input.Suppress(this.Config.ControllerSprintButton);
                this.Helper.Input.Suppress(this.Config.SlowDownKey);

                //is the key/button being pressed
                bool isSprintKeyPressed = this.Helper.Input.IsDown(this.Config.SprintKey);
                bool isControllerSprintButtonPressed = this.Helper.Input.IsDown(this.Config.ControllerSprintButton);
                bool isWalkKeyPressed = this.Helper.Input.IsDown(this.Config.SlowDownKey);

                if (isSprintKeyPressed || isControllerSprintButtonPressed && Game1.player.isMoving())
                {
                    playerSprinting = true;
                }

                else if (isWalkKeyPressed && Game1.player.isMoving())
                {
                    Game1.player.canOnlyWalk = true;
                }
            }
        }

        private void OneSecond(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (playerSprinting == true)
            {
                addedSprintSpeed++;
                Game1.player.Speed += addedSprintSpeed;
                if (addedSprintSpeed > 5)
                {
                    return;
                }
            }
        }
    }
}
