using StardewModdingAPI;

namespace Sprint_Sprint.Framework
{
    /// <summary> All mod integrations. </summary>
    class Integrations
    {
        #region Fields & Properties

        /// <summary> Provides simplified APIs for writing mods </summary>
        private IModHelper Helper;
        /// <summary> Encapsulates monitoring and logging for a given module </summary>
        private IMonitor Monitor;
        /// <summary> A manifest whicch describes a mod for SMAPI </summary>
        private IManifest ModManifest;
        /// <summary> Access mod configuration settings </summary>
        private ModConfig Config;

        #endregion

        #region Methods

        /// <summary> Access mod integrations </summary>
        /// <param name="helper"> Provides simplified APIs for writing mods </param>
        /// <param name="manifest"> A manifest which describes a mod for SMAPI </param>
        /// <param name="config"> Access mod configuration settings </param>
        public Integrations(IModHelper helper, IMonitor monitor, IManifest manifest, ModConfig config)
        {
            this.Helper = helper;
            this.Monitor = monitor;
            this.ModManifest = manifest;
            this.Config = config;
        }

        /// <summary> Warn users who have Sprint Sprint Sprint (deprecated version) installed </summary>
        public void SprintSprintSprintWarning()
        {
            if (this.Helper.ModRegistry.IsLoaded("Jessebot.SprintSprintSprint"))
                this.Monitor.Log("WARNING: Sprint Sprint Sprint (older version) is installed with Sprint Sprint (this mod). Please delete the Sprint folder in your Mod's folder", LogLevel.Alert);
        }

        /// <summary> Access the GenericModConfigMenu api </summary>
        public void GenericModConfigMenuApi()
        {
            var api = this.Helper.ModRegistry.GetApi<IGenericModConfigAPI>("spacechase0.GenericModConfigMenu");

            api.RegisterModConfig(this.ModManifest, () => this.Config = new ModConfig(), () => Helper.WriteConfig(this.Config));
            api.RegisterSimpleOption(this.ModManifest, "Sprint Keybind", "The key to hold in order to sprint",
                () => this.Config.SprintKey, (SButton val) => this.Config.SprintKey = val);
            api.RegisterSimpleOption(this.ModManifest, "Sprint Speed", "The sprint speed value. Higher value = higher movement speed. Must be an integer",
                () => this.Config.SprintSpeed, (int val) => this.Config.SprintSpeed = val);
            api.RegisterSimpleOption(this.ModManifest, "Disable Sprinting When Over-Exertion", "Disable sprinting when farmer is too tired",
                () => this.Config.NoSprintWhenOverExertion, (bool val) => this.Config.NoSprintWhenOverExertion = val);
            api.RegisterSimpleOption(this.ModManifest, "Enable Stamina Drain", "Enable stamina draining when sprinting",
                () => this.Config.StaminaDrain.Enabled, (bool val) => this.Config.StaminaDrain.Enabled = val);
            api.RegisterSimpleOption(this.ModManifest, "Stamina Drain Cost", "Amount of stamina loss every second. Decimal values accepted",
                () => this.Config.StaminaDrain.StaminaCost, (float val) => this.Config.StaminaDrain.StaminaCost = val);
        }

        #endregion
    }
}
