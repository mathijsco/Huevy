using Huevy.Lib.Core;
using System;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class TopColorAnalyzer : IColorAnalyzer
    {
        private const float ErrorSize = 0.05f; // 5% error size

        public readonly int[] _hues;
        public readonly int[] _saturation;
        public readonly int[] _brightness;

        public TopColorAnalyzer()
        {
            _hues = new int[360];
            _saturation = new int[256];
            _brightness = new int[256];
        }

        public void Add(TinyColor color)
        {
            _hues[(int)color.Hue]++;
            _saturation[(int)(color.Saturation * 255)]++;
            _brightness[(int)(color.Brightness * 255)]++;
        }


        public TinyColor FindColor()
        {
            // TOP 1 COLOR, AND KEEP UNTIL CHANGED ENOUGH.
            //var targetColors = bucket.Dictionary.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(3).ToList();
            // Ignore if previous color is still in the TOP 3
            //if (targetColors.Any(c => c.Equals(_currentColor)))
            //    return;

            var h = IndexOfMax(_hues, (int)(360*ErrorSize)+1);
            var s = IndexOfMax(_saturation, (int)(256 * ErrorSize) + 1) / 255f;
            var b = IndexOfMax(_brightness, (int)(256* ErrorSize) + 1) / 255f;

            return TinyColor.FromHsb(h, s, b);
        }

        private static int IndexOfMax(int[] collection, int errorSize)
        {
            var maxIndex = 0;
            var maxValue = 0;
            for (int i = 0; i <= collection.Length - errorSize; i++)
            {
                var currentValue = 0;
                for (int j = 0; j < errorSize; j++)
                    currentValue += collection[i+j];

                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                    maxIndex = i;
                }
            }
            return maxIndex + (int)Math.Floor(errorSize/2d);
        }
    }
}
