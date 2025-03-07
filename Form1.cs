using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dmitrijs.Lasenko_211LEB001
{
    public partial class Form1 : Form
    {
        private int a;
        private int b;
        private int c;

        public ImageClass imageClass = new ImageClass();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
                imageClass.ReadImage(bmp);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgOriginal, "RGB");
                imageClass.hst_original.drowHistogramm(chart1); // отоброжение оригинального изоброжения
                imageClass.hst_Costom.drowHistogramm(chart2); // созданного 

            }
        }
        //кнопки компонентов
        private void radioButton9_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null) // свичь того что происодит с гистограммой при выборе компонентов
            {
                RadioButton radbtn = (RadioButton)sender;
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgOriginal, radbtn.Text);

                switch (radbtn.Text)
                {
                    case "R":
                        imageClass.hst_Costom.readHistogrammRed(imageClass.imgCustom);
                        //данные о красном компоненте
                        break;
                    case "G":
                        imageClass.hst_Costom.readHistogrammGreen(imageClass.imgCustom);
                        // данные о зелёном компоненьте
                        break;
                    case "B":
                        imageClass.hst_Costom.readHistogrammBlue(imageClass.imgCustom);
                        // данные о синем компоненьте
                        break;
                    case "I":
                        imageClass.hst_Costom.readHistogrammIntensity(imageClass.imgCustom);
                        // данные о чёрном компоненьте
                        break;
                    case "H":
                        imageClass.hst_Costom.readHistogrammH(imageClass.imgHSV);
                        // данные о тоне
                        break;
                    case "S":
                        imageClass.hst_Costom.readHistogrammS(imageClass.imgHSV);
                        // данные о насыщенности
                        break;
                    case "V":
                        imageClass.hst_Costom.readHistogrammV(imageClass.imgHSV);
                        // данные о яркости
                        break;
                    default:
                        break;
                }

                imageClass.hst_Costom.drowHistogramm(chart2); // Обновление второй гистограммы в Chart2
            }
        }
        //метод для сохронения файла
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPEG Img File | *.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && pictureBox2.Image != null)
            {
                string fileName = saveFileDialog1.FileName;
                Bitmap bmp = (Bitmap)pictureBox2.Image.Clone();
                bmp.Save(fileName);
            }

            
        }
        // вычисляються и выводяться координаты пикселей певой картинки
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int imX = (int)((double)e.X / pictureBox1.Width * pictureBox1.Image.Width);
                int imY = (int)((double)e.Y / pictureBox1.Height * pictureBox1.Image.Height);

                if (imX >= 0 && imX < pictureBox1.Image.Width &&
                    imY >= 0 && imY < pictureBox1.Image.Height)
                {
                    Color pixelColor = ((Bitmap)pictureBox1.Image).GetPixel(imX, imY);
                    label1.Text = $"X: {imX} , Y:  {imY}, Color: R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}";
                }
            }
        }
        // вычисляються и выводяться координаты пикселей второй картинки 
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                int imX = (int)((double)e.X / pictureBox2.Width * pictureBox2.Image.Width);
                int imY = (int)((double)e.Y / pictureBox2.Height * pictureBox2.Image.Height);

                if (imX >= 0 && imX < pictureBox2.Image.Width &&
                    imY >= 0 && imY < pictureBox2.Image.Height)
                {
                    Color pixelColor = ((Bitmap)pictureBox2.Image).GetPixel(imX, imY);
                    label2.Text = $"X: {imX} , Y:  {imY}, Color: R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}";
                }
            }
        }
        private void button1_Click(object sender, EventArgs e) // poga lai atgriezt sakum stavikli atelu otraja boksa 
        {
            if (pictureBox2.Image != null)
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
                imageClass.ReadImage(bmp);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgOriginal, "RGB");
            }
        }
        //blur filtra masivu aizspildišana
        private void blurFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }

        private void blurFilter2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { 1, 1, 1 }, { 1, 10, 1 }, { 1, 1, 1 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }
        private void blurFilter3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }
        private void sharpenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }
        private void sharpenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }
        private void sharpenToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { -2, -2, -2 }, { -2, 17, -2 }, { -2, -2, -2 } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2
            }
        }

        private void mediana3x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] medianFilter = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                imageClass.MedianFilter3x3(medianFilter); // Вызываем метод из ImageClass
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); // Обновляем PictureBox2
            }
        }

        private void mediana5X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int[,] medianFilter = new int[5, 5] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };
                imageClass.MedianFilter5x5(medianFilter); // Вызываем метод из ImageClass
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); // Обновляем PictureBox2
            }
        }

        public void randomFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // генерируем рандомные значения 
            Random random = new Random();
            a = random.Next(-20, 10);
            b = random.Next(-10, 10);
            c = random.Next(10, 20);
            if (pictureBox1.Image != null)
            {
                int[,] f = new int[3, 3] { { b, a, b }, { a, b, a }, { b, c, b } };
                imageClass.FilterImage(f);
                pictureBox2.Image = imageClass.DrawImage(imageClass.imgCustom, "RGB"); //displaying an inverted image in PictureBox2

            }
        }
        //выводит рандомыне значения 
        private void label3_Click(object sender, EventArgs e)
        {
            label3.Text = $"a: {a} , b: {b} , c: {c}";
        }
    }
}