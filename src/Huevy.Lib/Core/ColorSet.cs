using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huevy.Lib.Core
{
    public sealed class ColorSet
    {
        private HueColor[] _colors;

        public ColorSet()
        {
            _colors = new HueColor[Enum.GetValues(typeof(ColorPosition)).Length];
        }

        public HueColor this[ColorPosition position]
        {
            get
            {
                return _colors[(int)position];
            }
            set
            {
                _colors[(int)position] = value;
            }
        }
    }
}
