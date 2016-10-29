using Huevy.Lib.Core;

namespace Huevy.Lib.ColorAnalyzers
{
    public interface IColorAnalyzer
    {
        void Add(TinyColor color);

        TinyColor FindColor();
    }
}
