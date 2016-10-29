using System;

namespace Huevy.Lib.Core
{
    public static class ColorExtensions
    {
        public static byte HueBrightness(this TinyColor color)
        {
            return Math.Min((byte)254, Math.Max((byte)1, (byte)(color.Brightness * 255)));
        }

        public static ushort HueHue(this TinyColor color)
        {
            return (ushort)Math.Round(color.Hue * 182.0416666666667f);
        }

        public static byte HueSaturation(this TinyColor color)
        {
            return (byte)(color.Saturation * 255);
        }

        public static HueColor ToHueColor(this TinyColor color)
        {
            return new HueColor
            {
                Hue = color.HueHue(),
                Saturation = color.HueSaturation(),
                Brightness = color.HueBrightness(),
                OriginalColor = color
            };
        }
    }
}
