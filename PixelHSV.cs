using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dmitrijs.Lasenko_211LEB001
{
    public class PixelHSV
    {
        public int H;
        public byte S;
        public byte V;

        public PixelHSV(byte r, byte g, byte b)
        {
            // конвертируем ргб в хсв
            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);
            float delta = max - min;

            // цвет тона
            if (delta == 0)
            {
                H = 0; // если так то цвет не меняеться 
            }
            else if (max == r)
            {
                H = (int)(60 * (((g - b) / delta) % 6));
            }
            else if (max == g) // если максимальный цвет зелённый то оттеноке по формуле считаем
            {
                H = (int)(60 * (((b - r) / delta) + 2));
            }
            else // если синий
            {
                H = (int)(60 * (((r - g) / delta) + 4));
            }

            if (H < 0)
            {
                H += 360; // пишем приделы оттенка
            }

            // считаем насышеность 
            if (max == 0)
            {
                S = 0; // если 0 то это чёрный
            }
            else
            {
                S = (byte)((delta / max) * 255);
            }

            // яркость
            V = (byte)max;
        }
    }
}
