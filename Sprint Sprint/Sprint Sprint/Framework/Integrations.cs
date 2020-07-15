using StardewModdingAPI;

namespace Sprint_Sprint.Framework
{
    /// <summary> All mod integrations. </summary>
    class Integrations
    {
        #region Fields & Properties

        private IModHelper Helper;
        private IManifest ModManifest;
        private ModConfig Config;

        #endregion

        #region Methods

        /// <summary> Access mod integrations </summary>
        /// <param name="helper"> Provides simplified APIs for writing mods </param>
        /// <param name="manifest"> A manifest which describes a mod for SMAPI </param>
        /// <param name="config"> Access mod configuration settings </param>
        public Integrations(IModHelper helper, IManifest manifest, ModConfig config)
        {
            this.Helper = helper;
            this.ModManifest = manifest;
            this.Config = config;
        }

        /// <summary> Access the GenericModConfigMenu api </summary>
        public void GenericModConfigMenuApi()
        {
            var api = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");

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
