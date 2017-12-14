using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 表示RGB
/// </summary>
namespace FusionCsharp.model
{
    public class colorRGB
    {
        private double r;
        private double g;
        private double b;

        public double R { get => r; set => r = value; }
        public double G { get => g; set => g = value; }
        public double B { get => b; set => b = value; }

        public colorRGB(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
