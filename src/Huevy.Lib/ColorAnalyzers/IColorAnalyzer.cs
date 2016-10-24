using Huevy.Lib.Core;
using System.Drawing;

namespace Huevy.Lib.ColorAnalyzers
{
    public interface IColorAnalyzer
    {
        HueColor FindColor(ColorBucket bucket);
    }
}
