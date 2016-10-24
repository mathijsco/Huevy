using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huevy.Lib.Core;
using Huevy.Lib.Utilities;

namespace Huevy.Lib.ColorSource
{
    public class LiveCaptureColorSource : IColorSource
    {
        public ColorSet DetectScene()
        {
            var screenshot = Screenshot.TakeSmall();

            throw new NotImplementedException();
        }
    }
}
