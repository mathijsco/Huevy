using Huevy.Lib.ColorAnalyzers;
using Huevy.Lib.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Huevy.Lib.Utilities.BitmapDisplay
{
    public sealed class BitmapDisplayForm : Form
    {
        private static BitmapDisplayForm _instance;
        public static BitmapDisplayForm Instance
        {
            get
            {
                if (_instance == null)
                {
                    var thread = new Thread(() =>
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        _instance = new BitmapDisplayForm();
                        Application.Run(_instance);

                        _instance.Dispose();
                        _instance = null;
                    });
                    thread.Start();
                }
                while (_instance == null) ;
                return _instance;
            }
        }

        private readonly PictureBoxWithInterpolationMode _pictureBox;

        private BitmapDisplayForm()
        {
            this.Text = "Current screenshot";
            this.StartPosition = FormStartPosition.Manual;
            this.MinimumSize = new Size(0, 0);
            this.Location = new Point(0, 0);
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;

            _pictureBox = new PictureBoxWithInterpolationMode();
            _pictureBox.SizeMode = PictureBoxSizeMode.Normal;
            _pictureBox.Dock = DockStyle.Fill;
            _pictureBox.InterpolationMode = InterpolationMode.NearestNeighbor;
            _pictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(_pictureBox);
            this.Show();

            this.ClientSize = new Size(150, 150);
        }

        public void LoadBitmap(Image bitmap)
        {
            var oldImage = _pictureBox.Image;
            _pictureBox.Image = bitmap;
            oldImage?.Dispose();
        }

        public void LoadScene(ColorSet colorSet)
        {
            var oldImage = _pictureBox.Image;
            var bitmap = new Bitmap(150, 150);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.FullScreen]), 0, 0, 150, 150);
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.Top]), 40, 30, 70, 30);
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.Bottom]), 40, 90, 70, 30);
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.Left]), 0, 30, 40, 90);
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.Right]), 110, 30, 40, 90);
                graphics.FillRectangle(FindColor(colorSet[ColorPosition.Center]), 40, 60, 70, 30);
            }
            _pictureBox.Image = bitmap;
            oldImage?.Dispose();
        }

        private SolidBrush FindColor(IColorAnalyzer colorAnalyzer)
        {
            var color = colorAnalyzer.FindColor();
            var systemColor = FromAhsb(255, color.Hue, color.Saturation, color.Brightness);
            return new SolidBrush(systemColor);
        }

        private static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            if (0 > alpha || 255 < alpha)
                throw new ArgumentOutOfRangeException("alpha", alpha, "Value must be within a range of 0 - 255.");
            if (0f > hue || 360f < hue)
                throw new ArgumentOutOfRangeException("hue", hue, "Value must be within a range of 0 - 360.");
            if (0f > saturation || 1f < saturation)
                throw new ArgumentOutOfRangeException("saturation", saturation, "Value must be within a range of 0 - 1.");
            if (0f > brightness || 1f < brightness)
                throw new ArgumentOutOfRangeException("brightness", brightness, "Value must be within a range of 0 - 1.");
            
            if (0 == saturation)
                return Color.FromArgb(alpha, Convert.ToInt32(brightness * 255), Convert.ToInt32(brightness * 255), Convert.ToInt32(brightness * 255));

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }
    }
}
