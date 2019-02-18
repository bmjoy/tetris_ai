using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace tetris_ai_cn
{
	public partial class Form1 : Form
	{
		//代码中左下角为原点，向右X正向上Y正。这和Fillrect（左上角原点）不同，所以只在绘图时转换。
		//数据区
		/// <summary>
		/// 列数，最大x坐标+1
		/// </summary>
		public static readonly int columns = 10;
		/// <summary>
		/// 行数，最大y坐标+1
		/// </summary>
		public static readonly int rows = 20;
		/// <summary>
		/// 背景图矩阵，存放0无方块，1有方块
		/// </summary>
		public static double[,] arr;
		/// <summary>
		/// 板块对象
		/// </summary>
		public Brick curbrick;
		/// <summary>
		/// 是否由AI控制，哪个AI控制
		/// </summary>
		int aichoose = 0;
		/// <summary>
		/// 当前分数
		/// </summary>
		public int score = 0;
		/// <summary>
		/// <returns>List（int）类型，第一个参数是可消总行数，接下来的项则是可消行的y坐标，从大到小排列</returns>
		/// </summary>
		List<int> countrows = new List<int> { 0 };
		/// <summary>
		/// 防止AI重复判断
		/// </summary>
		bool aihasdecided = false;
		/// <summary>
		/// 控制AI训练和测试
		/// </summary>
		bool aiistraining = false;
		//函数区
		/// <summary>
		/// 初始化所有控件
		/// </summary>
		public Form1()
		{
			arr = new double[columns, rows];
			InitializeComponent();
			pictureBox1.Size = new Size(24 * columns + 4, 24 * rows + 4);
			label2.Text = "当前分数：" + score + "分";
			timer1.Enabled = false;
			textBox1.Text = "准备就绪，" + Environment.NewLine + "上方向键旋转，左方向键左移，右方向键右移，下方向键加速下移";
		}
		/// <summary>
		/// AI控制的入口函数，添加AI请在此注册
		/// </summary>
		public void Aicontrol()
		{
			switch (aichoose)
			{
				case 0: return;
				case 1: AI_1(); break;
				case 2: AI_2(); break;
				case 3: AI_3(); break;
				case 4: AI_4(); break;
				default: break;
			}
		}
		//以下不建议修改
		/// <summary>
		/// 定时自动下落,相当于main函数，游戏主进程
		/// </summary>
		private void timer1_Tick(object sender, EventArgs e)
		{
			Cleanrows(countrows);
			countrows = new List<int> { 0 };
			if (curbrick == null)
			{
				curbrick = new Brick();
				aihasdecided = false;
			}
			timer1.Stop();
			if (!aihasdecided)
			{
				Aicontrol();
				aihasdecided = true;
			}
			curbrick.Canmove(curbrick.pos);
			timer1.Start();
			lock (curbrick)
			{
				for (int i = 0; i < columns; i++)
				{
					if (arr[i, rows - 1] == 1)
					{
						timer1.Stop();
						MessageBox.Show("游戏结束！");
						return;
					}
				}
				if (!curbrick.Dropmove())
				{
					Fillarr(arr, curbrick.posnodes);
					countrows = CountRow(arr);
					curbrick = null;
				}
				pictureBox1.Refresh();
				label2.Text = "当前分数：" + score + "分";
			}
		}
		/// <summary>
		/// 用于把板块写入背景图arr中，只有板块不能下落时才可以调用
		/// </summary>
		/// <param name="arr">背景图矩阵，int型二维数组</param>
		/// <param name="posnodes">以稀疏矩阵的方式存储每个方块对应背景矩阵arr的位置，List（Node）格式</param>
		public void Fillarr(double[,] arr, List<Node> posnodes)
		{
			foreach (Node item in posnodes)
			{
				arr[item.x, item.y] = 1;
			}
		}
		/// <summary>
		/// 对一个二维数组（矩阵）计算可消行数，与Cleanrows配套使用
		/// </summary>
		/// <param name="arr">一个二维数组</param>
		/// <returns>List（int）类型，第一个参数是可消总行数，接下来的项则是可消行的y坐标，从大到小排列</returns>
		public List<int> CountRow(double[,] arr)
		{
			List<int> countrows = new List<int>
			{
				0
			};
			bool isfull;
			for (int i = rows - 1; i >= 0; i--)
			{
				isfull = true;
				for (int j = 0; j < columns; j++)
				{
					if (arr[j, i] == 0)
					{
						isfull = false;
						break;
					}
				}
				if (isfull)
				{
					countrows[0]++;
					countrows.Add(i);
				}
			}
			return countrows;
		}
		/// <summary>
		/// 清除已满的行，与CountRow配套使用
		/// </summary>
		/// <param name="countrows">List（int）类型，第一个参数是可消总行数，接下来项的则是可消行的y坐标，从大到小排列</param>
		private void Cleanrows(List<int> countrows)
		{
			score += countrows[0];
			countrows.RemoveAt(0);
			foreach (int item in countrows)
				for (int i = 0; i < columns; i++)
					for (int j = item; j < rows - 1; j++)
					{
						arr[i, j] = arr[i, j + 1];
					}
		}
		//以下控件相关函数
		/// <summary>
		/// 新局
		/// </summary>
		private void button1_Click(object sender, EventArgs e)
		{
			timer1.Stop();
			arr = new double[columns, rows];
			curbrick = null;
			pictureBox1.Refresh();
			score = 0;
			timer1.Start();
			groupBox3.Enabled = !timer1.Enabled;
			w = new double[2, 200];
			w0 = new double[2, 1];
			b = new double[2, 1];
		}
		/// <summary>
		/// 暂停
		/// </summary>
		private void button2_Click(object sender, EventArgs e)
		{
			timer1.Stop();
			groupBox3.Enabled = !timer1.Enabled;
		}
		/// <summary>
		///继续
		/// </summary>
		private void button3_Click(object sender, EventArgs e)
		{
			timer1.Start();
			groupBox3.Enabled = !timer1.Enabled;
		}
		/// <summary>
		/// 控制下落快慢
		/// </summary>
		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			timer1.Interval = 200 * trackBar1.Value + 10;
		}
		/// <summary>
		/// 无AI
		/// </summary>
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 0;
			textBox1.Text = "切换：无AI，进入手动操作";
		}
		/// <summary>
		/// 选择一号AI
		/// </summary>
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 1;
			textBox1.Text = "切换：一号AI，将不响应用户方向键";
		}
		/// <summary>
		/// 选择二号AI
		/// </summary>
		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 2;
			textBox1.Text = "切换：二号AI，将不响应用户方向键";
		}
		/// <summary>
		/// 渲染绘图区
		/// </summary>
		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			for (int i = 0; i < columns; i++)
				for (int j = 0; j < rows; j++)
				{
					if (arr[i, j] == 1) g.FillRectangle(Brushes.LightBlue, 24 * i + 4, 24 * (rows - 1 - j) + 4, 20, 20);
				}
			if (curbrick != null && curbrick.posnodes != null)
				foreach (Node item in curbrick.posnodes)
				{
					g.FillRectangle(Brushes.LightBlue, 24 * item.x + 4, 24 * (rows - 1 - item.y) + 4, 20, 20);
				}
		}
		/// <summary>
		/// 很重要，重写键盘响应，否则方向键无法控制方块
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
			{
				return false;
			}
			return base.ProcessDialogKey(keyData);
		}
		/// <summary>
		/// 上方向键旋转，左方向键左移，右方向键右移，下方向键加速下移
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			Keys key = e.KeyCode;
			if (curbrick != null && curbrick.posnodes != null && timer1.Enabled == true && aichoose == 0)
			{
				if (key == Keys.Up)
				{
					curbrick.Transform();
					pictureBox1.Refresh();
				}
				if (key == Keys.Left)
				{
					curbrick.Leftmove();
					pictureBox1.Refresh();
				}
				if (key == Keys.Right)
				{
					curbrick.Rightmove();
					pictureBox1.Refresh();
				}
				if (key == Keys.Down)
				{
					curbrick.Dropmove();
					pictureBox1.Refresh();
				}
			}
		}
		/// <summary>
		/// 选择三号AI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 3;
			textBox1.Text = "切换：三号AI，将不响应用户方向键";
		}
		/// <summary>
		/// 清空输出
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, EventArgs e)
		{
			textBox1.Text = "";
			textBox2.Text = "";
		}
		/// <summary>
		/// 选择四号AI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 4;
			checkBox1.Enabled = !checkBox1.Enabled;
			textBox1.Text = "切换：四号AI，将不响应用户方向键，可进入训练模式";
			trainumber = 0;
		}
		/// <summary>
		/// 是否训练AI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			aiistraining = !aiistraining;
			trainumber = 0;
			groupBox2.Enabled = checkBox1.Checked;
		}
		/// <summary>
		/// 设置训练值
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button5_Click(object sender, EventArgs e)
		{
			batch = int.Parse(textBox3.Text);
			rate = (double)decimal.Parse(textBox4.Text);
			textBox1.Text += Environment.NewLine + "设置成功!" + "设置batch:" + batch + "设置rate:" + rate;
		}
		/// <summary>
		/// 储存数据到xml
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button6_Click(object sender, EventArgs e)
		{
			WriteXml();
		}
		/// <summary>
		/// 从xml还原数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button7_Click(object sender, EventArgs e)
		{
			ReadXml();
		}
	}
}
