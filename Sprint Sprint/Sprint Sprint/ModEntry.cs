﻿using Sprint_Sprint.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Sprint_Sprint
{
    class ModEntry : Mod
    {
        #region Fields & Properties

        /// <summary> Access mod integrations </summary>
        private Integrations ModIntegration;
        /// <summary> Access the mod's configuration settings </summary>
        private ModConfig Config;

        /// <summary> Check if the player is trying to sprint </summary>
        private bool KeyActivated = false;
        private bool SpeedAdded = false;
        private bool SpeedReplaced = false;
        private int AddedSpeedBeforeMount = 0;

        #endregion

        #region Methods

        /// <summary> The mod's entry point </summary>
        /// <param name="helper"> Provides simplified APIs </param>
        public override void Entry(IModHelper helper)
        {
            // read config
            this.Config = helper.ReadConfig<ModConfig>();

            // pass stuff to other classes
            this.ModIntegration = new Integrations(this.Helper, this.Monitor, this.ModManifest, this.Config);

            /* Events */
            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
            helper.Events.Input.ButtonPressed += this.ButtonPressed;
        }

        /// <summary> 
        /// Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). 
        /// All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.  
        /// </summary>
        /// <param name="sender"> The object's sender </param>
        /// <param name="e"> The event arguments </param>
        private void GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.ModIntegration.SprintSprintSprintWarning(); // alert to player that you have an outdated version installed
            this.ModIntegration.GenericModConfigMenuApi(); // add a generic mod config menu if the framework exists
        }

        /// <summary> Raised before/after the game state is updated (≈60 times per second) </summary>
        /// <param name="sender"> The object's sender </param>
        /// <param name="e"> The event arguments </param>
        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsPlayerFree || !Context.IsWorldReady || Game1.paused)
                return;

            this.CheckIfHoldDownToSprint();
            this.Sprint(e);
        }

        /// <summary> Raised after the player pressed a keyboard, mouse, or controller button. This includes mouse clicks </summary>
        /// <param name="sender"> The object sender </param>
        /// <param name="e"> The event arguments </param>
        private void ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            this.CheckIfToggleToSprint(e);
        }

        /// <summary> Make player sprint </summary>
        /// <param name="e"> The event arguments </param>
        private void Sprint(UpdateTickedEventArgs e)
        {
            if (this.CheckIfPlayerCanSprint())
            {
                if (!this.SpeedAdded) 
                {
                    Game1.player.addedSpeed += this.Config.SprintSpeed;  ///Add speeds on top of other buffs instead replacing it, just like the Buff class does.   
                    this.SpeedAdded = true; ///Remember speed has been added already.
                }
                this.DepleteStamina(e);  ///player.position.X and player.position.Y could also be used here to avoid consuming stamine if the character isn't moving, like running against a wall. 
            }
            else if (Game1.player.mount != null) 
            {
                if (!this.SpeedReplaced) 
                {
                    this.AddedSpeedBeforeMount = Game1.player.addedSpeed;  ///Stores speed from other buffs before replacing it. 
                    this.SpeedReplaced = true;
                } else if (Game1.player.addedSpeed != this.Config.HorseSpeed) ///Gotta make sure other buffs didn't change addedSpeed while riding the horse, and update AddedSpeedBeforeMount otherwise
                    this.AddedSpeedBeforeMount -= this.Config.HorseSpeed - Game1.player.addedSpeed ///Remove the difference.
                Game1.player.addedSpeed = this.Config.HorseSpeed;  ///And finally overrides the addedSpeed with the Config setting. No sprint nor buffs on horse, just the horse's own speed! 
            }
            else 
            {
                if (this.SpeedAdded) ///Remove the added speed if it was added.  
                { 
                    Game1.player.addedSpeed -= this.Config.SprintSpeed;
                    this.SpeedAdded = false;
                }
                if (this.SpeedReplaced)  ///Reverts the speed added if it was replaced. 
                {
                    Game1.player.addedSpeed = this.AddedSpeedBeforeMount;
                    this.SpeedReplaced = false;
                }
            }
        }

        /// <summary> Check if the player can sprint or not </summary>
        /// <returns> Returns if the player can sprint or not </returns>
        private bool CheckIfPlayerCanSprint()
        {
            if (this.KeyActivated && Game1.player.mount == null)
            {
                if (this.Config.NoSprintIfTooTired.Enabled)
                {
                    if (Game1.player.Stamina > this.Config.NoSprintIfTooTired.TiredStamina)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            else
                return false;
        }

        /// <summary> Check if the player is holding down sprint key to sprint </summary>
        private void CheckIfHoldDownToSprint()
        {
            if (this.Config.HoldToSprint)
            {
                if (this.Helper.Input.IsDown(this.Config.SprintKey))
                    this.KeyActivated = true;
                else
                    this.KeyActivated = false;
            }
        }

        /// <summary> Check if the player has toggle to sprint on</summary>
        /// <param name="e"> The event arguments </param>
        private void CheckIfToggleToSprint(ButtonPressedEventArgs e)
        {
            if (!this.Config.HoldToSprint && e.Button == this.Config.SprintKey)
            {
                if (this.KeyActivated)
                    this.KeyActivated = false;
                else
                    this.KeyActivated = true;
            }
        }

        /// <summary> Deplete player stamina</summary>
        private void DepleteStamina(UpdateTickedEventArgs e)
        {
            if (this.Config.StaminaDrain.Enabled && e.IsMultipleOf(60))
                Game1.player.Stamina -= this.Config.StaminaDrain.StaminaCost;
        }

        #endregion
    }
}
