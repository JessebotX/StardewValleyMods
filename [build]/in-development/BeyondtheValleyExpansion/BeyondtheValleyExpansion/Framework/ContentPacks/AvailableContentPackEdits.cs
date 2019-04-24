using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace BeyondTheValleyExpansion.Framework.ContentPacks
{
    class AvailableContentPackEdits
    {
        /*********
         ** Check if asset is being replaced
         *********/
        public bool replaceFarm;
        public bool replaceFarm_Combat;
        public bool replaceFarm_Foraging;

        /*********
         ** Asset storage
         *********/ 
        // Farm Maps
        public Map newFarm;
        public Map newFarm_Combat;
        public Map newFarm_Foraging;
    }
}
