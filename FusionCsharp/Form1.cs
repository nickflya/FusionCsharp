using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FusionCsharp.model;
using MathNet.Numerics.LinearAlgebra;
using FusionCsharp.Math;
using AForge.Video.DirectShow;
using AForge.Video;

namespace FusionCsharp
{
    public partial class Form1 : Form
    {
        List<colorRGB> colordata_mainview_txt = new List<colorRGB>();//主视图背景图
        Bitmap colordata_mainview_bmp;
        List<double> depthdata_mainview = new List<double>();//主视图深度图
        List<colorRGB> colordata_camera_txt = new List<colorRGB>();//摄像机背景图
        Bitmap colordata_camera_bmp;
        List<double> depthdata_camera = new List<double>();//摄像机深度图

        Bitmap curBitmap;
        FileVideoSource fileSource;//视频数据


        public Form1()
        {
            InitializeComponent();
        }

        #region 方法一：利用txt数据

        /// <summary>
        /// 读取txt数据进行融合
        /// </summary>
        void execute1()
        {

            //步骤一：读取主视图背景图、深度图、摄像机背景图（后期换成视频）和深度图，存入本地
            colordata_mainview_txt = IO.IOhelper.getcolordata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\colordata_unsign.txt");
            depthdata_mainview = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\depthdata.txt");
            colordata_camera_txt = IO.IOhelper.getcolordata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\color_unsign.txt");
            depthdata_camera = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\depthdata.txt");
            //步骤二：读取主视图和摄像机图MVP矩阵
            MathHelper mh = new MathHelper();
            Matrix<double> modelviewMatrix_mainview = mh.getmodelviewMatrix_mainview();
            Matrix<double> projectMatrix_mainview = mh.getprojectMatrix_mainview();
            Vector<double> viewport_mainview = mh.getviewport_mainview();

            Matrix<double> modelviewMatrix_camera = mh.getmodelviewMatrix_camera();
            Matrix<double> projectMatrix_camera = mh.getprojectMatrix_camera();
            Vector<double> viewport_cameradepth = mh.getviewport_cameradepth();
            Vector<double> viewport_cameracolor = mh.getviewport_cameracolor();
            //步骤三：视频与3D模型融合
            mh.fusion1(1000, 800, depthdata_mainview, colordata_mainview_txt, depthdata_camera, colordata_camera_txt, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, viewport_cameracolor);
            //步骤四：渲染绘制融合后的结果
            draw1(colordata_mainview_txt);
        }

        void draw1(List<colorRGB> colordata_mainview)
        {
            curBitmap = new Bitmap(1000, 800);
            if (curBitmap != null)
            {
                Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
                System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);
                int location = 0;
                unsafe
                {
                    byte* ptr = (byte*)(bmpData.Scan0);
                    for (int i = bmpData.Height - 1; i >= 0; i--)//Opengl坐标系起点在左下角，Scan0是从左上角开始。
                    {
                        for (int j = 0; j < bmpData.Width; j++)
                        {
                            location = i * bmpData.Width + j;
                            ptr[0] = (byte)colordata_mainview[location].R;
                            byte a = ptr[0];
                            ptr[1] = (byte)colordata_mainview[location].G;
                            ptr[2] = (byte)colordata_mainview[location].B;
                            ptr += 3;
                        }
                        ptr += bmpData.Stride - bmpData.Width * 3;
                    }

                    curBitmap.UnlockBits(bmpData);
                    Graphics g = panel_draw.CreateGraphics();
                    if (curBitmap != null)
                    {
                        g.DrawImage(curBitmap, 0, 0, curBitmap.Width, curBitmap.Height);
                    }
                }
            }
        }


        #endregion

        #region 方法二：利用bmp图片数据

