using System;

namespace Huevy.Lib.Core
{
    public sealed class ColorSet
    {
        private readonly ColorBucket[] _colors;

        public ColorSet()
        {
            _colors = new ColorBucket[Enum.GetValues(typeof(ColorPosition)).Length];
            for (int n = 0; n < _colors.Length; n++)
                _colors[n] = new ColorBucket();
        }

        public ColorBucket this[ColorPosition position]
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
