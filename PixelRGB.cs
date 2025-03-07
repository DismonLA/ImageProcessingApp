using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmitrijs.Lasenko_211LEB001
{
    
    public class PixelRGB
    {

        public byte R;
        public byte G;
        public byte B;
        public byte I;

        public PixelRGB()
        {
            R = 0;
            G = 0;
            B = 0;
            I = 0;
        }

        public PixelRGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            // Расчитываем интенсивность
            I = (byte)Math.Round(0.073f * b + 0.715 * g + 0.212f * r);
        }
        // Метод преобразующий CMYK в RGB
        public PixelRGB cmykToRGB(double c, double m, double y, double k)
        {
            
            double r = (255 * (1 - c) * (1 - k));
            double g = (255 * (1 - m) * (1 - k));
            double b = (255 * (1 - y) * (1 - k));

            return new PixelRGB((byte)r, (byte)g, (byte)b);
        }
        // Метод преобразующий YUV в RGB
        public PixelRGB yuvToRGB(double Y, double U, double V)
        {
            byte R, G, B = 0;

            R = (byte)(Y + 1.13983 * (V - 128));
            G = (byte)(Y - 0.39465 * (U - 128) - 0.58060 * (V - 128));
            B = (byte)(Y + 2.03211 * (U - 128));

            PixelRGB rgbPix = new PixelRGB(R, G, B);
            return rgbPix;
        }
        // Метод преобразующий  HSV в RGB
        public PixelRGB hsvToRGB(int h, byte s, byte v)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            int Hi = Convert.ToInt32(h / 60);
            byte Vmin = Convert.ToByte((255 - s) * v / 255);
            int a = Convert.ToInt32((v - Vmin) * (h % 60) / 60);
            byte Vinc = Convert.ToByte(Vmin + a);
            byte Vdec = Convert.ToByte(v - a);


            switch (Hi)
            {
                case 0: { r = v; g = Vinc; b = Vmin; break; }
                case 1: { r = Vdec; g = v; b = Vmin; break; }
                case 2: { r = Vmin; g = v; b = Vinc; break; }
                case 3: { r = Vmin; g = Vdec; b = v; break; }
                case 4: { r = Vinc; g = Vmin; b = v; break; }
                case 5: { r = v; g = Vmin; b = Vdec; break; }
            }

            PixelRGB rgbPix = new PixelRGB(r, g, b);
            return rgbPix;
        }
    }
}
