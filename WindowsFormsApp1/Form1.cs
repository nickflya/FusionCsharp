using Geb.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static private VideoFileReader _reader;
        #region 读取视频文件
        public  VideoFileReader getVideoFrame(string filename)
        {
            try
            {
                _reader = new VideoFileReader();
                _reader.Open(filename);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                _reader = null;
            }

            return _reader;

        }
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VideoFileReader _reader = getVideoFrame("F:\\test.mp4");
            while (_reader != null)
            {
                Bitmap bmp = _reader.ReadVideoFrame().ToBitmap();
                if (bmp != null)
                {
                    Graphics g = panel1.CreateGraphics();
                    if (bmp != null)
                    {
                        g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                    }
                }
            }
        }
    }
}
