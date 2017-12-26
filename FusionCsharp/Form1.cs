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
using FusionCsharp.model.viewmodel;
using System.Threading;

namespace FusionCsharp
{
    public partial class Form1 : Form
    {
        MathHelper mh = new MathHelper();

        MainView mainview = new MainView();//主视图
        CameraView cameraview1 = new CameraView(1);//摄像机1号

        public Form1()
        {
            cameraview1.FileSource = new AForge.Video.DirectShow.FileVideoSource("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\movie.avi");
            
            //步骤一：读取主视图背景图、深度图、摄像机背景图（后期换成视频）和深度图，存入本地
            mainview.Colordata = IO.IOhelper.readBMP("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\test.bmp");
            mainview.Depthdata = IO.IOhelper.getdepthdata("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\depthdata.txt");
            cameraview1.Depthdata = IO.IOhelper.getdepthdata("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\depthdata.txt");
            

            //步骤二：读取主视图和摄像机图MVP矩阵
            double[,] modelviewMatrix_mainview =
            {
                { 0.87274477586533461,-0.31853087327850532,0.36993869622979397,102.50361835799191},
                {0.10809792746051593, 0.86508111464077375,0.48984640773472798,44.104662708586204},
                {-0.47605818371130654,-0.38752128697699839,0.78942755073609061, -986.81032604767370},
                {0.00000000000000000, 0.00000000000000000,0.00000000000000000,1.0000000000000000}
            };
            double[,] modelviewMatrix_camera1 =
            {
                { 0.74775775165414804,-0.37934716130469248, 0.54493492827236278,95.762009173457713 },
                {0.092463160613615841,0.87221644200955561,0.48030099127269366,-8.2428991067991237},
                {-0.65750202187557893,-0.30876238355451124,0.68728224313688080,-776.94778055543122},
                {0.00000000000000000,0.00000000000000000,0.00000000000000000,1.0000000000000000}
            };
           double[,] projectMatrix_mainview =
            {
                { 3.0769231897839462,0.00000000000000000, 0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,3.8461539872299335,0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,0.00000000000000000,-2.6190933518446222,-2180.4271028338017},
                {0.00000000000000000,0.00000000000000000,-1.0000000000000000,0.00000000000000000}
            };
            double[,] projectMatrix_camera1 =
            {
                { 3.0769231897839462, 0.00000000000000000, 0.00000000000000000, 0.00000000000000000 },
                {0.00000000000000000,3.8461539872299335,0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,0.00000000000000000,-1.9028681049656828,-1082.8718693111402 },
                {0.00000000000000000,0.00000000000000000,-1.0000000000000000,0.00000000000000000}
            };
            double[] viewport_mainview = { 0.0, 0.0, 1000.0, 800.0 };
            double[] viewport_camera1depth = { 0.0, 0.0, 1000.0, 800.0 };
            double[] viewport_camera1color = { 0.0, 0.0, 1920.0, 1080.0 };
            mainview.ModelviewMatrix = mh.getMatrix(modelviewMatrix_mainview);
            mainview.ProjectMatrix = mh.getMatrix(projectMatrix_mainview);
            mainview.Viewport = mh.getVector(viewport_mainview);

            cameraview1.ModelviewMatrix = mh.getMatrix(modelviewMatrix_camera1);
            cameraview1.ProjectMatrix = mh.getMatrix(projectMatrix_camera1);
            cameraview1.Viewport_depthdata = mh.getVector(viewport_camera1depth);
            cameraview1.Viewport_colordata = mh.getVector(viewport_camera1color);


            //步骤三：计算主视图与摄像头对应像素关系
            cameraview1.List_relationshipMainviewCamera = mh.getRelationship(1000, 800, mainview.Depthdata, cameraview1.Depthdata, mainview.ModelviewMatrix, mainview.ProjectMatrix, mainview.Viewport, cameraview1.ModelviewMatrix, cameraview1.ProjectMatrix, cameraview1.Viewport_depthdata, cameraview1.Viewport_colordata);

            InitializeComponent();
        }




        #region 方法四：对方法三的改进，建立主视图像素与摄像头像素的关系
        public void execute()
        {

           execute1();
        }

        void execute1()
        {
            OpenVideoSource1(cameraview1.FileSource);
        }


        private void OpenVideoSource1(IVideoSource source)
        {

            // start new video source
            videoSourcePlayer1.VideoSource = source;
            videoSourcePlayer1.Start();
            videoSourcePlayer1.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(captureframe1);


        }


        private void captureframe1(object sender, ref Bitmap image)
        {
            //放入消息队列
            if (image != null)
            {
                cameraview1.Colordata = image;//获得视频帧数据

                DateTime start = DateTime.Now;
                //步骤三：视频与3D模型融合
                mh.funsion4(cameraview1.List_relationshipMainviewCamera, mainview.Colordata, cameraview1.Colordata);
                //步骤四：渲染绘制融合后的结果
                draw(mainview.Colordata);
                DateTime end = DateTime.Now;
                TimeSpan ts = end - start;
                string a = ts.TotalMilliseconds.ToString();
            }
        }
 
 
        void draw(Bitmap colordata_mainview_bmp)
        {
            if (colordata_mainview_bmp != null)
            {
                Graphics g = panel_draw.CreateGraphics();
                g.DrawImage(colordata_mainview_bmp, 0, 0, colordata_mainview_bmp.Width, colordata_mainview_bmp.Height);

            }
        }
        #endregion

        /// <summary>
        /// 融合按钮时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_fusion_Click(object sender, EventArgs e)
        {
            execute();
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
                //    // 创建视频数据源
                //    fileSource = new FileVideoSource(openFileDialog_colordata_camera_bmp.FileName);
            }
        }
    }
}
