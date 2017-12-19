using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using FusionCsharp.model;
using System.Drawing;
/// <summary>
/// 所有需要的数学函数
/// </summary>
namespace FusionCsharp.Math
{
    public class MathHelper
    {
        MatrixBuilder<double> mb;
        VectorBuilder<double> vb;
        public MathHelper()
        {
            //初始化一个矩阵和向量的构建对象
            mb = Matrix<double>.Build;
            vb = Vector<double>.Build;
        }
        ~MathHelper() { }

        /// <summary>
        /// 向量与矩阵相乘算法
        /// </summary>
        /// <param name="output"></param>
        /// <param name="vec"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Vector<double> multiVectorAndMatrix(Vector<double> output, Vector<double> vec, Matrix<double> mat)
        {
            double x = vec[0], y = vec[1], z = vec[2], w = vec[3];
            output[0] = mat[0, 0] * x + mat[0, 1] * y + mat[0, 2] * z + mat[0, 3] * w;
            output[1] = mat[1, 0] * x + mat[1, 1] * y + mat[1, 2] * z + mat[1, 3] * w;
            output[2] = mat[2, 0] * x + mat[2, 1] * y + mat[2, 2] * z + mat[2, 3] * w;
            output[3] = mat[3, 0] * x + mat[3, 1] * y + mat[3, 2] * z + mat[3, 3] * w;

            return output;
        }


        /// <summary>
        /// 将世界坐标转换为屏幕坐标
        /// </summary>
        /// <param name="ObjectCoordinate"></param>
        /// <param name="modelviewMatrix"></param>
        /// <param name="projectMatrix"></param>
        /// <param name="viewport"></param>
        /// <param name="WinCoordinate"></param>
        /// <returns></returns>
        public coordinate Project(coordinate ObjectCoordinate, Matrix<double> modelviewMatrix, Matrix<double> projectMatrix, Vector<double> viewport, coordinate WinCoordinate)
        {
            double[] a = { 0.0, 0.0, 0.0, 0.0 };
            double[,] b = { { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 } };

            Vector<double> input = vb.DenseOfArray(a);
            Vector<double> output = vb.DenseOfArray(a);
            Matrix<double> modelviewproj_matrix = mb.DenseOfArray(b);

            input[0] = ObjectCoordinate.X;
            input[1] = ObjectCoordinate.Y;
            input[2] = ObjectCoordinate.Z;
            input[3] = 1.0;

            //output = input * modelviewMatrix;//模型视图变换
            //input = output * projectMatrix;//投影变换

            //input = input * (modelviewMatrix * projectMatrix);//MVP变换
            output = multiVectorAndMatrix(output, input, modelviewMatrix);
            input = multiVectorAndMatrix(input, output, projectMatrix);
            //透视除法，进入标准化设备坐标
            if (input[3] == 0.0) return null;
            input[0] /= input[3];
            input[1] /= input[3];
            input[2] /= input[3];

            //将坐标由-1到1，转换到0-1
            input[0] = input[0] * 0.5 + 0.5;
            input[1] = input[1] * 0.5 + 0.5;
            input[2] = input[2] * 0.5 + 0.5;

            //将x,y转换到屏幕坐标
            input[0] = input[0] * viewport[2] + viewport[0];
            input[1] = input[1] * viewport[3] + viewport[1];

            WinCoordinate.X = input[0];
            WinCoordinate.Y = input[1];
            WinCoordinate.Z = input[2];

            return WinCoordinate;
        }

