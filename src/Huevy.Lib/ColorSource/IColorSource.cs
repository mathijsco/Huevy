using Huevy.Lib.ColorAnalyzers;
using Huevy.Lib.Core;

namespace Huevy.Lib.ColorSource
{
    public interface IColorSource
    {
        ColorSet DetectScene<T>() where T : IColorAnalyzer, new();
    }
}
