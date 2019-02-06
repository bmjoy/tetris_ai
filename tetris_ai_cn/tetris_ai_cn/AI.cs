using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace tetris_ai_cn
{
	/// <summary>
	/// ！为了方便，AI单独一个界面，但是AI是Form1对象的一部分
	/// </summary>
	partial class Form1
	{
		//数据区
		/// <summary>
		/// 预定义的int型二维矩阵
		/// </summary>
		private int[,] arr2;
		/// <summary>
		/// 预定义的int型二维矩阵
		/// </summary>
		private int[,] arr3;
		//AI区
		/// <summary>
		/// 一号AI的控制函数
		/// </summary>
		private void AI_1()
		{
			//自己写(｀・ω・´)
		}
		/// <summary>
		/// 二号AI的控制函数
		/// </summary>
		private void AI_2()
		{
			//自己写(｀・ω・´)
		}
		/// <summary>
		/// 三号AI的控制函数
		/// </summary>
		private void AI_3()
		{
			//自己写(｀・ω・´)
		}
		//工具函数区
		/// <summary>
		/// 深复制背景图矩阵arr2
		/// </summary>
		private void Copyarr()
		{
			arr2 = new int[columns, rows];
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
			arr2 = new int[columns, rows];
			arr3 = new int[columns, rows];
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
		private int[,] Expandarr(int[,] arr2)
		{
			int[,] t_arr = new int[columns + 2, rows + 2];
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
	}
}
