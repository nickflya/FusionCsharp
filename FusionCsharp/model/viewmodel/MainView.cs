using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace FusionCsharp.model.viewmodel
{
    /// <summary>
    /// 主视图模型
    /// </summary>
    public class MainView : View
    {
        private Bitmap colordata = null;//主视图背景图
        private Vector<double> viewport = null;//视点矩阵

        public Bitmap Colordata { get => colordata; set => colordata = value; }
        public Vector<double> Viewport { get => viewport; set => viewport = value; }
    }
}
