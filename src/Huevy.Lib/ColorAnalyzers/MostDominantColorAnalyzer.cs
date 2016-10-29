using System.Collections.Generic;
using System.Linq;
using Huevy.Lib.Core;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class MostDominantColorAnalyzer : IColorAnalyzer
    {
        private readonly Dictionary<TinyColor, int> _dictionary;

        public MostDominantColorAnalyzer()
        {
            _dictionary = new Dictionary<TinyColor, int>();
        }

        public void Add(TinyColor color)
        {
            // TODO: Add error margin for the color

            int amount;
            if (_dictionary.TryGetValue(color, out amount))
                _dictionary[color] = amount + 1;
            else
                _dictionary.Add(color, 1);
        }

        public TinyColor FindColor()
        {
            return _dictionary.OrderByDescending(pair => pair.Value * ColorIntensity(pair.Key)).Select(pair => pair.Key).First();
        }


        private static float ColorIntensity(TinyColor color)
        {
            var saturation = color.Saturation; // Sweetspot = 1
            var brightness = color.Brightness; // Sweetspot = 0.5
            if (brightness > 0.5)
                brightness = 1 - brightness;

            // Linear
            return saturation / 2 + brightness;
            // Arc
            //return (float)Math.Pow(saturation / 2 + brightness, 2);
        }
    }
}
