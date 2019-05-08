using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondTheValleyExpansion.Framework.Alchemy
{
    class WriteAlchemyData
    {
        private AlchemyDataModel _AlchemyDataModel = new AlchemyDataModel();

        public void DefaultData()
        {
            _AlchemyDataModel.itemData.Add(1, "e");
        }
    }
}
