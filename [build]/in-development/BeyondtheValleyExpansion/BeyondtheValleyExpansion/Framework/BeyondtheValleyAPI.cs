using BeyondTheValleyExpansion.Framework.ContentPacks;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace BeyondTheValleyExpansion.Framework
{
    public class BeyondtheValleyAPI
    {
        AvailableEdits _NewEdit = new AvailableEdits();
        void LoadNewAsset(string ReplaceFile, string FromFile)
        {
            if (ReplaceFile == RefFile.bveFarm)
            {
                _NewEdit.api_newFarm = RefMod.ModHelper.Content.Load<Map>(FromFile);
            }
        }
    }
}
