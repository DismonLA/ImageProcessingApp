using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmitrijs.Lasenko_211LEB001
{
    public class PixelYUV
    {
        //компонентный конструктор
        public double YUV_Y { get; set; }
        public double YUV_U { get; set; }
        public double YUV_V { get; set; }

        public PixelYUV()
        {
            YUV_Y = 0;
            YUV_U = 0;
            YUV_V = 0;
        }
        //Принимаеи значения RGB и вычисяем из них YUV по формуле из презинтации
        public PixelYUV(byte R, byte G, byte B)
        {
            YUV_Y = (double)((0.299 * R) + (0.587 * G) + (0.114 * B));
            YUV_U = (double)((-0.14713 * R) - (0.28886 * G) + (0.436 * B) + 128);
            YUV_V = (double)((0.615 * R) - (0.51499 * G) - (0.10001 * B) + 128);
        }
        
    }
}