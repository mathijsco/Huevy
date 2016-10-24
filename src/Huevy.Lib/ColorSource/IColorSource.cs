using Huevy.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huevy.Lib.ColorSource
{
    public interface IColorSource
    {
        ColorSet DetectScene();
    }
}
