using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Huevy.Lib.Core
{
    public sealed class ColorBucket
    {
        private readonly Dictionary<Color, int> _innerBucket;

        public ColorBucket()
        {
            _innerBucket = new Dictionary<Color, int>();
        }

        internal IReadOnlyDictionary<Color, int> Dictionary
        {
            get { return _innerBucket; }
        }

        public void Add(byte red, byte green, byte blue)
        {
            //var color = (red << 16) | (green << 8) | blue;
            var color = Color.FromArgb(
                FindClosest32Byte(red),
                FindClosest32Byte(green),
                FindClosest32Byte(blue)
            );
            int amount;
            if (_innerBucket.TryGetValue(color, out amount))
                _innerBucket[color] = amount + 1;
            else
                _innerBucket.Add(color, 1);
        }

        // Round on 40
        private static byte FindClosest40Byte(byte input)
        {
            const double rounding = 40d;
            return (byte)(Math.Round(input / rounding) * rounding);
        }

        // Round on 32
        private static byte FindClosest32Byte(byte input)
        {
            // 1110 0000

            // < 0010 0000
            if (input < 0x20) return 0x00;
            // > 1110 0000
            if (input >= 0xE0) return 0xFF;

            // ###* ****
            return (byte)(input & 0xE0);
        }

        // Round on 64
        private static byte FindClosest64Byte(byte input)
        {
            // 1110 0000
            // Trim bytes before 32: & 0xE0

            // < 0100 0000
            if (input < 0x40) return 0x00;
            // > 1100 0000
            if (input >= 0xC0) return 0xFF;

            // ##** ****
            return (byte)(input & 0xC0);
        }
    }
}
