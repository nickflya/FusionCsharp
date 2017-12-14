using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 坐标系模型
/// </summary>
namespace FusionCsharp.model
{
    public class coordinate
    {
        double x;
        double y;
        double z;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        public coordinate(double x , double y,double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
