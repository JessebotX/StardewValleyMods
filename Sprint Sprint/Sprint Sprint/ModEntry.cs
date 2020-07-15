using Sprint_Sprint.Framework;
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

        #endregion

        #region Methods

        /// <summary> The mod's entry point </summary>
        /// <param name="helper"> Provides simplified APIs </param>
        public override void Entry(IModHelper helper)
        {
            // pass stuff to other classes
            this.ModIntegration = new Integrations(this.Helper, this.ModManifest, this.Config);

            // read config
            this.Config = helper.ReadConfig<ModConfig>();

            /* Events */
            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
        }

        /// <summary> 
        /// Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). 
        /// All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.  
        /// </summary>
        /// <param name="sender"> The object's sender </param>
        /// <param name="e"> The event arguments </param>
        private void GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.ModIntegration.GenericModConfigMenuApi();
        }

        /// <summary> Raised before/after the game state is updated (≈60 times per second) </summary>
        /// <param name="sender"> The object's sender </param>
        /// <param name="e"> The event arguments </param>
        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsPlayerFree || !Context.IsWorldReady || !Game1.paused)
                return;
        }

        #endregion
    }
}
