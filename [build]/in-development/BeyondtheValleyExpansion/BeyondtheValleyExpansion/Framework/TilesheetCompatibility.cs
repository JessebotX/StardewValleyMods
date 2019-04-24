using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondTheValleyExpansion.Framework
{
    class TilesheetCompatibility
    {
        /// <summary>The relative path to the folder containing tilesheet variants.</summary>
        private readonly string TilesheetsPath = Path.Combine("assets", "Maps", "FarmMaps", "tilesheets");

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
