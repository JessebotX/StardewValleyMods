using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondTheValleyExpansion.Framework
{
    class Alchemy
    {
        /// <summary> if alchemy feature is unlocked </summary>
        public bool unlockedAlchemy;
        /// <summary> stores item ids that are currently in used </summary>
        public List<int> itemsInUsed = new List<int>();
        /// <summary> </summary>
        public List<Response> alchemyMenuChoices = new List<Response>();

    }
}
