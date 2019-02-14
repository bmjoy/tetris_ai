using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace showarrincolor
{
	public partial class Form1 : Form
	{
		/// <summary>
		/// 一个三维数组，预定义的
		/// </summary>
		private double[,,] arr;
		/// <summary>
		/// 计数的
		/// </summary>
		int ii = 0;
		/// <summary>
		/// 初始化
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 绘制picturebox1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			if (arr != null)
			{
				int columns = arr.GetLength(1);
				int rows = arr.GetLength(2);
				Graphics g = e.Graphics;
				for (int i = 0; i < columns; i++)
					for (int j = 0; j < rows; j++)
					{
						double temp = arr[0, i, j];
						if (temp >= 0)
						{
							if (temp > 1) temp = 1;
							Brush b = new SolidBrush(Color.FromArgb((int)(255 * temp), Color.DarkGreen));
							g.FillRectangle(b, 24 * i + 4, 24 * (rows - 1 - j) + 4, 20, 20);
						}
						else
						{
							if (temp < -1) temp = -1;
							Brush b = new SolidBrush(Color.FromArgb((int)(255 * (-temp)), Color.DarkOrange));
							g.FillRectangle(b, 24 * i + 4, 24 * (rows - 1 - j) + 4, 20, 20);
						}
					}
			}
		}
		/// <summary>
		/// 从xml里获得三维数组还有w0，b的值
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private double[,,] Getxmlarr(string s = "")
		{
			DataSet ds = new DataSet();
			string pathbase = Environment.CurrentDirectory;
			string path = pathbase + "\\model" + s + ".xml";
			try
			{
				ds.ReadXml(path);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("找不到model.xml");
				return null;
			}
			double[,] w = new double[2, 200];
			double[,,] new_w = new double[2, 10, 20];
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 200; i++)
				{
					w[j, i] = double.Parse(ds.Tables["w"].Rows[i][j].ToString());
				}
			}
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 10; i++)
					for (int k = 0; k < 20; k++)
					{
						new_w[j, i, k] = w[j, i * 20 + k];
					}
			}
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 1; i++)
				{
					textBox1.Text += ds.Tables["w0"].Rows[i][j].ToString() + "   ";
					textBox2.Text += ds.Tables["b"].Rows[i][j].ToString() + "   ";
				}
			}
			textBox1.Text += Environment.NewLine;
			textBox2.Text += Environment.NewLine;
			return new_w;
		}
		/// <summary>
		/// 绘制picturebox2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox2_Paint(object sender, PaintEventArgs e)
		{
			if (arr != null)
			{
				int columns = arr.GetLength(1);
				int rows = arr.GetLength(2);
				Graphics g = e.Graphics;
				for (int i = 0; i < columns; i++)
					for (int j = 0; j < rows; j++)
					{
						double temp = arr[1, i, j];
						if (temp >= 0)
						{
							if (temp > 1) temp = 1;
							Brush b = new SolidBrush(Color.FromArgb((int)(255 * temp), Color.DarkGreen));
							g.FillRectangle(b, 24 * i + 4, 24 * (rows - 1 - j) + 4, 20, 20);
						}
						else
						{
							if (temp < -1) temp = -1;
							Brush b = new SolidBrush(Color.FromArgb((int)(255 * (-temp)), Color.DarkOrange));
							g.FillRectangle(b, 24 * i + 4, 24 * (rows - 1 - j) + 4, 20, 20);
						}
					}
			}
		}
		/// <summary>
		/// 启动按钮呀
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			ii = 10;
			timer1.Start();
		}
		/// <summary>
		/// 放视频的(～￣▽￣)～ 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (ii <= 600)
			{
				arr = Getxmlarr(ii.ToString());
				pictureBox1.Refresh();
				pictureBox2.Refresh();
				//getpng();
				ii += 10;
			}
			else
			{
				//string str;
				//str = textBox1.Text;
				//string pathbase = Environment.CurrentDirectory;
				//string path = pathbase + "\\w0.txt";
				//StreamWriter sw = new StreamWriter(path, false);
				//sw.WriteLine(str);
				//sw.Close();//写入
				//str = textBox2.Text;
				//pathbase = Environment.CurrentDirectory;
				//path = pathbase + "\\b.txt";
				//sw = new StreamWriter(path, false);
				//sw.WriteLine(str);
				//sw.Close();//写入
				timer1.Enabled = false;
				//MessageBox.Show("输出完成");
				//Application.Exit();
			}
		}
		/// <summary>
		/// 把控件屏幕截图下来（当且仅当系统缩放100%）
		/// </summary>
		private void getpng()
		{
			int columns = arr.GetLength(1);
			int rows = arr.GetLength(2);
			Bitmap bit = new Bitmap(24 * columns, 24 * rows);//实例化一个和窗体一样大的bitmap
			Graphics g = Graphics.FromImage(bit);
			g.CompositingQuality = CompositingQuality.Default;//质量设为最高
			g.CopyFromScreen(pictureBox1.PointToScreen(Point.Empty), Point.Empty, new Size(24 * columns, 24 * rows));
			bit.Save("w1" + ii + ".png");
			g.CopyFromScreen(pictureBox2.PointToScreen(Point.Empty), Point.Empty, new Size(24 * columns, 24 * rows));
			bit.Save("w2" + ii + ".png");
		}
	}
}