        void execute2()
        {
            //步骤一：读取主视图背景图、深度图、摄像机背景图（后期换成视频）和深度图，存入本地
            colordata_mainview_bmp = IO.IOhelper.readBMP("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\test.bmp");
            depthdata_mainview = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\depthdata.txt");
            colordata_camera_bmp = IO.IOhelper.readBMP("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\1.jpg");
            depthdata_camera = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\depthdata.txt");
            //步骤二：读取主视图和摄像机图MVP矩阵
            MathHelper mh = new MathHelper();
            Matrix<double> modelviewMatrix_mainview = mh.getmodelviewMatrix_mainview();
            Matrix<double> projectMatrix_mainview = mh.getprojectMatrix_mainview();
            Vector<double> viewport_mainview = mh.getviewport_mainview();

            Matrix<double> modelviewMatrix_camera = mh.getmodelviewMatrix_camera();
            Matrix<double> projectMatrix_camera = mh.getprojectMatrix_camera();
            Vector<double> viewport_cameradepth = mh.getviewport_cameradepth();
            Vector<double> viewport_cameracolor = mh.getviewport_cameracolor();
            //步骤三：视频与3D模型融合
            mh.fusion2(1000, 800, depthdata_mainview, colordata_mainview_bmp, depthdata_camera, colordata_camera_bmp, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, viewport_cameracolor);
            //步骤四：渲染绘制融合后的结果
            draw2(colordata_mainview_bmp);
        }

        void draw2(Bitmap colordata_mainview_bmp)
        {
            if (colordata_mainview_bmp != null)
            {
                Graphics g = panel_draw.CreateGraphics();
                if (colordata_mainview_bmp != null)
                {
                    g.DrawImage(colordata_mainview_bmp, 0, 0, colordata_mainview_bmp.Width, colordata_mainview_bmp.Height);
                }
            }
        }

        #endregion

        void execute3()
        {
            //步骤一：读取主视图背景图、深度图、摄像机背景图（后期换成视频）和深度图，存入本地
            colordata_mainview_bmp = IO.IOhelper.readBMP("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\test.bmp");
            depthdata_mainview = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\depthdata.txt");
            depthdata_camera = IO.IOhelper.getdepthdata("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\depthdata.txt");
            OpenVideoSource(fileSource);

        }

        /// <summary>
        /// 获取视频每一帧数据的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void captureframe(object sender, ref Bitmap image)
        {
            if (image != null)
            {
                colordata_camera_bmp = image;//获得视频帧数据

                //步骤二：读取主视图和摄像机图MVP矩阵
                MathHelper mh = new MathHelper();
                Matrix<double> modelviewMatrix_mainview = mh.getmodelviewMatrix_mainview();
                Matrix<double> projectMatrix_mainview = mh.getprojectMatrix_mainview();
                Vector<double> viewport_mainview = mh.getviewport_mainview();

                Matrix<double> modelviewMatrix_camera = mh.getmodelviewMatrix_camera();
                Matrix<double> projectMatrix_camera = mh.getprojectMatrix_camera();
                Vector<double> viewport_cameradepth = mh.getviewport_cameradepth();
                Vector<double> viewport_cameracolor = mh.getviewport_cameracolor();
                viewport_cameracolor[2] = colordata_camera_bmp.Width;
                viewport_cameracolor[3] = colordata_camera_bmp.Height;
                //步骤三：视频与3D模型融合
                mh.fusion2(1000, 800, depthdata_mainview, colordata_mainview_bmp, depthdata_camera, colordata_camera_bmp, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, viewport_cameracolor);
                //步骤四：渲染绘制融合后的结果
                draw2(colordata_mainview_bmp);
            }
        }

        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();
            videoSourcePlayer.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(captureframe);

            this.Cursor = Cursors.Default;
        }

        //融合按钮时间
        private void button_fusion_Click(object sender, EventArgs e)
        {
            //execute1();
            //execute2();
            execute3();
        }

        /// <summary>
        /// 绘制函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_draw_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_colordata_camera_bmp_Click(object sender, EventArgs e)
        {
            if (openFileDialog_colordata_camera_bmp.ShowDialog() == DialogResult.OK)
            {
                // 创建视频数据源
                fileSource = new FileVideoSource(openFileDialog_colordata_camera_bmp.FileName);
            }
        }
    }
}
