using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dmitrijs.Lasenko_211LEB001;
using System.Windows.Forms;

namespace Dmitrijs.Lasenko_211LEB001
{
    public class ImageClass
    {
        public PixelRGB[,] imgOriginal; // картинка в RGB которое мы загрузили
        public PixelRGB[,] imgCustom;   // картинка в RGB после обработки
        public PixelHSV[,] imgHSV;      // картинка в HSV
        public PixelCMYK[,] imgCMYK;    // картинка в CMYK
        public PixelYUV[,] imgYUV;      // картинка в YUV
        public HistogrammClass hst_original; // histogramm ориг изоброжения
        public HistogrammClass hst_Costom;  // histogramm изоброжения c которым поработали 

        //читаем изоброжение
        public void ReadImage(Bitmap bmp)
        {
            // инициализтруем масивы 
            imgOriginal = new PixelRGB[bmp.Width, bmp.Height];
            imgCustom = new PixelRGB[bmp.Width, bmp.Height];
            imgHSV = new PixelHSV[bmp.Width, bmp.Height];
            imgCMYK = new PixelCMYK[bmp.Width, bmp.Height];
            imgYUV = new PixelYUV[bmp.Width, bmp.Height];
            hst_original = new HistogrammClass();
            hst_Costom = new HistogrammClass();


            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            IntPtr ptr = IntPtr.Zero;
            int pixelComponents;

            if (bmpData.PixelFormat == PixelFormat.Format24bppRgb) pixelComponents = 3;
            else if (bmpData.PixelFormat == PixelFormat.Format32bppArgb) pixelComponents = 4;
            else pixelComponents = 0;

            var row = new byte[bmp.Width * pixelComponents];
            //var collumn = new byte[bmp.Height * pixelComponents];

            for (int y = 0; y < bmp.Height; y++)
            {
                ptr = bmpData.Scan0 + y * bmpData.Stride; //stride - skenesanas platums
                Marshal.Copy(ptr, row, 0, row.Length);
                // Инициализация пикселей разными цветовыми пространствами
                for (int x = 0; x < bmp.Width; x++)
                {
                    // оригинальное RGB
                    imgOriginal[x, y] = new PixelRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    // копируется из оригинала и обрабатывается
                    imgCustom[x, y] = new PixelRGB(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                    //преоброзование в HSV
                    imgHSV[x, y] = new PixelHSV(imgOriginal[x, y].R, imgOriginal[x, y].G, imgOriginal[x, y].B);

                    // Преобразование в CMYK
                    imgCMYK[x, y] = new PixelCMYK(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);

                    // Преобразование в YUV
                    imgYUV[x, y] = new PixelYUV(row[pixelComponents * x + 2], row[pixelComponents * x + 1], row[pixelComponents * x]);
                }
            }
            bmp.UnlockBits(bmpData);
            // инициалтзируем их
            hst_original.readHistogramm(imgOriginal);
            hst_Costom.readHistogramm(imgCustom);
        }
        //Метод который рисует изоброжение нужным нам оброзом в цветовом пространстве
        public Bitmap DrawImage(PixelRGB[,] img, string mode)
        {
            IntPtr ptr = IntPtr.Zero;

            var bmp = new Bitmap(img.GetLength(0), img.GetLength(1), PixelFormat.Format24bppRgb);

            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            var row = new byte[bmp.Width * 3];

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    switch (mode)
                    {
                        case "RGB":
                            {
                                row[3 * x + 2] = img[x, y].R;
                                row[3 * x + 1] = img[x, y].G;
                                row[3 * x] = img[x, y].B;
                                break;
                            }
                        case "R":
                            {
                                row[3 * x + 2] = img[x, y].R;
                                row[3 * x + 1] = 0;
                                row[3 * x] = 0;
                                break;
                            }
                        case "G":
                            {
                                row[3 * x + 2] = 0;
                                row[3 * x + 1] = img[x, y].G;
                                row[3 * x] = 0;
                                break;
                            }
                        case "B":
                            {
                                row[3 * x + 2] = 0;
                                row[3 * x + 1] = 0;
                                row[3 * x] = img[x, y].B;
                                break;
                            }
                        case "I":
                            {
                                row[3 * x + 2] = img[x, y].I;
                                row[3 * x + 1] = img[x, y].I;
                                row[3 * x] = img[x, y].I;
                                break;
                            }
                        case "HSV":
                            {
                                row[3 * x + 2] = img[x, y].hsvToRGB(imgHSV[x, y].H, imgHSV[x, y].S, imgHSV[x, y].V).R;
                                row[3 * x + 1] = img[x, y].hsvToRGB(imgHSV[x, y].H, imgHSV[x, y].S, imgHSV[x, y].V).G;
                                row[3 * x] = img[x, y].hsvToRGB(imgHSV[x, y].H, imgHSV[x, y].S, imgHSV[x, y].V).B;
                                break;
                            }
                        case "H":
                            {
                                row[3 * x + 2] = img[x, y].hsvToRGB(imgHSV[x, y].H, 255, 255).R;
                                row[3 * x + 1] = img[x, y].hsvToRGB(imgHSV[x, y].H, 255, 255).G;
                                row[3 * x] = img[x, y].hsvToRGB(imgHSV[x, y].H, 255, 255).B;
                                break;
                            }
                        case "S":
                            {
                                row[3 * x + 2] = imgHSV[x, y].S;
                                row[3 * x + 1] = imgHSV[x, y].S;
                                row[3 * x] = imgHSV[x, y].S;
                                break;
                            }
                        case "V":
                            {
                                row[3 * x + 2] = imgHSV[x, y].V;
                                row[3 * x + 1] = imgHSV[x, y].V;
                                row[3 * x] = imgHSV[x, y].V;
                                break;
                            }
                        case "CMYK":
                            {
                                row[3 * x + 2] = img[x, y].cmykToRGB(imgCMYK[x, y].C, imgCMYK[x, y].M, imgCMYK[x, y].Y, imgCMYK[x, y].K).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(imgCMYK[x, y].C, imgCMYK[x, y].M, imgCMYK[x, y].Y, imgCMYK[x, y].K).G;
                                row[3 * x] = img[x, y].cmykToRGB(imgCMYK[x, y].C, imgCMYK[x, y].M, imgCMYK[x, y].Y, imgCMYK[x, y].K).B;
                                break;
                            }
                        case "C":
                            {
                                row[3 * x + 2] = img[x, y].cmykToRGB(imgCMYK[x, y].C, 0, 0, 0).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(imgCMYK[x, y].C, 0, 0, 0).G;
                                row[3 * x] = img[x, y].cmykToRGB(imgCMYK[x, y].C, 0, 0, 0).B;
                                break;
                            }
                        case "M":
                            {
                                row[3 * x + 2] = img[x, y].cmykToRGB(0, imgCMYK[x, y].M, 0, 0).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(0, imgCMYK[x, y].M, 0, 0).G;
                                row[3 * x] = img[x, y].cmykToRGB(0, imgCMYK[x, y].M, 0, 0).B;
                                break;
                            }
                        case "Y":
                            {
                                row[3 * x + 2] = img[x, y].cmykToRGB(0, 0, imgCMYK[x, y].Y, 0).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(0, 0, imgCMYK[x, y].Y, 0).G;
                                row[3 * x] = img[x, y].cmykToRGB(0, 0, imgCMYK[x, y].Y, 0).B;
                                break;
                            }
                        case "K":
                            {
                                row[3 * x + 2] = img[x, y].cmykToRGB(0, 0, 0, imgCMYK[x, y].K).R;
                                row[3 * x + 1] = img[x, y].cmykToRGB(0, 0, 0, imgCMYK[x, y].K).G;
                                row[3 * x] = img[x, y].cmykToRGB(0, 0, 0, imgCMYK[x, y].K).B;
                                break;
                            }
                        case "YUV":
                            {
                                row[3 * x + 2] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, imgYUV[x, y].YUV_U, imgYUV[x, y].YUV_V).R;
                                row[3 * x + 1] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, imgYUV[x, y].YUV_U, imgYUV[x, y].YUV_V).G;
                                row[3 * x] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, imgYUV[x, y].YUV_U, imgYUV[x, y].YUV_V).B;
                                break;
                            }
                        case "YUV_Y":
                            {
                                row[3 * x + 2] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, 0, 0).R;
                                row[3 * x + 1] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, 0, 0).G;
                                row[3 * x] = img[x, y].yuvToRGB(imgYUV[x, y].YUV_Y, 0, 0).B;
                                break;
                            }
                        case "YUV_U":
                            {
                                row[3 * x + 2] = img[x, y].yuvToRGB(0, imgYUV[x, y].YUV_U, 0).R;
                                row[3 * x + 1] = img[x, y].yuvToRGB(0, imgYUV[x, y].YUV_U, 0).G;
                                row[3 * x] = img[x, y].yuvToRGB(0, imgYUV[x, y].YUV_U, 0).B;
                                break;
                            }
                        case "YUV_V":
                            {
                                row[3 * x + 2] = img[x, y].yuvToRGB(0, 0, imgYUV[x, y].YUV_V).R;
                                row[3 * x + 1] = img[x, y].yuvToRGB(0, 0, imgYUV[x, y].YUV_V).G;
                                row[3 * x] = img[x, y].yuvToRGB(0, 0, imgYUV[x, y].YUV_V).B;
                                break;
                            }
                    }
                }
                ptr = bmpData.Scan0 + y * bmpData.Stride;
                Marshal.Copy(row, 0, ptr, row.Length);
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

            //ld5
            public void FilterImage(int[,] f)
            {
                int k = 0;
                // Заполняем окна 3x3 для каждого канала
                for (int fi = 0; fi < 3; fi++)
                {
                    for (int fj = 0; fj < 3; fj++)
                    {
                        k += f[fi, fj];
                    }

                }
                for (int x = 1; x < imgOriginal.GetLength(0) - 1; x++)
                {
                    for (int y = 1; y < imgOriginal.GetLength(1) - 1; y++)
                    {
                        int r = 0;
                        int g = 0;
                        int b = 0;
                        int i = 0;

                        for (int fi = 0; fi < 3; fi++)
                        {
                            for (int fj = 0; fj < 3; fj++)
                            {
                                r += imgOriginal[x + fi - 1, y + fj - 1].R * f[fi, fj];
                                g += imgOriginal[x + fi - 1, y + fj - 1].G * f[fi, fj];
                                b += imgOriginal[x + fi - 1, y + fj - 1].B * f[fi, fj];
                                i += imgOriginal[x + fi - 1, y + fj - 1].I * f[fi, fj];
                            }
                        }

                        r = Math.Max(0, Math.Min(255, r / k));
                        g = Math.Max(0, Math.Min(255, g / k));
                        b = Math.Max(0, Math.Min(255, b / k));
                        i = Math.Max(0, Math.Min(255, i / k));

                        imgCustom[x, y].R = (byte)r;
                        imgCustom[x, y].G = (byte)g;
                        imgCustom[x, y].B = (byte)b;
                        imgCustom[x, y].I = (byte)i;
                    }
                }
            }

            public void MedianFilter3x3(int[,] filter)
            {
                int[,] windowR = new int[3, 3];
                int[,] windowG = new int[3, 3];
                int[,] windowB = new int[3, 3];

                for (int x = 1; x < imgOriginal.GetLength(0) - 1; x++)
                {
                    for (int y = 1; y < imgOriginal.GetLength(1) - 1; y++)
                    {
                        // Заполняем окна 3x3 для каждого канала
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                windowR[i, j] = imgOriginal[x + i - 1, y + j - 1].R;
                                windowG[i, j] = imgOriginal[x + i - 1, y + j - 1].G;
                                windowB[i, j] = imgOriginal[x + i - 1, y + j - 1].B;
                            }
                        }

                        // Применяем фильтр медианы
                        int[] flattenedWindowR = windowR.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowR);
                        int medianR = flattenedWindowR[4];

                        int[] flattenedWindowG = windowG.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowG);
                        int medianG = flattenedWindowG[4];

                        int[] flattenedWindowB = windowB.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowB);
                        int medianB = flattenedWindowB[4];

                        // Устанавливаем новые значения пиксел
                        imgCustom[x, y].R = (byte)medianR;
                        imgCustom[x, y].G = (byte)medianG;
                        imgCustom[x, y].B = (byte)medianB;
                    }
                }
            }
            public void MedianFilter5x5(int[,] filter)
            {
                int[,] windowR = new int[5, 5];
                int[,] windowG = new int[5, 5];
                int[,] windowB = new int[5, 5];

                for (int x = 2; x < imgOriginal.GetLength(0) - 2; x++)
                {
                    for (int y = 2; y < imgOriginal.GetLength(1) - 2; y++)
                    {
                        // Заполняем окна 3x3 для каждого канала
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                windowR[i, j] = imgOriginal[x + i - 2, y + j - 2].R;
                                windowG[i, j] = imgOriginal[x + i - 2, y + j - 2].G;
                                windowB[i, j] = imgOriginal[x + i - 2, y + j - 2].B;
                            }
                        }

                        int[] flattenedWindowR = windowR.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowR);
                        int medianR = flattenedWindowR[12];

                        int[] flattenedWindowG = windowG.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowG);
                        int medianG = flattenedWindowG[12];

                        int[] flattenedWindowB = windowB.Cast<int>().ToArray();
                        Array.Sort(flattenedWindowB);
                        int medianB = flattenedWindowB[12];

                        // Устанавливаем новые значения пиксел
                        imgCustom[x, y].R = (byte)medianR;
                        imgCustom[x, y].G = (byte)medianG;
                        imgCustom[x, y].B = (byte)medianB;
                    }
                }
            }
    }
 }