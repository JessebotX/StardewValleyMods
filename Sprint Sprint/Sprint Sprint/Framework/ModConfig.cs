using Sprint_Sprint.Framework.Config;
using StardewModdingAPI;

namespace Sprint_Sprint.Framework
{
    class ModConfig
    {
        /// <summary> The sprinting keybind </summary>
        public SButton SprintKey { get; set; } = SButton.LeftShift;
        /// <summary> The sprinting speed </summary>
        public int SprintSpeed { get; set; } = 5;
        /// <summary> Disable sprinting when player is too tired </summary>
        public bool NoSprintWhenOverExertion { get; set; } = true;
        /// <summary> Modify stamina draining settings </summary>
        public StaminaDrainConfig StaminaDrain { get; set; } = new StaminaDrainConfig();
    }
}