        /// <summary>
        /// 将屏幕坐标转换为世界坐标
        /// </summary>
        /// <param name="WinCoordinate"></param>
        /// <param name="modelviewMatrix"></param>
        /// <param name="projectMatrix"></param>
        /// <param name="viewport"></param>
        /// <param name="ObjectCoordinate"></param>
        /// <returns></returns>
        public coordinate Unproject(coordinate WinCoordinate, Matrix<double> modelviewMatrix, Matrix<double> projectMatrix, Vector<double> viewport, coordinate ObjectCoordinate)
        {
            double[] a = { 0.0, 0.0, 0.0, 0.0 };
            double[,] b = { { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 }, { 0.0, 0.0, 0.0, 0.0 } };

            Matrix<double> finalMatrix = mb.DenseOfArray(b);
            Matrix<double> modelviewMatrix_invert = mb.DenseOfArray(b);
            Matrix<double> projectMatrix_invert = mb.DenseOfArray(b);
            Vector<double> input = vb.DenseOfArray(a);
            Vector<double> output = vb.DenseOfArray(a);

            //求逆矩阵
            modelviewMatrix_invert = modelviewMatrix.Inverse();
            projectMatrix_invert = projectMatrix.Inverse();
            //合并MVP矩阵
            finalMatrix = modelviewMatrix_invert * projectMatrix_invert;

            input[0] = WinCoordinate.X;
            input[1] = viewport[3] - WinCoordinate.Y;
            input[2] = WinCoordinate.Z;
            input[3] = 1.0;

            //从屏幕坐标变换为0到1之间的坐标
            input[0] = (input[0] - viewport[0]) / viewport[2];
            input[1] = (input[1] - viewport[1]) / viewport[3];

            //从0到1之间变换到-1到1之间
            input[0] = input[0] * 2 - 1;
            input[1] = input[1] * 2 - 1;
            input[2] = input[2] * 2 - 1;

            //乘以MVP逆矩阵之后得到世界空间的坐标
            //output = input * finalMatrix;
            output = multiVectorAndMatrix(output, input, finalMatrix);
            //从齐次坐标变换为笛卡尔坐标
            if (output[3] == 0.0) return null;
            output[0] /= output[3];
            output[1] /= output[3];
            output[2] /= output[3];

            //将结果赋值。
            ObjectCoordinate.X = output[0];
            ObjectCoordinate.Y = output[1];
            ObjectCoordinate.Z = output[2];

            return ObjectCoordinate;
        }

        /// <summary>
        /// 得到主视图的视图矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<double> getmodelviewMatrix_mainview()
        {
            double[,] a = { { 0.87274477586533461,-0.31853087327850532,0.36993869622979397,102.50361835799191  },
                {0.10809792746051593, 0.86508111464077375,0.48984640773472798,44.104662708586204},
                {-0.47605818371130654,-0.38752128697699839,0.78942755073609061, -986.81032604767370},
                {0.00000000000000000, 0.00000000000000000,0.00000000000000000,1.0000000000000000} };

            return mb.DenseOfArray(a);
        }

        /// <summary>
        /// 得到摄像机视图矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<double> getmodelviewMatrix_camera()
        {
            double[,] a = { { 0.74775775165414804,-0.37934716130469248, 0.54493492827236278,95.762009173457713 },
                {0.092463160613615841,0.87221644200955561,0.48030099127269366,-8.2428991067991237},
                {-0.65750202187557893,-0.30876238355451124,0.68728224313688080,-776.94778055543122},
                {0.00000000000000000,0.00000000000000000,0.00000000000000000,1.0000000000000000} };
            return mb.DenseOfArray(a);
        }


        /// <summary>
        /// 得到主视图投影矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<double> getprojectMatrix_mainview()
        {
            double[,] a = { { 3.0769231897839462,0.00000000000000000, 0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,3.8461539872299335,0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,0.00000000000000000,-2.6190933518446222,-2180.4271028338017},
                {0.00000000000000000,0.00000000000000000,-1.0000000000000000,0.00000000000000000} };

            return mb.DenseOfArray(a);
        }

        /// <summary>
        /// 得到摄像机的投影矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<double> getprojectMatrix_camera()
        {
            double[,] a = { { 3.0769231897839462, 0.00000000000000000, 0.00000000000000000, 0.00000000000000000 },
                {0.00000000000000000,3.8461539872299335,0.00000000000000000,0.00000000000000000},
                {0.00000000000000000,0.00000000000000000,-1.9028681049656828,-1082.8718693111402 },
                {0.00000000000000000,0.00000000000000000,-1.0000000000000000,0.00000000000000000} };

            return mb.DenseOfArray(a);
        }

