using Huevy.Lib.ColorAnalyzers;
using Huevy.Lib.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public void LoadScene(ColorSet colorSet, IColorAnalyzer colorAnalyzer)
        {
            var oldImage = _pictureBox.Image;
            var bitmap = new Bitmap(150, 150);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.FullScreen), 0, 0, 150, 150);
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.Top), 40, 30, 70, 30);
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.Bottom), 40, 90, 70, 30);
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.Left), 0, 30, 40, 90);
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.Right), 110, 30, 40, 90);
                graphics.FillRectangle(FindColor(colorSet, colorAnalyzer, ColorPosition.Center), 40, 60, 70, 30);
            }
            _pictureBox.Image = bitmap;
            oldImage?.Dispose();
        }

        private SolidBrush FindColor(ColorSet set, IColorAnalyzer analyzer, ColorPosition position)
        {
            return new SolidBrush(analyzer.FindColor(set[position]).OriginalColor);
        }
    }
}
