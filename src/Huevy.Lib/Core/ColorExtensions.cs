using System;
using System.Drawing;

namespace Huevy.Lib.Core
{
    public static class ColorExtensions
    {
        public static byte HueBrightness(this Color color)
        {
            return Math.Min((byte)254, Math.Max((byte)1, (byte)(color.GetBrightness() * 255)));
        }

        public static ushort HueHue(this Color color)
        {
            return (ushort)Math.Round(color.GetHue() * 182.0416666666667f);
        }

        public static byte HueSaturation(this Color color)
        {
            return (byte)(color.GetSaturation() * 255);
        }

        public static float ColorIntensity(this Color color)
        {
            var saturation = color.GetSaturation(); // Sweetspot = 1
            var brightness = color.GetBrightness(); // Sweetspot = 0.5
            if (brightness > 0.5)
                brightness = 1 - brightness;

            // Linear
            return saturation / 2 + brightness;
            // Arc
            //return (float)Math.Pow(saturation / 2 + brightness, 2);
        }

        public static HueColor ToHueColor(this Color color)
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
