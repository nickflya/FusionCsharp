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
        /// 得到矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix<double> getMatrix(double[,] Matrix)
        {
            return mb.DenseOfArray(Matrix);
        }


        /// <summary>
        /// 得到向量
        /// </summary>
        /// <returns></returns>
        public Vector<double> getVector(double[] vector)
        {
            double[] a = { 0.0, 0.0, 1000.0, 800.0 };
            return vb.DenseOfArray(a);
        }

        public void funsion4(List<relationshipMainviewCamera> list_relationshipMainviewCamera, Bitmap colordata_mainview_bmp, Bitmap colordata_camera_bmp)
        {
            //内存法
            System.Drawing.Imaging.BitmapData bmpdata_mainview = null;
            System.Drawing.Imaging.BitmapData bmpdata_camera = null;
            try
            {


                Rectangle rect_mainview = new Rectangle(0, 0, colordata_mainview_bmp.Width, colordata_mainview_bmp.Height);
                bmpdata_mainview = colordata_mainview_bmp.LockBits(rect_mainview, System.Drawing.Imaging.ImageLockMode.ReadWrite, colordata_mainview_bmp.PixelFormat);
                IntPtr ptr_mainview = bmpdata_mainview.Scan0;
                int bytes_mainview = colordata_mainview_bmp.Width * colordata_mainview_bmp.Height * 3;
                byte[] rgbvalues_mainview = new byte[bytes_mainview];
                System.Runtime.InteropServices.Marshal.Copy(ptr_mainview, rgbvalues_mainview, 0, bytes_mainview);

                Rectangle rect_camera = new Rectangle(0, 0, colordata_camera_bmp.Width, colordata_camera_bmp.Height);
                bmpdata_camera = colordata_camera_bmp.LockBits(rect_camera, System.Drawing.Imaging.ImageLockMode.ReadWrite, colordata_camera_bmp.PixelFormat);
                IntPtr ptr_camera = bmpdata_camera.Scan0;
                int bytes_camera = colordata_camera_bmp.Width * colordata_camera_bmp.Height * 3;
                byte[] rgbvalues_camera = new byte[bytes_camera];
                System.Runtime.InteropServices.Marshal.Copy(ptr_camera, rgbvalues_camera, 0, bytes_camera);


                foreach (var relationship in list_relationshipMainviewCamera)
                {
                    rgbvalues_mainview[relationship.Mainview_y * bmpdata_mainview.Stride + relationship.Mainview_x * 3 + 2] =
                rgbvalues_camera[relationship.Camera_y * bmpdata_camera.Stride + relationship.Camera_x * 3 + 2];
                    rgbvalues_mainview[relationship.Mainview_y * bmpdata_mainview.Stride + relationship.Mainview_x * 3 + 1] =
                        rgbvalues_camera[relationship.Camera_y * bmpdata_camera.Stride + relationship.Camera_x * 3 + 1];
                    rgbvalues_mainview[relationship.Mainview_y * bmpdata_mainview.Stride + relationship.Mainview_x * 3] =
                        rgbvalues_camera[relationship.Camera_y * bmpdata_camera.Stride + relationship.Camera_x * 3];

                    ////提取像素法
                    //colordata_mainview_bmp.SetPixel(relationship.Mainview_x, relationship.Mainview_y, colordata_camera_bmp.GetPixel(relationship.Camera_x, relationship.Camera_y));

                }
                System.Runtime.InteropServices.Marshal.Copy(rgbvalues_mainview, 0, ptr_mainview, bytes_mainview);
                colordata_mainview_bmp.UnlockBits(bmpdata_mainview);
                //System.Runtime.InteropServices.Marshal.Copy(rgbvalues_camera, 0, ptr_camera, bytes_camera);
                colordata_camera_bmp.UnlockBits(bmpdata_camera);
            }
            catch (System.Exception ex)
            {
                //if (bmpdata_mainview != null)
                //    colordata_mainview_bmp.UnlockBits(bmpdata_mainview);
                //if (bmpdata_camera != null)
                //    colordata_camera_bmp.UnlockBits(bmpdata_camera);
            }

        }

        public List<relationshipMainviewCamera> getRelationship(int WindowWidth, int WindowHeight, List<double> depthdata_mainview, List<double> depthdata_camera, Matrix<double> modelviewMatrix_mainview, Matrix<double> projectMatrix_mainview, Vector<double> viewport_mainview, Matrix<double> modelviewMatrix_camera, Matrix<double> projectMatrix_camera, Vector<double> viewport_cameradepth, Vector<double> viewport_cameracolor)
        {
            List<relationshipMainviewCamera> list_relationshipMainviewCamera = new List<relationshipMainviewCamera>();
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
                    relationshipMainviewCamera relationship = new relationshipMainviewCamera();
                    relationship.Mainview_x = (int)WinCoordinate_mainview.X;
                    relationship.Mainview_y = (int)viewport_mainview[3] - (int)WinCoordinate_mainview.Y - 1;
                    relationship.Camera_x = (int)WinCoordinate_cameracolor.X;
                    relationship.Camera_y = (int)viewport_cameracolor[3] - (int)WinCoordinate_cameracolor.Y;
                    list_relationshipMainviewCamera.Add(relationship);
                }

            }
            return list_relationshipMainviewCamera;
        }
    }
}
