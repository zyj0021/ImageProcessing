﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap bitmap;
        Bitmap newbitmap;
        Stopwatch sw = new Stopwatch();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                bitmap = (Bitmap)Image.FromFile(path);
                pictureBox1.Image = bitmap.Clone() as Image;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool isSave = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName.ToString();

                if (fileName != "" && fileName != null)
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();

                    System.Drawing.Imaging.ImageFormat imgformat = null;

                    if (fileExtName != "")
                    {
                        switch (fileExtName)
                        {
                            case "jpg":
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case "gif":
                                imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            default:
                                MessageBox.Show("只能存取为: jpg,bmp,gif 格式");
                                isSave = false;
                                break;
                        }

                    }

                    //默认保存为JPG格式   
                    if (imgformat == null)
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    if (isSave)
                    {
                        try
                        {
                            this.pictureBox2.Image.Save(fileName, imgformat);
                            //MessageBox.Show("图片已经成功保存!");   
                        }
                        catch
                        {
                            MessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                        }
                    }
                }
            }
        }

        /**
         * 调节图像亮度，默认系数为0.6
         * */
        private void button2_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                sw.Reset();
                sw.Restart();
                Color pixel;              
                int red, green, blue;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        red = (int)(pixel.R * 0.6);
                        green = (int)(pixel.G * 0.6);
                        blue = (int)(pixel.B * 0.6);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;
            }
        }

        /**
         * 添加暗角
         **/
        private void button1_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;

                sw.Reset();
                sw.Restart();

                int width = newbitmap.Width;
                int height = newbitmap.Height;
                float cx = width / 2;
                float cy = height / 2;
                float maxDist = cx * cx + cy * cy;
                float currDist = 0, factor;
                Color pixel; 
                
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        currDist = ((float)i - cx) * ((float)i - cx) + ((float)j - cy) * ((float)j - cy);
                        factor = currDist / maxDist;

                        pixel = newbitmap.GetPixel(i, j);
                        int red = (int)(pixel.R * (1 - factor));
                        int green = (int)(pixel.G * (1 - factor));
                        int blue = (int)(pixel.B * (1 - factor));
                        newbitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }

                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;               
            }
        }

        /**
         * 去色（灰度）
         * */
        private void button3_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                sw.Reset();
                sw.Restart();
                Color pixel;
                int gray;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                    }
                }
                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;
            }
        }
    
        /**
         * 马赛克效果
         * 默认的效果为2格
         * */
        private void button5_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                sw.Reset();
                sw.Restart();
                int RIDIO = 20;//马赛克的尺度，默认为周围两个像素
                for (int h = 0; h < newbitmap.Height; h += RIDIO)
                {
                    for (int w = 0; w < newbitmap.Width; w += RIDIO)
                    {
                        int avgRed = 0, avgGreen = 0, avgBlue = 0;
                        int count = 0;
                        //取周围的像素
                        for (int x = w; (x < w + RIDIO && x < newbitmap.Width); x++)
                        {
                            for (int y = h; (y < h + RIDIO && y < newbitmap.Height); y++)
                            {
                                Color pixel = newbitmap.GetPixel(x,y);
                                avgRed += pixel.R;
                                avgGreen += pixel.G;
                                avgBlue += pixel.B;
                                count++;
                            }
                        }

                        //取平均值
                        avgRed = avgRed / count;
                        avgBlue = avgBlue / count;
                        avgGreen = avgGreen / count;

                        //设置颜色
                        for (int x = w; (x < w + RIDIO && x < newbitmap.Width); x++)
                        {
                            for (int y = h; (y < h + RIDIO && y < newbitmap.Height); y++)
                            {
                                Color newColor = Color.FromArgb(avgRed, avgGreen ,avgBlue);
                                newbitmap.SetPixel(x, y, newColor);
                            }
                        }
                    }
                }
                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;
            }
        }

        /**
         * 扩散效果
         * */
        private void button6_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                sw.Reset();
                sw.Restart();
                Color pixel;
                int red, green, blue;
                int flag = 0;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        Random ran = new Random();
                        int RankKey = ran.Next(-5, 5);
                        if (x + RankKey >= newbitmap.Width || y + RankKey >= newbitmap.Height || x + RankKey < 0 || y + RankKey < 0)
                        {
                            flag = 1;
                            continue;
                        }
                        
                        pixel = newbitmap.GetPixel(x + RankKey, y + RankKey);
                        red = (int)(pixel.R);
                        green = (int)(pixel.G);
                        blue = (int)(pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;
            }
        }

        /**
         * 浮雕效果
         * */
        private void button4_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                sw.Reset();
                sw.Restart();
                Color pixel;
                int red, green, blue;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        red = (int)(255 - pixel.R);
                        green = (int)(255 -  pixel.G);
                        blue = (int)(255 - pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
                sw.Stop();
                timer.Text = sw.ElapsedMilliseconds.ToString();
                pictureBox2.Image = newbitmap.Clone() as Image;
            }
        }

        

    
       
    }
}
