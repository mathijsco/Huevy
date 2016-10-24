using System;
using System.Collections.Generic;
using System.Linq;
using Huevy.Lib.Core;
using Huevy.Lib.Utilities;
using System.Drawing;
using System.Drawing.Imaging;

namespace Huevy.Lib.ColorSource
{
    public class LiveCaptureColorSource : IColorSource
    {
        // Regions
        // new float[] { 0, 0, 1, 1 }               // Full screen
        // new float[] { 0, 0.3f, 0.3f, 0.7f }      // Left
        // new float[] { 0.7f, 0.3f, 1, 0.7f }      // Right
        // new float[] { 0.3f, 0.3f, 0.7f, 0.7f }   // Center
        // new float[] { 0.3f, 0.135f, 0.7f, 0.3f } // Top
        // new float[] { 0.3f, 0.7f, 0.7f, 0.865f } // Bottom


        public ColorSet DetectScene()
        {
            // TODO: Process the bitmap for all positions at once.

            var colorSet = new ColorSet();
            colorSet[ColorPosition.FullScreen] = DetectColorForRegion(new float[] { 0, 0, 1, 1 });
            colorSet[ColorPosition.Left] = DetectColorForRegion(new float[] { 0, 0.3f, 0.3f, 0.7f });
            colorSet[ColorPosition.Right] = DetectColorForRegion(new float[] { 0.7f, 0.3f, 1, 0.7f });
            colorSet[ColorPosition.Center] = DetectColorForRegion(new float[] { 0.3f, 0.3f, 0.7f, 0.7f });
            colorSet[ColorPosition.Top] = DetectColorForRegion(new float[] { 0.3f, 0.135f, 0.7f, 0.3f });
            colorSet[ColorPosition.Bottom] = DetectColorForRegion(new float[] { 0.3f, 0.7f, 0.7f, 0.865f });
            return colorSet;
        }

        private HueColor DetectColorForRegion(float[] region)
        {
            // Add the color findings to the list with an counter
            var dic = new Dictionary<Color, int>();

            using (var processedBitmap = Screenshot.TakeSmall())
            {
                unsafe
                {
                    BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                    int bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                    int heightInPixels = bitmapData.Height;
                    int widthInPixels = bitmapData.Width;
                    byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                    var xStart = (int)(widthInPixels * region[0]) * bytesPerPixel;
                    var yStart = (int)(heightInPixels * region[1]);
                    var xStop = (int)(widthInPixels * region[2]) * bytesPerPixel;
                    var yStop = (int)(heightInPixels * region[3]);

                    for (int y = yStart; y < yStop; y++)
                    {
                        byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                        for (int x = xStart; x < xStop; x = x + bytesPerPixel)
                        {
                            byte blue = FindClosest32Byte(currentLine[x]);
                            byte green = FindClosest32Byte(currentLine[x + 1]);
                            byte red = FindClosest32Byte(currentLine[x + 2]);

                            //var color = (red << 16) | (green << 8) | blue;
                            var color = Color.FromArgb(red, green, blue);
                            int amount;
                            if (dic.TryGetValue(color, out amount))
                                dic[color] = amount + 1;
                            else
                                dic.Add(color, 1);
                        }
                    }
                    processedBitmap.UnlockBits(bitmapData);
                }
            }

            // ------------------------------------
            // AVERAGE COLOR OF TOP 3
            // Reverse the colors, to make the last one with highest occurence the most dominant.
            //var mostCommonColors = dic.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(3).Reverse();
            //var targetColor = mostCommonColors.Aggregate((c1, c2) => Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2));
            // ------------------------------------


            // ------------------------------------
            // COLOR WITH HIGHEST INTENSITY
            //var targetColor = dic.OrderByDescending(pair => pair.Value * pair.Key.ColorIntensity()).Select(pair => pair.Key).First();
            // ------------------------------------


            // ------------------------------------
            // TOP 1 COLOR, AND KEEP UNTIL CHANGED ENOUGH.
            var targetColors = dic.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(3).ToList();
            // Ignore if previous color is still in the TOP 3
            //if (targetColors.Any(c => c.Equals(_currentColor)))
            //    return;
            var targetColor = targetColors.First();
            // ------------------------------------

            //_currentColor = targetColor;
            return new HueColor
            {
                Hue = targetColor.HueHue(),
                Saturation = targetColor.HueSaturation(),
                Brightness = targetColor.HueBrightness(),
                OriginalColor = targetColor
            };
        }



        // Round on 40
        private static byte FindClosest40Byte(byte input)
        {
            const double rounding = 40d;
            return (byte)(Math.Round(input / rounding) * rounding);
        }


        // Round on 32
        private static byte FindClosest32Byte(byte input)
        {
            // 1110 0000

            // < 0010 0000
            if (input < 0x20) return 0x00;
            // > 1110 0000
            if (input >= 0xE0) return 0xFF;

            // ###* ****
            return (byte)(input & 0xE0);
        }

        // Round on 64
        private static byte FindClosest64Byte(byte input)
        {
            // 1110 0000
            // Trim bytes before 32: & 0xE0

            // < 0100 0000
            if (input < 0x40) return 0x00;
            // > 1100 0000
            if (input >= 0xC0) return 0xFF;

            // ##** ****
            return (byte)(input & 0xC0);
        }
    }
}
