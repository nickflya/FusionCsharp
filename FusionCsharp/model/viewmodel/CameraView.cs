using AForge.Video.DirectShow;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FusionCsharp.model.viewmodel
{
    /// <summary>
    /// 摄像机模型
    /// </summary>
    public class CameraView : View
    {
        private int id = -1;//摄像机ID号
        private Bitmap colordata = null;//每一帧的图像
        private FileVideoSource fileSource = null;//视频数据
        private List<relationshipMainviewCamera> list_relationshipMainviewCamera = new List<relationshipMainviewCamera>();//存储主视图与摄像头像素对应关系
        private bool isActive = true;//是否激活 
        private Vector<double> viewport_colordata = null;//视频帧的viewport
        private Vector<double> viewport_depthdata = null;//深度图的viewport
        public int Id { get => id; set => id = value; }
        public FileVideoSource FileSource { get => fileSource; set => fileSource = value; }
        public List<relationshipMainviewCamera> List_relationshipMainviewCamera { get => list_relationshipMainviewCamera; set => list_relationshipMainviewCamera = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public Vector<double> Viewport_colordata { get => viewport_colordata; set => viewport_colordata = value; }
        public Vector<double> Viewport_depthdata { get => viewport_depthdata; set => viewport_depthdata = value; }
        public Bitmap Colordata { get => colordata; set => colordata = value; }

        public CameraView(int id)
        {
            Id = id;
        }
    }
}
