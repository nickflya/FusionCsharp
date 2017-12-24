using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FusionCsharp.model.viewmodel
{
    /// <summary>
    /// 视点基类
    /// </summary>
    abstract public class View
    {
        private List<double> depthdata = null;//深度图
        private Matrix<double> modelviewMatrix = null;//视图矩阵
        private Matrix<double> projectMatrix = null;//投影矩阵


        public List<double> Depthdata { get => depthdata; set => depthdata = value; }
        public Matrix<double> ModelviewMatrix { get => modelviewMatrix; set => modelviewMatrix = value; }
        public Matrix<double> ProjectMatrix { get => projectMatrix; set => projectMatrix = value; }

    }
}
