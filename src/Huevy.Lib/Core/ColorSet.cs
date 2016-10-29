using Huevy.Lib.ColorAnalyzers;
using System;

namespace Huevy.Lib.Core
{
    public sealed class ColorSet
    {
        private readonly IColorAnalyzer[] _colors;

        private ColorSet(Func<IColorAnalyzer> composer)
        {
            _colors = new IColorAnalyzer[Enum.GetValues(typeof(ColorPosition)).Length];
            for (int n = 0; n < _colors.Length; n++)
                _colors[n] = composer();
        }

        public IColorAnalyzer this[ColorPosition position]
        {
            get
            {
                return _colors[(int)position];
            }
            set
            {
                _colors[(int)position] = value;
            }
        }

        public static ColorSet Create<T>() where T : IColorAnalyzer, new()
        {
            return new ColorSet(() => new T());
        }
    }
}
