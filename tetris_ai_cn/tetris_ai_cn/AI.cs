using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tetris_ai_cn
{
	class AI
	{

		/// <summary>
		/// 预定义的int型二维矩阵
		/// </summary>
		private static int[,] arr2;

		/// <summary>
		/// 一号AI的控制函数
		/// </summary>
		public static void AI_1()
		{

		}

		/// <summary>
		/// 深复制背景图矩阵
		/// </summary>
		private static void Copyarr()
		{
			arr2 = new int[Form1.columns, Form1.rows];
			for (int i = 0; i < Form1.columns; i++)
				for (int j = 0; j < Form1.rows; j++)
					arr2[i, j] = Form1.arr[i, j];
		}

		/// <summary>
		/// 深复制背景图矩阵
		/// </summary>
		/// <param name="arr">一个二维int型数组</param>
		/// <returns>一个二维int型数组</returns>
		private static int[,] Copyarr(int[,] arr)
		{
			int rows = arr.Rank;
			int columns = arr.GetLength(0);
			int[,] arr2 = new int[columns, rows];
			for (int i = 0; i < columns; i++)
				for (int j = 0; j < rows; j++)
					arr2[i, j] = arr[i, j];
			return arr2;
		}
	}
}
