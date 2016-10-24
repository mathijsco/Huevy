using System.Drawing;
using System.Linq;
using Huevy.Lib.Core;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class HighestIntensityColorAnalyzer : IColorAnalyzer
    {
        public HueColor FindColor(ColorBucket bucket)
        {
            return bucket.Dictionary.OrderByDescending(pair => pair.Value * pair.Key.ColorIntensity()).Select(pair => pair.Key).First().ToHueColor();
        }
    }
}
