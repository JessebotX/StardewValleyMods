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
using StardewModdingAPI.Utilities;
using xTile;
using xTile.Layers;
using xTile.Tiles;
using System.IO;
using BeyondTheValley.Framework;
using BeyondTheValley.Framework.Actions;
using StardewValley.Tools;

namespace BeyondTheValley
{
    class ModEntry : Mod, IAssetLoader
    {
        /*********
        ** Fields
        *********/
        /// <summary> 'helper.Translation' becomes 'i18n' </summary>
        public ITranslationHelper i18n;

        /// <summary> Create Instance of TileActionFramework class <see cref="TileActionFramework"/> </summary>
        public TileActionFramework TileActionFramework = new TileActionFramework();

        /* content pack replacement */
        private Map editFarm;
        private Map editFarm_Foraging;

        // content pack bools
        /// <summary> content pack replaces Farm </summary>
        private bool replaceFarm;
        /// <summary> content pack replaces Farm </summary>
        private bool replaceFarm_Foraging;

        /* other */
        /// <summary> How many Content Packs are installed </summary>
        private int contentPacksInstalled;

        /*********
        ** Entry
        *********/
        public override void Entry(IModHelper helper)
        {
            this.i18n = helper.Translation;

            /* other methods */
            ContentPackData();

            /* Helper Events */
            helper.Events.GameLoop.SaveLoaded += this.SaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;
            helper.Events.Multiplayer.ModMessageReceived += this.ModMessageReceived;
            helper.Events.Input.ButtonPressed += this.ButtonPressed;
        }

        private void ContentPackData()
        {
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                // bool for if content.json exists
                bool contentFileExists = File.Exists(Path.Combine(contentPack.DirectoryPath, "content.json"));

                //read content packs
                ContentPackModel pack = contentPack.ReadJsonFile<ContentPackModel>("content.json");
                this.Monitor.Log($"Reading: {contentPack.Manifest.Name} {contentPack.Manifest.Version} by {contentPack.Manifest.Author} from {contentPack.DirectoryPath} (ID: {contentPack.Manifest.UniqueID})", LogLevel.Trace);

                // if content.json does not exists
                if (!contentFileExists)
                    this.Monitor.Log($"{contentPack.Manifest.Name}({contentPack.Manifest.Version}) by {contentPack.Manifest.Author} is missing a content.json file. Mod will be ignored", LogLevel.Warn);

                // if content.json exists
                else if (contentFileExists)
                    contentPacksInstalled += 1;

                foreach (BVEEditModel edit in pack.ReplaceFiles)
                {
                    this.Monitor.Log($"Replacing {edit.ReplaceFile} with {edit.FromFile}", LogLevel.Trace);
                    /* Check if content pack replaces one of the following files */
                    /// <summary> 
                    /// If content pack replaces Farm/Standard Farm 
                    /// </summary>
                    if (edit.ReplaceFile == "assets/Maps/FarmMaps/Farm.tbin")
                    {
                        editFarm = contentPack.LoadAsset<Map>(edit.FromFile);
                        replaceFarm = true;
                        continue;
                    }

                    /// <summary> 
                    /// If content pack replaces Farm_Combat/Wilderness Farm 
                    /// </summary>
                    if (edit.ReplaceFile == "assets/Maps/FarmMaps/Farm_Foraging.tbin")
                    {
                        editFarm_Foraging = contentPack.LoadAsset<Map>(edit.FromFile);
                        replaceFarm_Foraging = true;
                        continue;
                    }
                }
            }
        }

        /*********
        ** Content API crap
        *********/
        public bool CanLoad<T>(IAssetInfo asset)
        {
            // Standard Farm/Farm
            if (asset.AssetNameEquals("Maps/Farm"))
                return true;

            if (asset.AssetNameEquals("Maps/Farm_Combat"))
                return true;

            else
                return false;
        }

        public T Load<T>(IAssetInfo asset)
        {
            // Standard Farm/Farm
            if (!replaceFarm && asset.AssetNameEquals("Maps/Farm"))
                return this.Helper.Content.Load<T>("assets/Maps/FarmMaps/Farm.tbin");

            else if (replaceFarm && asset.AssetNameEquals("Maps/Farm"))
                return (T)(object)editFarm;

            // Forest Farm/Farm_Foraging
            if (!replaceFarm_Foraging && asset.AssetNameEquals("Maps/Farm_Foraging"))
                return this.Helper.Content.Load<T>("assets/Maps/FarmMaps/Farm_Foraging.tbin");

            else if (replaceFarm_Foraging && asset.AssetNameEquals("Maps/Farm_Foraging"))
                return (T)(object)editFarm_Foraging;

            throw new NotSupportedException($"Unexpected asset '{asset.AssetName}'.");
        }

