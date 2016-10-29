using System.Runtime.InteropServices;

namespace Huevy.Lib.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TinyColor
    {
        public float Hue { get; private set; }

        public float Saturation { get; private set; }

        public float Brightness { get; private set; }


        private TinyColor(float hue, float saturation, float brightness)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Brightness = brightness;
        }

        public static TinyColor FromRgb(byte red, byte green, byte blue)
        {
            float r = (float)red / 255;
            float g = (float)green / 255;
            float b = (float)blue / 255;

            return new TinyColor(
                CalculateHue(r, g, b),
                CalculateSaturation(r, g, b),
                CalculateBrightness(r, g, b)
            );
        }

        public static TinyColor FromRgb(int red, int green, int blue)
        {
            return FromRgb((byte)red, (byte)green, (byte)blue);
        }

        public static TinyColor FromHsb(float hue, float saturation, float brightness)
        {
            return new TinyColor(hue, saturation, brightness);
        }


        private static float CalculateHue(float r, float g, float b)
        {
            if (r == g && g == b)
                return 0f;

            float num1 = 0f;
            float num2 = r;
            float num3 = r;
            if (g > num2) num2 = g;
            if (b > num2) num2 = b;
            if (g < num3) num3 = g;
            if (b < num3) num3 = b;
            float num4 = num2 - num3;
            if (r == num2) num1 = (g - b) / num4;
            else if (g == num2) num1 = 2f + (b - r) / num4;
            else if (b == num2) num1 = 4f + (r - g) / num4;
            num1 *= 60f;
            if (num1 < 0f) num1 += 360f;
            return num1;
        }

        private static float CalculateSaturation(float r, float g, float b)
        {
            float result = 0f;
            float num1 = r;
            float num2 = r;
            if (g > num1) num1 = g;
            if (b > num1) num1 = b;
            if (g < num2) num2 = g;
            if (b < num2) num2 = b;
            if (num1 != num2)
            {
                float num3 = (num1 + num2) / 2f;
                if ((double)num3 <= 0.5d)
                    result = (num1 - num2) / (num1 + num2);
                else
                    result = (num1 - num2) / (2f - num1 - num2);
            }
            return result;
        }

        private static float CalculateBrightness(float r, float g, float b)
        {
            float num1 = r;
            float num2 = r;
            if (g > num1) num1 = g;
            if (b > num1) num1 = b;
            if (g < num2) num2 = g;
            if (b < num2) num2 = b;
            return (num1 + num2) / 2f;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TinyColor))
                return false;
            return this == (TinyColor)obj;
        }

        public override int GetHashCode()
        {
            return this.Hue.GetHashCode() ^ this.Brightness.GetHashCode() ^ this.Saturation.GetHashCode();
        }

        public static bool operator ==(TinyColor color1, TinyColor color2)
        {
            return color1.Hue == color2.Hue
                && color1.Saturation == color2.Saturation
                && color1.Brightness == color2.Brightness;
        }

        public static bool operator !=(TinyColor color1, TinyColor color2)
        {
            return !(color1 == color2);
        }
    }
}
