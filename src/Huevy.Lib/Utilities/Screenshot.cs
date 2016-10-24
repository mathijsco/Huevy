using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Huevy.Lib.Utilities
{
    public sealed class Screenshot
    {
        private const int MaxWidthInPixels = 150;

        public static Bitmap TakeSmall()
        {
            var screenshot = Take();
            var targetSize = new Size(MaxWidthInPixels, MaxWidthInPixels);
            if (screenshot.Width >= screenshot.Height)
                targetSize.Height = (int)((double)MaxWidthInPixels / screenshot.Width * screenshot.Height);
            else
                targetSize.Width = (int)((double)MaxWidthInPixels / screenshot.Height * screenshot.Width);

            var bitmap = new Bitmap(targetSize.Width, targetSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawImage(screenshot, 0, 0, bitmap.Width, bitmap.Height);
            }
            screenshot.Dispose();
            return bitmap;
        }

        public static Bitmap Take()
        {
            Size sz = Screen.PrimaryScreen.Bounds.Size;
            IntPtr hDesk = GetDesktopWindow();
            IntPtr hSrce = GetWindowDC(hDesk);
            IntPtr hDest = CreateCompatibleDC(hSrce);
            IntPtr hBmp = CreateCompatibleBitmap(hSrce, sz.Width, sz.Height);
            IntPtr hOldBmp = SelectObject(hDest, hBmp);
            bool b = BitBlt(hDest, 0, 0, sz.Width, sz.Height, hSrce, 0, 0, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
            Bitmap bmp = Bitmap.FromHbitmap(hBmp);
            SelectObject(hDest, hOldBmp);
            DeleteObject(hBmp);
            DeleteDC(hDest);
            ReleaseDC(hDesk, hSrce);

            //bmp.Save(@"c:\temp\test.png");
            //bmp.Dispose();
            return bmp;
        }


        // P/Invoke declarations
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr ptr);
    }
}
