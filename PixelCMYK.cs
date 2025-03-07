using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmitrijs.Lasenko_211LEB001
{
    public class PixelCMYK
    {
        public double C { get; }
        public double M { get; }
        public double Y { get; }
        public double K { get; }

        public PixelCMYK(byte r, byte g, byte b)
        {
            double newR = r / 255.0;
            double newG = g / 255.0;
            double newB = b / 255.0;
            // считаем К (чёрный цвет)
            K = 1 - Math.Max(Math.Max(newR, newG), newB);
            if ((1 - K) == 0)
            {
                // Расчитываеи компоненты C, M, Y если K = 1
                C = (1 - newR - K) / K;
                M = (1 - newG - K) / K;
                Y = (1 - newB - K) / K;
            }
            else
            {
                // ксли К НЕ РАВЕН 1
                C = (1 - newR - K) / (1 - K);
                M = (1 - newG - K) / (1 - K);
                Y = (1 - newB - K) / (1 - K);
            }
        }
    }
}