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
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;

            /* Read Config */
            this.Config = helper.ReadConfig<ModConfig>();
        }

        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            this.Helper.Input.Suppress(this.Config.SprintKey);
            bool isSprintKey = this.Helper.Input.IsDown(this.Config.SprintKey);

            sprintingBuff.millisecondsDuration = 5000;
            if (!Context.IsPlayerFree)
            {
                return;
            }

            else
            {
                if (isSprintKey && !SprintBuffExists())
                {
                    playerSprinting = true;

                    Game1.buffsDisplay.addOtherBuff(sprintingBuff);
                }

                else
                {
                    Game1.buffsDisplay.otherBuffs.Remove(sprintingBuff);
                    sprintingBuff.removeBuff();
                    Game1.buffsDisplay.syncIcons();
                }
            }
        }

        private bool SprintBuffExists()
        {
            if (sprintingBuff == null)
            {
                return false;
            }

            return Game1.buffsDisplay.otherBuffs.Contains(sprintingBuff);
        }
    }
}
