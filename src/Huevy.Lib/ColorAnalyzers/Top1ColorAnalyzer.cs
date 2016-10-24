using System.Drawing;
using System.Linq;
using Huevy.Lib.Core;

namespace Huevy.Lib.ColorAnalyzers
{
    public sealed class Top1ColorAnalyzer : IColorAnalyzer
    {
        //private Color _currentColor = Color.Transparent;

        public HueColor FindColor(ColorBucket bucket)
        {
            // TOP 1 COLOR, AND KEEP UNTIL CHANGED ENOUGH.
            var targetColors = bucket.Dictionary.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(3).ToList();
            // Ignore if previous color is still in the TOP 3
            //if (targetColors.Any(c => c.Equals(_currentColor)))
            //    return;

            //_currentColor = targetColor;
            return targetColors.First().ToHueColor();
        }
    }
}
