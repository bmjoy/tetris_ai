﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace tetris_ai_cn
{
	public partial class Form1 : Form
	{
		//代码中左下角为原点，向右X正向上Y正。这和fillrect（左上角原点）不同，所以只在绘图时转换。
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
		public static int[,] arr;
		/// <summary>
		/// 板块对象
		/// </summary>
		Brick curbrick;
		/// <summary>
		/// 是否由AI控制
		/// </summary>
		int aichoose = 0;
		/// <summary>
		/// 当前分数
		/// </summary>
		int score = 0;
		/// <summary>
		/// 初始化所有控件
		/// </summary>
		public Form1()
		{
			arr = new int[columns, rows];
			InitializeComponent();
			pictureBox1.Size = new Size(24 * columns + 4, 24 * rows + 4);
			label2.Text = "当前分数：" + score + "分";
			timer1.Enabled = false;
			trackBar1.Enabled = !timer1.Enabled;
		}
		/// <summary>
		/// 用于AI控制
		/// </summary>
		/// <param name="rotatetime">俄罗斯方块逆时针旋转次数</param>
		/// <param name="xpos">俄罗斯方块最佳下落x坐标</param>
		public void Commander(int rotatetime, Node xpos)
		{
			while (!curbrick.Transform(rotatetime)) ;
			while (!curbrick.Canmove(xpos)) ;
		}
		/// <summary>
		/// AI控制的入口函数
		/// </summary>
		public void Aicontrol()
		{
			switch (aichoose)
			{
				case 0: return;
				case 1: AI.AI_1(); break;
				default: break;
			}
		}
		/// <summary>
		/// 新局
		/// </summary>
		private void button1_Click(object sender, EventArgs e)
		{
			timer1.Stop();
			trackBar1.Enabled = !timer1.Enabled;
			arr = new int[columns, rows];
			pictureBox1.Refresh();
			timer1.Start();
			trackBar1.Enabled = !timer1.Enabled;
		}
		/// <summary>
		/// 暂停
		/// </summary>
		private void button2_Click(object sender, EventArgs e)
		{
			timer1.Stop();
			trackBar1.Enabled = !timer1.Enabled;
		}
		/// <summary>
		///继续
		/// </summary>
		private void button3_Click(object sender, EventArgs e)
		{
			timer1.Start();
			trackBar1.Enabled = !timer1.Enabled;
		}
		/// <summary>
		/// 控制下落快慢
		/// </summary>
		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			timer1.Interval = 100 * trackBar1.Value + 1;
		}
		/// <summary>
		/// 无AI
		/// </summary>
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 0;
		}
		/// <summary>
		/// 选择一号AI
		/// </summary>
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 1;
		}
		/// <summary>
		/// 选择二号AI
		/// </summary>
		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			aichoose = 2;
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
		/// 定时自动下落
		/// </summary>
		private void timer1_Tick(object sender, EventArgs e)
		{
			List<int> countrows = new List<int>();
			if (curbrick == null) curbrick = new Brick();
			lock (curbrick)
			{
				for (int i = 0; i < columns; i++)
				{
					if (arr[i, rows - 1] == 1)
					{
						timer1.Stop();
						trackBar1.Enabled = !timer1.Enabled;
						MessageBox.Show("游戏结束！");
						return;
					}
				}
				if (curbrick.posnodes == null) curbrick.Canmove(curbrick.pos);
				if (!curbrick.Dropmove())
				{
					Fillarr();
					countrows = CountRow(arr);
					Cleanrows(countrows);
					curbrick = null;
				}
				pictureBox1.Refresh();
				label2.Text = "当前分数：" + score + "分";
			}
		}
		/// <summary>
		/// 清除已满的行，与CountRow配套使用
		/// </summary>
		/// <param name="countrows">List（int）类型，第一个参数是可消总行数，接下来项的则是可消行的y坐标，从大到小排列</param>
		private void Cleanrows(List<int> countrows)
		{
			countrows.RemoveAt(0);
			foreach (int item in countrows)
				for (int i = 0; i < columns; i++)
					for (int j = item; j < rows - 1; j++)
					{
						arr[i, j] = arr[i, j + 1];
					}
		}
		/// <summary>
		/// 用于把板块写入背景图arr中，只有板块不能下落时才可以调用
		/// </summary>
		private void Fillarr()
		{
			foreach (Node item in curbrick.posnodes)
			{
				arr[item.x, item.y] = 1;
			}
		}
		/// <summary>
		/// 对一个二维数组（矩阵）计算可消行数，与Cleanrows配套使用
		/// </summary>
		/// <param name="arr">一个二维数组</param>
		/// <returns>List（int）类型，第一个参数是可消总行数，接下来的项则是可消行的y坐标，从大到小排列</returns>
		public List<int> CountRow(int[,] arr)
		{
			List<int> countrows = new List<int>();
			countrows.Add(0);
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
					score++;
					countrows.Add(i);
				}
			}
			return countrows;
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
	}
}