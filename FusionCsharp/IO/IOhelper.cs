using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FusionCsharp.model;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// 文件操作类
/// </summary>
namespace FusionCsharp.IO
{

    public class IOhelper
    {

        #region 读取TXT文件
        

        /// <summary>
        /// 得到主视图和摄像机的深度图
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<double> getdepthdata(string filename)
        {
            List<double> depthdata_list = new List<double>();

            StreamReader sr = File.OpenText(filename);
            string depth_string = "";
            string[] depth_array;
            try
            {
                while ((depth_string = sr.ReadLine()) != null)
                {
                    depth_array = depth_string.Split(',');
                    double depth_double = double.Parse(depth_array[0]);

                    depthdata_list.Add(depth_double);
                }
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                sr.Close();
            }
            return depthdata_list;
        }
        #endregion


        #region 读取bmp文件
        public static Bitmap readBMP(string filename)
        {
            Bitmap curBitmap = (Bitmap)Image.FromFile(filename);
            return curBitmap;
        }
        #endregion

 

    }
}
