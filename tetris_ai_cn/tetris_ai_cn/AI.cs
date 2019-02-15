using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace tetris_ai_cn
{
	partial class Form1
	{
		/// <summary>
		/// 给AI训练用的
		/// </summary>
		double[,] target = new double[2, 1];
		/// <summary>
		/// 预定义的int型二维矩阵
		/// </summary>
		private double[,] arr2;
		/// <summary>
		/// 预定义的int型二维矩阵
		/// </summary>
		private double[,] arr3;
		/// <summary>
		/// 模拟variable
		/// </summary>
		double[,] w = new double[2, 200];
		double[,] w0 = new double[2, 1];
		/// <summary>
		/// 模拟bp反向传播
		/// </summary>
		double[,] dw = new double[2, 200];
		double[,] dw0 = new double[2, 1];
		/// <summary>
		/// 模拟偏置量
		/// </summary>
		double[,] b = new double[2, 1];
		/// <summary>
		/// 模拟输出
		/// </summary>
		double[,] y = new double[2, 1];
		/// <summary>
		/// 损失
		/// </summary>
		double[,] loss = new double[2, 1];
		/// <summary>
		/// 学习步长
		/// </summary>
		double rate = 1;
		/// <summary>
		/// 训练次数
		/// </summary>
		private int trainumber = 0;
		/// <summary>
		/// 训练一批数
		/// </summary>
		private int batch = 100;
		/// <summary>
		/// 一号AI的控制函数
		/// </summary>
		private void AI_1()
		{
			//自己写(｀・ω・´)，如果项目80 stars了给出我的代码
		}
		/// <summary>
		/// 二号AI的控制函数
		/// </summary>
		private void AI_2()
		{
			//自己写(｀・ω・´)，如果项目80 stars了给出我的代码
		}
		/// <summary>
		/// 三号AI的控制函数
		/// </summary>
		private void AI_3()
		{
			//自己写(｀・ω・´)，如果项目80 stars了给出我的代码
		}
		/// <summary>
		/// 四号AI的控制函数
		/// </summary>
		private void AI_4()
		{
			//自己写(｀・ω・´)，如果项目100 stars了给出我的代码
		}
		/// <summary>
		/// 深复制背景图矩阵arr2
		/// </summary>
		private void Copyarr()
		{
			arr2 = new double[columns, rows];
			for (int i = 0; i < columns; i++)
				for (int j = 0; j < rows; j++)
				{
					arr2[i, j] = arr[i, j];
				}
		}
		/// <summary>
		/// 深复制背景图矩阵arr2和arr3
		/// </summary>
		private void Copyarr2()
		{
			arr2 = new double[columns, rows];
			arr3 = new double[columns, rows];
			for (int i = 0; i < columns; i++)
				for (int j = 0; j < rows; j++)
				{
					arr2[i, j] = arr[i, j];
					arr3[i, j] = arr[i, j];
				}
		}
		/// <summary>
		/// 将int型二维数组增加一圈
		/// </summary>
		/// <param name="arr2">一个int型二维数组</param>
		/// <returns>一个扩大的int型二维数组</returns>
		private double[,] Expandarr(double[,] arr2)
		{
			double[,] t_arr = new double[columns + 2, rows + 2];
			for (int i = 1; i <= columns; i++)
				for (int j = 1; j <= rows; j++)
					t_arr[i, j] = arr2[i - 1, j - 1];
			for (int i = 0; i < columns + 2; i++)
			{
				t_arr[i, 0] = 1; t_arr[i, rows + 1] = 1;
			}
			for (int j = 0; j < rows + 2; j++)
			{
				t_arr[0, j] = 1; t_arr[columns + 1, j] = 1;
			}
			return t_arr;
		}
		/// <summary>
		/// 把二维数组拍扁成“一维”
		/// </summary>
		/// <param name="arr2">二维数组</param>
		/// <returns>"一维"数组</returns>
		private double[,] Plain(double[,] arr2)
		{
			double[,] new_arr = new double[columns * rows, 1];
			for (int cl = 0; cl < columns; cl++)
				for (int r = 0; r < rows; r++)
					new_arr[cl * rows + r, 0] = arr2[cl, r];
			return new_arr;
		}
		/// <summary>
		/// 简单激活函数
		/// </summary>
		/// <param name="y">double数据</param>
		/// <param name="clow">下限</param>
		/// <param name="chigh">上限</param>
		/// <returns>处理过的数据</returns>
		private double Activionfun(double y, double clow, double chigh)
		{
			double high = chigh;
			double low = clow;
			if (y > high) y = high;
			else if (y < low) y = low;
			return y;
		}
		/// <summary>
		/// 数背景图矩阵有几个1
		/// </summary>
		/// <param name="arr2">一个double型矩阵</param>
		/// <returns>1的数量</returns>
		private int Count1(double[,] arr2)
		{
			int counter = 0;
			int ca = arr2.GetLength(0);
			int ra = arr2.GetLength(1);
			for (int i = 0; i < ca; i++)
				for (int j = 0; j < ra; j++)
					if (arr2[i, j] == 1) counter++;
			return counter;
		}
		/// <summary>
		/// 储存数据到xml
		/// </summary>
		public void WriteXml(string str = "")
		{
			//创建一个数据集，将其写入xml文件
			//string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "model.xml";
			string filename = "\\model" + str + ".xml";
			System.Data.DataSet ds = new System.Data.DataSet("MODEL");
			System.Data.DataTable table = new System.Data.DataTable("w");
			ds.Tables.Add(table);
			table.Columns.Add("c1", typeof(double));
			table.Columns.Add("c2", typeof(double));
			for (int i = 0; i < 200; i++)
			{
				System.Data.DataRow row = table.NewRow();
				for (int j = 0; j < 2; j++)
				{
					row[j] = w[j, i];
				}
				ds.Tables["w"].Rows.Add(row);
			}
			table = new System.Data.DataTable("w0");
			ds.Tables.Add(table);
			table.Columns.Add("c1", typeof(double));
			table.Columns.Add("c2", typeof(double));
			for (int i = 0; i < 1; i++)
			{
				System.Data.DataRow row = table.NewRow();
				for (int j = 0; j < 2; j++)
				{
					row[j] = w0[j, i];
				}
				ds.Tables["w0"].Rows.Add(row);
			}
			table = new System.Data.DataTable("b");
			ds.Tables.Add(table);
			table.Columns.Add("c1", typeof(double));
			table.Columns.Add("c2", typeof(double));
			for (int i = 0; i < 1; i++)
			{
				System.Data.DataRow row = table.NewRow();
				for (int j = 0; j < 2; j++)
				{
					row[j] = b[j, i];
				}
				ds.Tables["b"].Rows.Add(row);
			}
			table = new System.Data.DataTable("checkpoint");
			ds.Tables.Add(table);
			table.Columns.Add("trainumber", typeof(int));
			table.Columns.Add("batch", typeof(int));
			table.Columns.Add("rate", typeof(double));
			System.Data.DataRow row1 = table.NewRow();
			row1[0] = trainumber;
			row1[1] = batch;
			row1[2] = rate;
			ds.Tables["checkpoint"].Rows.Add(row1);
			string pathbase = Environment.CurrentDirectory;
			string path = pathbase + filename;
			ds.WriteXml(path);
			textBox1.Text = "写入数据成功！数据见" + path;
		}
		/// <summary>
		/// 从xml还原数据
		/// </summary>
		private void ReadXml()
		{
			//将xml文件的数据放到数据集里，再将数据集绑定到所需的控件即可 
			System.Data.DataSet ds = new System.Data.DataSet();
			string pathbase = Environment.CurrentDirectory;
			string path = pathbase + "\\model.xml";
			try
			{
				ds.ReadXml(path);
			}
			catch (System.IO.FileNotFoundException)
			{
				MessageBox.Show("找不到model.xml");
				return;
			}
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 200; i++)
				{
					w[j, i] = double.Parse(ds.Tables["w"].Rows[i][j].ToString());
				}
			}
			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 1; i++)
				{
					w0[j, i] = double.Parse(ds.Tables["w0"].Rows[i][j].ToString());
					b[j, i] = double.Parse(ds.Tables["b"].Rows[i][j].ToString());
				}
			}
			trainumber = int.Parse(ds.Tables["checkpoint"].Rows[0][0].ToString());
			batch = int.Parse(ds.Tables["checkpoint"].Rows[0][1].ToString());
			rate = double.Parse(ds.Tables["checkpoint"].Rows[0][2].ToString());
			textBox1.Text = "读取数据成功！" + "累计训练数" + trainumber + "最后批量" + batch + "最后学习步长" + rate;
		}
	}
}
