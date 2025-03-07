using Dmitrijs.Lasenko_211LEB001;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Dmitrijs.Lasenko_211LEB001
{
    public class HistogrammClass
    {
        public int[] hR;
        public int[] hI;
        public int[] hG;
        public int[] hB;
        public int[] hH;
        public int[] hS;
        public int[] hV;

        public HistogrammClass() // диапозон масивов для компонентов
        {
            hR = new int[256];
            hI = new int[256];
            hG = new int[256];
            hB = new int[256];
            hH = new int[361];
            hS = new int[256];
            hV = new int[256];
        }
        public void eraseHistogramm() // метод для обнуления значений в гистограммах
        {
            for (int i = 0; i < 256; i++)
            {
                hR[i] = 0;
                hI[i] = 0;
                hG[i] = 0;
                hB[i] = 0;
                hH[i] = 0;
                hS[i] = 0;
                hV[i] = 0;
            }
        }
        // Метод для считывания гистограммы изоброжения для RGB
        public void readHistogramm(PixelRGB[,] img)
        {
            // тут мы перебираем все пиксели и увеличиваем соотвецтвующие значения в гистограмме.
            eraseHistogramm();
            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    hR[img[x, y].R]++;
                    hI[img[x, y].I]++;
                    hG[img[x, y].G]++;
                    hB[img[x, y].B]++;

                }
            }
        }

        public void drowHistogramm(Chart chart)
        {
            // методы для гистограмы каждого жлемента 
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add("ChartArea");

            chart.Series.Add("R");
            chart.Series["R"].Color = Color.Red;

            chart.Series.Add("G");
            chart.Series["G"].Color = Color.Green;

            chart.Series.Add("B");
            chart.Series["B"].Color = Color.Blue;

            chart.Series.Add("I");
            chart.Series["I"].Color = Color.Black;

            chart.Series.Add("H");
            chart.Series["H"].Color = Color.Cyan;

            chart.Series.Add("S");
            chart.Series["S"].Color = Color.Magenta;

            chart.Series.Add("V");
            chart.Series["V"].Color = Color.Yellow;


            // заполнение гистограм

            for (int i = 0; i < 256; i++)
            {
                chart.Series["R"].Points.AddXY(i, hR[i]);
                chart.Series["I"].Points.AddXY(i, hI[i]);
                chart.Series["G"].Points.AddXY(i, hG[i]);
                chart.Series["B"].Points.AddXY(i, hB[i]);

            }
            
            for (int i = 0; i < 256; i++)
            {
                chart.Series["H"].Points.AddXY(i, hH[i]);
                chart.Series["S"].Points.AddXY(i, hS[i]);
                chart.Series["V"].Points.AddXY(i, hV[i]);
            }
        }

        // дальше идут методы которые считывают значение каждого цвета а ргб и хсв
        public void readHistogrammRed(PixelRGB[,] img)
        {
            eraseHistogramm();
            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    hR[img[x, y].R]++;
                }
            }
        }

        public void readHistogrammGreen(PixelRGB[,] img)
        {
            eraseHistogramm();
            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    hG[img[x, y].G]++;
                }
            }
        }

        public void readHistogrammBlue(PixelRGB[,] img)
        {
            eraseHistogramm();
            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    hB[img[x, y].B]++;
                }
            }
        }
        public void readHistogrammIntensity(PixelRGB[,] img)
        {
            eraseHistogramm();
            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    hI[img[x, y].I]++;
                }
            }
        }

        public void readHistogrammH(PixelHSV[,] img)
        {
            eraseHistogramm();
            int maxHValue = 360; // Максимальное значение оттенка

            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    int hValue = img[x, y].H;
                    //тут происходит проверка есть ли элемент максива в нужном нам дипазаоне т.к были с этим проблемы.
                    if (hValue >= 0 && hValue <= maxHValue)
                    {
                        hH[hValue]++;
                    }
                }
            }
        }
        public void readHistogrammS(PixelHSV[,] img)
        {
            eraseHistogramm();
            int maxSValue = 256; // Максимальное значение насыщенности

            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    int sValue = img[x, y].S;
                   
                    if (sValue >= 0 && sValue <= maxSValue)
                    {
                        hS[sValue]++;
                    }
                }
            }
        }

        public void readHistogrammV(PixelHSV[,] img)
        {
            eraseHistogramm();
            int maxVValue = 256; // Максимальное значение значения

            for (int x = 0; x < img.GetLength(0); x++)
            {
                for (int y = 0; y < img.GetLength(1); y++)
                {
                    int vValue = img[x, y].V;

                    if (vValue >= 0 && vValue <= maxVValue)
                    {
                        hV[vValue]++;
                    }
                }
            }
        }

    }
}
