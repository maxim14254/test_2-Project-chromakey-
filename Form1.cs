using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = null;// очищаем от прошлой картинки 
                bitmap= new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bitmap;// вставляем новую картинку
            }
        }
        private List<Pixel> GetPixels(Bitmap bitmap)// метод получения коллекции пикселеей
        {
            List < Pixel > pixels = new List<Pixel>(bitmap.Width * bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++) 
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    pixels.Add(new Pixel { color = bitmap.GetPixel(x, y), 
                                            point = new Point() { X = x, Y = y } 
                                           });
                }
            }
            return pixels;
        }

        private async void  пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() => Rezult());// асинхронно запускаем метод подсчета и вывода результата
        }

        private void Rezult()
        {
            if (bitmap == null)// проверка на null
                return;
            List<Pixel> pixels = GetPixels(bitmap);
            double G; // переменная для хранения интенсивности цвета
            double R;
            double B;
            List<Pixel> newPixels = new List<Pixel>();
            for (int i = 0; i < pixels.Count; i++)
            {
                G = pixels[i].color.G;
                R = pixels[i].color.R;
                B = pixels[i].color.B;
                int e = 220;
                bool b = !(((G-3) / R >= (255.0-3) / e && G < 255 && (G-3) / B >= (255.0-3) / e && G>=30) ||
                        (B >= (255 / (255.0 - e)+0.3) * (R - e) && G == 255 && R >= (255 / (255.0 - e)+0.3) * (B - e) && (R + B + G <= 750))); //  если это условие false то данный пиксель-зеленый (в скобочках должно быть true)                //!(((G - 15) / R >= (255.0 - 15) / e && G < 255 && (G - 15) / B >= (255.0 - 15) / e) || 
                                                                                                                                                                                                                                                   //(B >= (255 / (255.0 - e) + 0.4) * (R - e) && G == 255 && R >= (255 / (255.0 - e) + 0.4) * (B - e))); 
                if (b)                                                                         
                {
                    newPixels.Add(pixels[i]);// добавляем полученные пиксели (все кроме зеленого)
                }
            }
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int i = 0; i < newPixels.Count; i++)
            {
                newBitmap.SetPixel(newPixels[i].point.X, newPixels[i].point.Y, newPixels[i].color);
            }
            pictureBox1.Image = newBitmap;
        }
    }
}
