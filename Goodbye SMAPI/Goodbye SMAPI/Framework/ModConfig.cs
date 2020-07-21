using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodbye_SMAPI.Framework
{
    class ModConfig
    {
        public string[] CommandInput { get; set; } =
        {
            "Goodbye", "Bye"
        };

        public string[] Responses { get; set; } =
        {
            "Goodbye"
        };
    }
}
