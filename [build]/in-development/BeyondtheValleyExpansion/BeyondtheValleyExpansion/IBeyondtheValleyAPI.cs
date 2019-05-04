namespace BeyondTheValleyExpansion
{
    /// <summary> Interface to provide an API for Jessebot.BeyondtheValley </summary>
    public interface IBeyondtheValleyAPI
    {
        /// <summary> Load a new asset instead of the Default/Content Pack edit </summary>
        /// <param name="ReplaceFile"> the file to replace relative to "Jessebot.BeyondtheValley"'s root folder </param>
        /// <param name="FromFile"> the file to load instead relative to your mod's root folder </param>
        void LoadNewAsset(string ReplaceFile, string FromFile);
    }
}
