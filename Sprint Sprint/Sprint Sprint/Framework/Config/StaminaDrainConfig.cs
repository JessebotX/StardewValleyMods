using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_Sprint.Framework.Config
{
    class StaminaDrainConfig
    {
        /// <summary> Check if stamina draining is on </summary>
        public bool Enabled { get; set; }
        /// <summary> How much stamina lost every second </summary>
        public float StaminaCost { get; set; }
    }
}
