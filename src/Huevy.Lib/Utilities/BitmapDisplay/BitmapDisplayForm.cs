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
            this.ClientSize = new Size(150, 150);
            this.Location = new Point(0, 0);
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;

            _pictureBox = new PictureBoxWithInterpolationMode();
            _pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            _pictureBox.Dock = DockStyle.Fill;
            _pictureBox.InterpolationMode = InterpolationMode.NearestNeighbor;
            _pictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(_pictureBox);
            this.Show();
        }

        public void LoadBitmap(Image bitmap)
        {
            _pictureBox.Image = bitmap;
        }
    }
}
