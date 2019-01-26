using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewModdingAPI.Framework;
using StardewModdingAPI.Utilities;
using StardewModdingAPI;

namespace Sprint
{
    class ModConfig
    {
        public SButton SprintKey { get; set; } = SButton.LeftShift;
        public bool StaminaDrain { get; set; } = true;
        public float StaminaDrainPerSecond { get; set; } = 0.5f;
    }
}
