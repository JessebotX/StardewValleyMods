using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Framework;
using StardewModdingAPI.Events;
using xTile;

namespace BeyondTheValley
{
    class IAssetLoader
    {
        Farm_Loader loader = new Farm_Loader;
        public bool CanLoad<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Maps/Farm.tbin");
        }

        public T Load<T>(IAssetInfo asset)
        {
            return this.Helper.Content.Load<T>("assets/Maps/FarmMaps/Farm.tbin");
        }
    }
}
