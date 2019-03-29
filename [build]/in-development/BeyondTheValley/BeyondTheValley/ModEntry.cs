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
using xTile.Layers;
using xTile.Tiles;
using System.IO;
using BeyondTheValley.Framework;

namespace BeyondTheValley
{
    class ModEntry : Mod, IAssetLoader
    {
        /* content pack replacement */
        /// <summary> content pack replaces Farm </summary>
        public bool ReplaceFarm = false;
        /// <summary> content pack replaces Farm </summary>
        public bool ReplaceFarm_Foraging = false;

        public GameLocation Farm_Foraging = Game1.getLocationFromName("Farm_Foraging");

        public override void Entry(IModHelper helper)
        {
            /* --------- Content Packs ------------ */
            foreach (IContentPack ContentPack in this.Helper.ContentPacks.GetOwned())
            {
                bool ContentFileExists = File.Exists(Path.Combine(ContentPack.DirectoryPath, "content.json"));

                ContentPackModel Pack = ContentPack.ReadJsonFile<ContentPackModel>("content.json");
                this.Monitor.Log("Reading: {ContentPack.Manifest.Name} {ContentPack.Manifest.Version} by {ContentPack.Manifest.Author} from {ContentPack.DirectoryPath} (ID: {ContentPack.Manifest.UniqueID})", LogLevel.Trace);

                if (!ContentFileExists)
                    this.Monitor.Log("{ContentPack.Manifest.Name}({ContentPack.Manifest.Version}) by {ContentPack.Manifest.Author} is missing a content.json file. Mod will be ignored", LogLevel.Warn);

                foreach (ReplaceFileModel ContentPackEdit in Pack.ReplaceFiles)
                {
                    this.Monitor.Log($"Replacing {ContentPackEdit.ReplaceFile} with {ContentPackEdit.FromFile}", LogLevel.Trace);

                    /* Check if content pack replaces one of the following files */
                    /// <summary> If content pack replaces Farm/Standard Farm </summary>
                    if (ContentPackEdit.ReplaceFile == "assets/Maps/FarmMaps/Farm.tbin")
                        ReplaceFarm = true;
                    /// <summary> If content pack
                    if (ContentPackEdit.ReplaceFile == "assets/Maps/FarmMaps/Farm_Foraging.tbin")
                        ReplaceFarm_Foraging = true;
                }
            }
            //--------------------------------------//

            /* Helper Events */
            helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
            helper.Events.GameLoop.DayStarted += this.GameLoop_DayStarted;
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (!ReplaceFarm_Foraging)
            {
                if (Game1.player.mailReceived.Contains("ccVault"))
                {
                    //----------Farm_Foraging--------------------//
                    /// <summary> removes north fences on Forest Farm </summary>
                    Layer Farm_Foraging_Front = Farm_Foraging.map.GetLayer("Buildings");
                    TileSheet spring_outdoorsTileSheet = Farm_Foraging.map.GetTileSheet("untitled tile sheet");

                    Farm_Foraging.removeTile(61, 50, "Front");
                    Farm_Foraging.removeTile(62, 50, "Front");
                    Farm_Foraging.removeTile(61, 51, "Buildings");
                    Farm_Foraging.removeTile(62, 51, "Buildings");

                    for (int TileY = 53; TileY < 90; TileY++)
                    {
                        Farm_Foraging.removeTile(44, TileY, "Buildings");
                        Farm_Foraging.removeTile(44, TileY, "Front");
                    }

                    Farm_Foraging_Front.Tiles[44, 88] = new StaticTile(Farm_Foraging_Front, spring_outdoorsTileSheet, BlendMode.Alpha, tileIndex: 358);
                    //-------------------------------------------//
                }
            }
        }

        public bool CanLoad<T>(IAssetInfo asset)
        {
            // Standard Farm/Farm
            if (asset.AssetNameEquals("Maps/Farm"))
                return true;

            // Forest Farm/Farm_Foraging
            else if (asset.AssetNameEquals("Maps/Farm_Foraging"))
                return true;

            // Cindersap Forest
            else
                return asset.AssetNameEquals("Maps/Forest");
        }

        public T Load<T>(IAssetInfo asset)
        {
            //Standard Farm/Farm
            if (!ReplaceFarm && asset.AssetNameEquals("Maps/Farm"))
                return this.Helper.Content.Load<T>("assets/Maps/FarmMaps/Farm.tbin");

            else
                return this.Helper.Content.Load<T>("assets/Maps/FarmMaps/Farm_Foraging.tbin");
        }
    }
}