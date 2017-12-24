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

namespace FusionCsharp
{
    public partial class Form1 : Form
    {
        MathHelper mh = new MathHelper();
        MainView mainview = new MainView();
        CameraView cameraview1 = new CameraView();

        public Form1()
        {
            cameraview1.FileSource = new AForge.Video.DirectShow.FileVideoSource("F:\\OneDrive - AD\\台式机-study\\OpenGL\\坐标转换程序\\plan1WEB\\movie.avi");
            InitializeComponent();
        }




        #region 方法四：对方法三的改进，建立主视图像素与摄像头像素的关系
        public void execute()
        {
            //步骤一：读取主视图背景图、深度图、摄像机背景图（后期换成视频）和深度图，存入本地
            mainview.Colordata = IO.IOhelper.readBMP("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\test.bmp");
            mainview.Depthdata = IO.IOhelper.getdepthdata("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\主视图\\depthdata.txt");
            cameraview1.Depthdata = IO.IOhelper.getdepthdata("C:\\Users\\huahai\\Documents\\GitHub\\FusionCsharp\\FusionCsharp\\bin\\Debug\\newdata2\\摄像机\\depthdata.txt");


            //步骤二：读取主视图和摄像机图MVP矩阵
            mainview.ModelviewMatrix = mh.getmodelviewMatrix_mainview();
            mainview.ProjectMatrix = mh.getprojectMatrix_mainview();
            mainview.Viewport = mh.getviewport_mainview();

            cameraview1.ModelviewMatrix = mh.getmodelviewMatrix_camera();
            cameraview1.ProjectMatrix = mh.getprojectMatrix_camera();
            cameraview1.Viewport_depthdata = mh.getviewport_cameradepth();
            cameraview1.Viewport_colordata = mh.getviewport_cameracolor();

            //步骤三：计算主视图与摄像头对应像素关系
            cameraview1.List_relationshipMainviewCamera = mh.getRelationship(1000, 800, mainview.Depthdata, cameraview1.Depthdata, mainview.ModelviewMatrix, mainview.ProjectMatrix, mainview.Viewport, cameraview1.ModelviewMatrix, cameraview1.ProjectMatrix, cameraview1.Viewport_depthdata, cameraview1.Viewport_colordata);

            //TODO:分线程
            OpenVideoSource(cameraview1.FileSource);
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

        private void captureframe(object sender, ref Bitmap image)
        {
            //TODO:放入消息队列
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
                if (colordata_mainview_bmp != null)
                {
                    g.DrawImage(colordata_mainview_bmp, 0, 0, colordata_mainview_bmp.Width, colordata_mainview_bmp.Height);
                }
            }
        }
        #endregion

        //融合按钮时间
        private void button_fusion_Click(object sender, EventArgs e)
        {
            //logic lgc = new logic();
            //lgc.execute();
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
