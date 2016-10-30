using Huevy.Lib.Core;
using System;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class TopColorAnalyzer : IColorAnalyzer
    {
        private const float ErrorSize = 0.05f; // 5% error size

        public readonly int[] _hues;
        public readonly int[,] _saturation;
        public readonly int[] _brightness;

        public TopColorAnalyzer()
        {
            _hues = new int[360];
            _saturation = new int[360, 256];
            _brightness = new int[256];
        }

        public void Add(TinyColor color)
        {
            int hueIndex = (int)color.Hue;
            _hues[hueIndex]++; // Always add one to the index 0. Will be used to calculate how much of the HUE in total.
            _saturation[hueIndex, (int)(color.Saturation * 255)]++;
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
            var s = IndexOfMaxUpper(_saturation, (int)(256 * ErrorSize) + 1, h) / 255f;
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

        private static int IndexOfMaxUpper(int[,] collection, int errorSize, int posLow)
        {
            var maxIndex = 0;
            var maxValue = 0;
            for (int i = 1; i <= collection.GetUpperBound(1) - errorSize; i++)
            {
                var currentValue = 0;
                for (int j = 0; j < errorSize; j++)
                    currentValue += collection[posLow, i + j];

                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                    maxIndex = i;
                }
            }
            return maxIndex + (int)Math.Floor(errorSize / 2d);
        }
    }
}
