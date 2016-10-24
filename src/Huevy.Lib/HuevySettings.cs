using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huevy.Lib
{
    public sealed class HuevySettings
    {
        public TimeSpan UpdateTime { get; set; }

        // 0 ~ 1
        public float MinBrightness { get; set; }

        // 0 ~ 1
        public float MaxBrightness { get; set; }
    }
}