        // ---------------------------- \\

        private void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            this.Monitor.Log($"{contentPacksInstalled} content packs installed for Beyond the Valley");

            TileActionFramework.SaveDeleteTilesAction();
        }

        private void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            /* --- (Multiplayer) sync deleted tiles from tile actions --- */
            if (TileActionFramework.tileRemoved == true && !Context.IsMainPlayer)
            {
                TileActionFramework.MultiplayerDeleteTilesAction();

                TileActionFramework.tileRemoved = false;
            }
        }

        private void ModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            // read list
            if (e.FromModID == "Jessebot.BeyondTheValley" && e.Type == "DeletedTiles")
            {
                TileActionFramework.mpInputArgs = e.ReadAs<List<string>>();
            }
        }

        private void ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Custom \\
             // Tile \\
            // Actions \\
            if (e.Button.IsActionButton())
            {
                // grabs player's cursor xy coords
                Vector2 tile = e.Cursor.GrabTile;

                string tileAction = Game1.player.currentLocation.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Action", "Buildings");

                if (tileAction != null)
                {
                    // --- General Tile Actions --- \\

                    /// <summary> 
                    /// Action | BVEMessage (strMessage)
                    /// If interacted, prints out a message from the i18n key 
                    /// </summary>
                    if (tileAction.StartsWith("BVEMessage "))
                    {
                        string[] input = tileAction.Split(' ').Skip(1).ToArray();

                        //get i18n key
                        string strMessage = i18n.Get(input[0]);

                        //print's message out
                        Game1.drawObjectDialogue(strMessage);
                    }

                    // --- Delete Tiles Actions --- \\

                    // pickaxe
                    /// <summary>
                    /// Action | BVECopperPickaxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Copper pickaxe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVECopperPickaxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 1;

                        /// <summary> Calls PickaxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.PickaxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.PickaxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                    /// <summary>
                    /// Action | BVESteelPickaxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Steel pickaxe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVESteelPickaxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 2;

                        /// <summary> Calls PickaxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.PickaxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.PickaxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                    /// <summary>
                    /// Action | BVEGoldPickaxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Gold pickaxe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVEGoldPickaxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 3;

                        /// <summary> Calls PickaxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.PickaxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.PickaxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                    /// <summary>
                    /// Action | BVEIridiumPickaxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Iridium pickaxe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVEIridiumPickaxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 4;

                        /// <summary> Calls PickaxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.PickaxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.PickaxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }

                    // axe
                    /// <summary>
                    /// Action | BVECopperAxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Copper axe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVECopperAxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 1;

                        /// <summary> Calls AxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.AxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.AxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }

                    /// <summary> 
                    /// Action | BVESteel Axe (coordX) (coordY) (strLayer
                    /// If interacted with your Steel axe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVESteelAxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 2;

                        /// <summary> Calls AxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.AxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.AxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                    /// <summary> 
                    /// Action | BVEGold Axe (coordX) (coordY) (strLayer)
                    /// If interacted with your Gold axe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVEGoldAxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 3;

                        /// <summary> Calls AxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.AxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.AxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                    /// <summary> 
                    /// Action | BVEIridiumAxe (coordX) (coordY) (strLayer)
                    /// If interacted with your Iridium axe(+) equipped, it will remove the following tiles on that layer, separate with '/' delimiter 
                    /// </summary>
                    if (tileAction.StartsWith("BVEIridiumAxe "))
                    {
                        // process to get the first word in the current tile action
                        string[] fullString = tileAction.Split(' ').ToArray();
                        string currentAction = fullString[0];
                        int toolUpgradeLevel = 4;

                        /// <summary> Calls AxeDeleteTilesAction in TileActionFramework <see cref="TileActionFramework.AxeDeleteTilesAction(string, string, int)"/> </summary>
                        TileActionFramework.AxeDeleteTilesAction(tileAction, currentAction, toolUpgradeLevel);
                    }
                }
            }
        }
    }
}