        /// <summary>
        /// 得到主视图视点矩阵
        /// </summary>
        /// <returns></returns>
        public Vector<double> getviewport_mainview()
        {
            double[] a = { 0.0, 0.0, 1000.0, 800.0 };
            return vb.DenseOfArray(a);
        }

        /// <summary>
        /// 得到摄像机深度图的视口矩阵
        /// </summary>
        /// <returns></returns>
        public Vector<double> getviewport_cameradepth()
        {
            double[] a = { 0.0, 0.0, 1000.0, 800.0 };
            return vb.DenseOfArray(a);
        }

        /// <summary>
        /// 得到摄像机背景图的视口矩阵
        /// </summary>
        /// <returns></returns>
        public Vector<double> getviewport_cameracolor()
        {
            double[] a = { 0.0, 0.0, 1000.0, 800.0 };
            return vb.DenseOfArray(a);
        }

        /// <summary>
        /// 3D模型与视频融合算法
        /// </summary>
        /// <param name="WindowWidth"></param>
        /// <param name="WindowHeight"></param>
        /// <param name="depthdata_mainview"></param>
        /// <param name="colordata_mainview"></param>
        /// <param name="depthdata_camera"></param>
        /// <param name="colordata_camera"></param>
        /// <param name="modelviewMatrix_mainview"></param>
        /// <param name="projectMatrix_mainview"></param>
        /// <param name="viewport_mainview"></param>
        /// <param name="modelviewMatrix_camera"></param>
        /// <param name="projectMatrix_camera"></param>
        /// <param name="viewport_cameradepth"></param>
        /// <param name="viewport_cameracolor"></param>
        public void fusion1(int WindowWidth, int WindowHeight, List<double> depthdata_mainview, List<colorRGB> colordata_mainview, List<double> depthdata_camera, List<colorRGB> colordata_camera, Matrix<double> modelviewMatrix_mainview, Matrix<double> projectMatrix_mainview, Vector<double> viewport_mainview, Matrix<double> modelviewMatrix_camera, Matrix<double> projectMatrix_camera, Vector<double> viewport_cameradepth, Vector<double> viewport_cameracolor)
        {
            for (int i = 0; i < WindowWidth * WindowHeight; i++)
            {
                //根据一维数组中的位置，转换成主视图屏幕上的X,Y坐标。
                coordinate WinCoordinate_mainview = new coordinate(0.0, 0.0, 0.0);
                int int_iWindowWidth = ((int)(i / WindowWidth)) * WindowWidth;
                WinCoordinate_mainview.X = i - int_iWindowWidth;
                WinCoordinate_mainview.Y = (int)(i / WindowWidth);
                //从存储主视图屏幕深度信息的一维数组中取出对应的深度，即Z坐标。
                WinCoordinate_mainview.Z = depthdata_mainview[i];
                //从主视图屏幕的XYZ坐标反算到局部坐标
                coordinate ObjectCoordinate = new coordinate(0.0, 0.0, 0.0);
                ObjectCoordinate = Unproject(WinCoordinate_mainview, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, ObjectCoordinate);
                //再从局部坐标正算到摄像机的深度屏幕坐标，这样就得到了主视图屏幕上一点在摄像机的屏幕坐标和深度。
                coordinate WinCoordinate_cameradepth = new coordinate(0.0, 0.0, 0.0);
                WinCoordinate_cameradepth = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, WinCoordinate_cameradepth);
                //对深度屏幕坐标进行取整，因为像素坐标只能是整数
                WinCoordinate_cameradepth.X = (int)WinCoordinate_cameradepth.X;
                WinCoordinate_cameradepth.Y = WindowHeight - WinCoordinate_cameradepth.Y;//因为project得到的Y是OpenGL坐标
                if (WinCoordinate_cameradepth.X > 0 && WinCoordinate_cameradepth.X < WindowWidth && WinCoordinate_cameradepth.Y > 0 && WinCoordinate_cameradepth.Y < WindowHeight && WinCoordinate_cameradepth.Z > 0 && WinCoordinate_cameradepth.Z < 1)
                {
                    //将得到的摄像机深度屏幕坐标XY转换成一维数组中的位置
                    double depthdata_one_location_camera = WinCoordinate_cameradepth.Y * WindowWidth + WinCoordinate_cameradepth.X;
                    //取出摄像机深度图里面对应像素的深度
                    double depthdata_one_camera = depthdata_camera[(int)depthdata_one_location_camera];

                    //从局部坐标正算到摄像机的颜色屏幕坐标，这样就得到了摄像机视频应该映射的屏幕坐标。
                    coordinate WinCoordinate_cameracolor = new coordinate(0.0, 0.0, 0.0);
                    WinCoordinate_cameracolor = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameracolor, WinCoordinate_cameracolor);
                    //对颜色屏幕坐标进行取整，因为像素坐标只能是整数
                    WinCoordinate_cameracolor.X = (int)WinCoordinate_cameracolor.X;
                    WinCoordinate_cameracolor.Y = viewport_cameracolor[3] - (int)WinCoordinate_cameracolor.Y;//因为project得到的Y是OpenGL坐标
                                                                                                             //将得到的摄像机颜色屏幕坐标XY转换成一维数组中的位置
                    double colordata_one_location_camera = WinCoordinate_cameradepth.Y * WindowWidth + WinCoordinate_cameradepth.X;

                    if (System.Math.Abs(WinCoordinate_cameradepth.Z - depthdata_one_camera) < 0.15)
                    {
                        //取摄像机视频图相应像素的RGB值，然后替换主视点相应像素的RGB值。目前是假设摄像机RGB图和深度图是同样的屏幕宽高，如果以后不一样要另算。
                        // var t = colordata_mainview[i];
                        colordata_mainview[i] = colordata_camera[(int)colordata_one_location_camera];
                        // if (t != colordata_mainview[i])
                        //     alert("主视点原来值：" + t + "    " + "主视点修改之后的值：" + colordata_mainview[i]);
                        //t1++;
                    }
                    else if (WinCoordinate_cameradepth.Z > depthdata_one_camera)
                    {
                        colordata_mainview[i] = colordata_camera[(int)colordata_one_location_camera];
                        //t2++;
                    }
                    else
                    {
                        colordata_mainview[i] = colordata_camera[(int)colordata_one_location_camera];
                        //colordata_mainview[i] = data_unsignedInt[colordata_one_location_camera];
                        //console.log(colordata_one_location_camera);
                        //t3++;
                    }
                }
            }
        }

       public void fusion2(int WindowWidth, int WindowHeight, List<double> depthdata_mainview, Bitmap colordata_mainview_bmp, List<double> depthdata_camera, Bitmap colordata_camera_bmp, Matrix<double> modelviewMatrix_mainview, Matrix<double> projectMatrix_mainview, Vector<double> viewport_mainview, Matrix<double> modelviewMatrix_camera, Matrix<double> projectMatrix_camera, Vector<double> viewport_cameradepth, Vector<double> viewport_cameracolor)
        {
            DateTime start = DateTime.Now;
            for (int i = 0; i < WindowWidth * WindowHeight; i++)
            {

                //根据一维数组中的位置，转换成主视图屏幕上的X,Y坐标。
                coordinate WinCoordinate_mainview = new coordinate(0.0, 0.0, 0.0);
                int int_iWindowWidth = ((int)(i / WindowWidth)) * WindowWidth;
                WinCoordinate_mainview.X = i - int_iWindowWidth;
                WinCoordinate_mainview.Y = (int)(i / WindowWidth);
                //从存储主视图屏幕深度信息的一维数组中取出对应的深度，即Z坐标。
                WinCoordinate_mainview.Z = depthdata_mainview[i];
                //从主视图屏幕的XYZ坐标反算到局部坐标

                coordinate ObjectCoordinate = new coordinate(0.0, 0.0, 0.0);
                //ObjectCoordinate = Unproject(WinCoordinate_mainview, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, ObjectCoordinate);
                //再从局部坐标正算到摄像机的深度屏幕坐标，这样就得到了主视图屏幕上一点在摄像机的屏幕坐标和深度。
                coordinate WinCoordinate_cameradepth = new coordinate(0.0, 0.0, 0.0);

                WinCoordinate_cameradepth = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, WinCoordinate_cameradepth);
                //对深度屏幕坐标进行取整，因为像素坐标只能是整数
                WinCoordinate_cameradepth.X = (int)WinCoordinate_cameradepth.X;
                WinCoordinate_cameradepth.Y = WindowHeight - (int)WinCoordinate_cameradepth.Y;//因为project得到的Y是OpenGL坐标

            }
            DateTime end = DateTime.Now;
            TimeSpan ts = end - start;
            string a = ts.TotalMilliseconds.ToString();

            for (int i = 0; i < WindowWidth * WindowHeight; i++)
            {
                
                //根据一维数组中的位置，转换成主视图屏幕上的X,Y坐标。
                coordinate WinCoordinate_mainview = new coordinate(0.0, 0.0, 0.0);
                int int_iWindowWidth = ((int)(i / WindowWidth)) * WindowWidth;
                WinCoordinate_mainview.X = i - int_iWindowWidth;
                WinCoordinate_mainview.Y = (int)(i / WindowWidth);
                //从存储主视图屏幕深度信息的一维数组中取出对应的深度，即Z坐标。
                WinCoordinate_mainview.Z = depthdata_mainview[i];
                //从主视图屏幕的XYZ坐标反算到局部坐标

                coordinate ObjectCoordinate = new coordinate(0.0, 0.0, 0.0);
                ObjectCoordinate = Unproject(WinCoordinate_mainview, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, ObjectCoordinate);
                //再从局部坐标正算到摄像机的深度屏幕坐标，这样就得到了主视图屏幕上一点在摄像机的屏幕坐标和深度。
                coordinate WinCoordinate_cameradepth = new coordinate(0.0, 0.0, 0.0);


                WinCoordinate_cameradepth = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, WinCoordinate_cameradepth);
                //对深度屏幕坐标进行取整，因为像素坐标只能是整数
                WinCoordinate_cameradepth.X = (int)WinCoordinate_cameradepth.X;
                WinCoordinate_cameradepth.Y = WindowHeight - (int)WinCoordinate_cameradepth.Y;//因为project得到的Y是OpenGL坐标



                if (WinCoordinate_cameradepth.X > 0 && WinCoordinate_cameradepth.X < WindowWidth
                    && WinCoordinate_cameradepth.Y > 0 && WinCoordinate_cameradepth.Y < WindowHeight
                    && WinCoordinate_cameradepth.Z > 0)
                {

                    //将得到的摄像机深度屏幕坐标XY转换成一维数组中的位置
                    double depthdata_one_location_camera = WinCoordinate_cameradepth.Y * WindowWidth + WinCoordinate_cameradepth.X;
                    //取出摄像机深度图里面对应像素的深度
                    double depthdata_one_camera = depthdata_camera[(int)depthdata_one_location_camera];

                    //从局部坐标正算到摄像机的颜色屏幕坐标，这样就得到了摄像机视频应该映射的屏幕坐标。
                    coordinate WinCoordinate_cameracolor = new coordinate(0.0, 0.0, 0.0);
                    WinCoordinate_cameracolor = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameracolor, WinCoordinate_cameracolor);
                    //对颜色屏幕坐标进行取整，因为像素坐标只能是整数
                    WinCoordinate_cameracolor.X = (int)WinCoordinate_cameracolor.X;
                    WinCoordinate_cameracolor.Y = viewport_cameracolor[3] - (int)WinCoordinate_cameracolor.Y;//因为project得到的Y是OpenGL坐标


                    colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, colordata_camera_bmp.Height - (int)WinCoordinate_cameracolor.Y));

                    continue;
                    if (System.Math.Abs(WinCoordinate_cameradepth.Z - depthdata_one_camera) < 0.01)
                    {
                        //取摄像机视频图相应像素的RGB值，然后替换主视点相应像素的RGB值。目前是假设摄像机RGB图和深度图是同样的屏幕宽高，如果以后不一样要另算。
                        // var t = colordata_mainview[i];
                        //colordata_mainview[i] = colordata_camera[(int)colordata_one_location_camera];
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, WindowHeight -(int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        // if (t != colordata_mainview[i])
                        //     alert("主视点原来值：" + t + "    " + "主视点修改之后的值：" + colordata_mainview[i]);
                        //t1++;
                    }
                    else if (WinCoordinate_cameradepth.Z > depthdata_one_camera)
                    {
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        //t2++;
                    }
                    else
                    {
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        //colordata_mainview[i] = data_unsignedInt[colordata_one_location_camera];
                        //console.log(colordata_one_location_camera);
                        //t3++;
                    }
                }
            }

        }

        //Rectangle rect;
        //System.Drawing.Imaging.BitmapData bmpData;

        //int bytes;
        //byte[] rgbValues;
        public void fusion3(int WindowWidth, int WindowHeight, List<double> depthdata_mainview, Bitmap colordata_mainview_bmp, List<double> depthdata_camera, Bitmap colordata_camera_bmp, Matrix<double> modelviewMatrix_mainview, Matrix<double> projectMatrix_mainview, Vector<double> viewport_mainview, Matrix<double> modelviewMatrix_camera, Matrix<double> projectMatrix_camera, Vector<double> viewport_cameradepth, Vector<double> viewport_cameracolor)
        {
            //位图矩形
            Rectangle rect = new Rectangle(0, 0, colordata_mainview_bmp.Width, colordata_mainview_bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = colordata_mainview_bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, colordata_mainview_bmp.PixelFormat);
            //获得首地址
            IntPtr ptr = bmpData.Scan0;
            //定义被锁定的数组大小，由位图数据与未用空间组成
            int bytes = bmpData.Stride * bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            ////复制被锁定的位图像素值到该数组内
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < WindowWidth * WindowHeight; i++)
            {
                //根据一维数组中的位置，转换成主视图屏幕上的X,Y坐标。
                coordinate WinCoordinate_mainview = new coordinate(0.0, 0.0, 0.0);
                int int_iWindowWidth = ((int)(i / WindowWidth)) * WindowWidth;
                WinCoordinate_mainview.X = i - int_iWindowWidth;
                WinCoordinate_mainview.Y = (int)(i / WindowWidth);
                //从存储主视图屏幕深度信息的一维数组中取出对应的深度，即Z坐标。
                WinCoordinate_mainview.Z = depthdata_mainview[i];
                //从主视图屏幕的XYZ坐标反算到局部坐标
                coordinate ObjectCoordinate = new coordinate(0.0, 0.0, 0.0);
                ObjectCoordinate = Unproject(WinCoordinate_mainview, modelviewMatrix_mainview, projectMatrix_mainview, viewport_mainview, ObjectCoordinate);
                //再从局部坐标正算到摄像机的深度屏幕坐标，这样就得到了主视图屏幕上一点在摄像机的屏幕坐标和深度。
                coordinate WinCoordinate_cameradepth = new coordinate(0.0, 0.0, 0.0);
                WinCoordinate_cameradepth = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameradepth, WinCoordinate_cameradepth);
                //对深度屏幕坐标进行取整，因为像素坐标只能是整数
                WinCoordinate_cameradepth.X = (int)WinCoordinate_cameradepth.X;
                WinCoordinate_cameradepth.Y = WindowHeight - (int)WinCoordinate_cameradepth.Y;//因为project得到的Y是OpenGL坐标

                if (WinCoordinate_cameradepth.X > 0 && WinCoordinate_cameradepth.X < WindowWidth
                    && WinCoordinate_cameradepth.Y > 0 && WinCoordinate_cameradepth.Y < WindowHeight
                    && WinCoordinate_cameradepth.Z > 0)
                {
                    //将得到的摄像机深度屏幕坐标XY转换成一维数组中的位置
                    double depthdata_one_location_camera = WinCoordinate_cameradepth.Y * WindowWidth + WinCoordinate_cameradepth.X;
                    //取出摄像机深度图里面对应像素的深度
                    double depthdata_one_camera = depthdata_camera[(int)depthdata_one_location_camera];

                    //从局部坐标正算到摄像机的颜色屏幕坐标，这样就得到了摄像机视频应该映射的屏幕坐标。
                    coordinate WinCoordinate_cameracolor = new coordinate(0.0, 0.0, 0.0);
                    WinCoordinate_cameracolor = Project(ObjectCoordinate, modelviewMatrix_camera, projectMatrix_camera, viewport_cameracolor, WinCoordinate_cameracolor);
                    //对颜色屏幕坐标进行取整，因为像素坐标只能是整数
                    WinCoordinate_cameracolor.X = (int)WinCoordinate_cameracolor.X;
                    WinCoordinate_cameracolor.Y = viewport_cameracolor[3] - (int)WinCoordinate_cameracolor.Y;//因为project得到的Y是OpenGL坐标
                                                                                                             //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, colordata_camera_bmp.Height - (int)WinCoordinate_cameracolor.Y));

                    #region 融合
                    byte a1 = rgbValues[(int)WinCoordinate_mainview.X * 3 + ((int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1)];
                    //融合
                    rgbValues[(int)WinCoordinate_mainview.X * 3 + ((int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1) + 2] =
                        colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, colordata_camera_bmp.Height - (int)WinCoordinate_cameracolor.Y).R;
                    byte a2 = rgbValues[(int)WinCoordinate_mainview.X * 3 + ((int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1)];

                    rgbValues[(int)WinCoordinate_mainview.X * 3 + ((int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1) + 1] =
                        colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, colordata_camera_bmp.Height - (int)WinCoordinate_cameracolor.Y).G;
                    rgbValues[(int)WinCoordinate_mainview.X * 3 + ((int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1)] =
                        colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, colordata_camera_bmp.Height - (int)WinCoordinate_cameracolor.Y).B;

                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                    #endregion



                    continue;
                    if (System.Math.Abs(WinCoordinate_cameradepth.Z - depthdata_one_camera) < 0.01)
                    {
                        //取摄像机视频图相应像素的RGB值，然后替换主视点相应像素的RGB值。目前是假设摄像机RGB图和深度图是同样的屏幕宽高，如果以后不一样要另算。
                        // var t = colordata_mainview[i];
                        //colordata_mainview[i] = colordata_camera[(int)colordata_one_location_camera];
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, WindowHeight -(int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        // if (t != colordata_mainview[i])
                        //     alert("主视点原来值：" + t + "    " + "主视点修改之后的值：" + colordata_mainview[i]);
                        //t1++;
                    }
                    else if (WinCoordinate_cameradepth.Z > depthdata_one_camera)
                    {
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        //t2++;
                    }
                    else
                    {
                        //colordata_mainview_bmp.SetPixel((int)WinCoordinate_mainview.X, (int)WinCoordinate_mainview.Y, colordata_camera_bmp.GetPixel((int)WinCoordinate_cameracolor.X, (int)WinCoordinate_cameracolor.Y));
                        //colordata_mainview[i] = data_unsignedInt[colordata_one_location_camera];
                        //console.log(colordata_one_location_camera);
                        //t3++;
                    }
                }
            }
            colordata_mainview_bmp.UnlockBits(bmpData);
        }

    }
}
