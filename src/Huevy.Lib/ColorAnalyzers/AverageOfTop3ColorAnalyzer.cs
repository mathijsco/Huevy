using System.Drawing;
using Huevy.Lib.Core;
using System.Linq;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class AverageOfTop3ColorAnalyzer : IColorAnalyzer
    {
        public HueColor FindColor(ColorBucket bucket)
        {
            // Reverse the colors, to make the last one with highest occurence the most dominant.
            var mostCommonColors = bucket.Dictionary.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(3).Reverse();
            return mostCommonColors.Aggregate((c1, c2) => Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2)).ToHueColor();
        }
    }
}
