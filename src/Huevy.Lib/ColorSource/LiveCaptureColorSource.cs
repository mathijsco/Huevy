using Huevy.Lib.Core;
using Huevy.Lib.Utilities;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;

namespace Huevy.Lib.ColorSource
{
    public class LiveCaptureColorSource : IColorSource
    {
        private readonly ListDictionary _regions = new ListDictionary
        {
            { (int)ColorPosition.FullScreen, new float[] { 0, 0, 1, 1 }               },
            { (int)ColorPosition.Top,        new float[] { 0.3f, 0.135f, 0.7f, 0.3f } },
            { (int)ColorPosition.Right,      new float[] { 0.7f, 0.3f, 1, 0.7f }      },
            { (int)ColorPosition.Bottom,     new float[] { 0.3f, 0.7f, 0.7f, 0.865f } },
            { (int)ColorPosition.Left,       new float[] { 0, 0.3f, 0.3f, 0.7f }      },
            { (int)ColorPosition.Center,     new float[] { 0.3f, 0.3f, 0.7f, 0.7f }   }
        };
        
        public ColorSet DetectScene()
        {
            var colorSet = new ColorSet();

            // Add the color findings to the list with an counter
            using (var processedBitmap = Screenshot.TakeSmall())
            {
                var width = processedBitmap.Width;
                var height = processedBitmap.Height;
                var enumLength = Enum.GetValues(typeof(ColorPosition)).Length;
                var ranges = new int[enumLength, 4];
                for (int p = 0; p < enumLength; p++)
                {
                    var region = (float[])_regions[p];
                    ranges[p, 0] = (int)(width * region[0]); // X start position
                    ranges[p, 1] = (int)(height * region[1]); // Y start position
                    ranges[p, 2] = (int)(width * region[2]); // X end position
                    ranges[p, 3] = (int)(height * region[3]); // Y end position
                }

                // Proces the bitmap
                unsafe
                {
                    BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                    int bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                    int heightInPixels = bitmapData.Height;
                    int widthInPixels = bitmapData.Width;
                    byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < heightInPixels; y++)
                    {
                        byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                        for (int x = 0; x < widthInPixels; x++)
                        {
                            // Process all color positions
                            for (int p = 0; p < enumLength; p++)
                            {
                                // X is out if range
                                if (x < ranges[p, 0] || x > ranges[p, 2])
                                    continue;
                                // Y is out if range
                                if (y < ranges[p, 1] || y > ranges[p, 3])
                                    continue;

                                // *bytesPerPixel because of RGB colors
                                int xAdjusted = x * bytesPerPixel;
                                byte blue = currentLine[xAdjusted];
                                byte green = currentLine[xAdjusted + 1];
                                byte red = currentLine[xAdjusted + 2];

                                colorSet[(ColorPosition)p].Add(red, green, blue);
                            }
                        }
                    }

                    processedBitmap.UnlockBits(bitmapData);
                }
            }

            return colorSet;
        }
    }
}
