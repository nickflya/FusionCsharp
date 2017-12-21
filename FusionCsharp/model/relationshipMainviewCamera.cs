using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FusionCsharp.model
{
    /// <summary>
    /// 存放主视图与摄像机上像素的对应关系
    /// </summary>
    public class relationshipMainviewCamera
    {
        private int mainview_x;
        private int mainview_y;
        private int camera_x;
        private int camera_y;

        public int Mainview_x { get => mainview_x; set => mainview_x = value; }
        public int Mainview_y { get => mainview_y; set => mainview_y = value; }
        public int Camera_x { get => camera_x; set => camera_x = value; }
        public int Camera_y { get => camera_y; set => camera_y = value; }
    }
}
