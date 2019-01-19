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
        
        /*-Buffs-*/
        private Buff sprintBuff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, "Sprint", "Sprint");
        //-------//

        private bool playerSprinting = false;

        public override void Entry(IModHelper helper)
        {
            /* Event Handlers */
            this.Helper.Events.Input.ButtonPressed += this.ButtonPressed;
            this.Helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsPlayerFree)
            {
                return;
            }

            bool sprintKeyPressed = this.Helper.Input.IsDown(this.Config.SprintKey | this.Config.ControllerSprintButton);

            if (sprintKeyPressed)
            {
                playerSprinting = true;
                Game1.buffsDisplay.addOtherBuff(sprintBuff);
            }
        }

        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            this.Helper.Input.Suppress(this.Config.SprintKey);
            this.Helper.Input.Suppress(this.Config.ControllerSprintButton);
            if (playerSprinting)
            {
                sprintBuff.millisecondsDuration = 5000;
            }
        }
    }
}
