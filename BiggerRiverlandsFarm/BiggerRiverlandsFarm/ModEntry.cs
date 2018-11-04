using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace BiggerRiverlandsFarm
{

    public class ModEntry : Mod, IAssetLoader
    {
        private ModConfig Config;
        

        public bool CanLoad<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Maps/Farm_Fishing");
        }

        public override void Entry(IModHelper helper)
        {
            Config = this.Helper.ReadConfig<ModConfig>();
        }

        T IAssetLoader.Load<T>(IAssetInfo asset)
        {
            if (this.Config.ProgressionMap)
            {
                return this.Helper.Content.Load<T>("assets/Farm_Fishing.tbin", ContentSource. ModFolder);
            }

            else
            {
                return this.Helper.Content.Load<T>("assets/Farm_Fishing_ProgressionMap.tbin", ContentSource. ModFolder);
            }
        }
    }
}
