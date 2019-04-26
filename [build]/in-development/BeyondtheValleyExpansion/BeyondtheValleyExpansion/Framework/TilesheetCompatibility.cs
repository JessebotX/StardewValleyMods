using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile;
using xTile.Tiles;

namespace BeyondTheValleyExpansion.Framework
{
    class TilesheetCompatibility
    {
        /// <summary> The path to the folder containing tilesheet variants </summary>
        private readonly string TilesheetsPath = Path.Combine("assets", "Maps", "tilesheets");

        /// <summary> Phase 1 of attempting to apply tilesheet recolours </summary>
        /// <param name="map"> the map to apply the tilesheet recolours to </param>
        public void TilesheetRecolours(Map map)
        {
            DirectoryInfo compatFolder = this.GetCustomTilesheetFolder();
            if (compatFolder != null)
            {
                ModEntry.ModMonitor.Log($"Applying map tilesheets from {Path.Combine(this.TilesheetsPath, compatFolder.Name)}.", LogLevel.Trace);
                foreach (TileSheet tilesheet in map.TileSheets)
                {
                    string assetFileName = Path.GetFileName(tilesheet.ImageSource);
                    if (File.Exists(Path.Combine(compatFolder.FullName, assetFileName)))
                        tilesheet.ImageSource = ModEntry.ModHelper.Content.GetActualAssetKey(Path.Combine(this.TilesheetsPath, compatFolder.Name, assetFileName));
                }
            }
        }

        /// <summary> Load custom tilesheets if applicable </summary>
        public DirectoryInfo GetCustomTilesheetFolder()
        {
            // get root compatibility folder
            DirectoryInfo compatFolder = new DirectoryInfo(Path.Combine(ModEntry.ModHelper.DirectoryPath, this.TilesheetsPath));
            if (!compatFolder.Exists)
                return null;

            // get first folder matching an installed mod
            foreach (DirectoryInfo folder in compatFolder.GetDirectories())
            {
                if (folder.Name != "_default" && ModEntry.ModHelper.ModRegistry.IsLoaded(folder.Name))
                    return folder;
            }

            return null;
        }
    }
}
