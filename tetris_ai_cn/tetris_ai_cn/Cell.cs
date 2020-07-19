using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tetris_ai_cn
{
	class Cell
	{
		public List<Cell> lastcells = null;
		public List<Cell> nextcells = null;
		public static double weight = 1;
		public static double bias = 0;
		public int actId = 0;
		public double output = 0;
		/// <summary>
		/// 使用WANN进化函数
		/// </summary>
		/// <returns></returns>
		public void Act(double x)
		{
			/* Assume x is a float (not vector)
				case 1  -- Linear
				case 2  -- Unsigned Step Function
				case 3  -- Sin
				case 4  -- Gausian with mean 0 and sigma 1
				case 5  -- Hyperbolic Tangent (signed)
				case 6  -- Sigmoid unsigned [1 / (1 + exp(-x))]
				case 7  -- Inverse
				case 8  -- Absolute Value
				case 9  -- Relu
				case 10 -- Cosine

				x is a numjs NDArrayß
			*/
			double result;
			switch (actId)
			{
				case 1: // Linear
					result = x;
					break;
				case 2: // Unsigned Step Function
					result = 0.0;
					if (x > 0.0)
					{
						result = 1.0;
					}
					break;
				case 3: // Sine
					result = Math.Sin(Math.PI * x);
					break;
				case 4: // Gaussian with mean zero and unit variance
						// value = np.exp(-np.multiply(x, x) / 2.0)
					result = Math.Exp(-(x * x) / 2.0);
					break;
				case 5: // Hyperbolic Tangent
					result = Math.Tanh(x);
					break;
				case 6: // Sigmoid
					result = (Math.Tanh(x / 2.0) + 1.0) / 2.0;
					break;
				case 7: // Inverse
					result = -x;
					break;
				case 8: // Absolute Value
					result = Math.Abs(x);
					break;
				case 9: // ReLU
					result = Math.Max(x, 0);
					break;
				case 10: // Cosine
					result = Math.Cos(Math.PI * x);
					break;
				default:
					result = x;
					break;
			}
			output += result;
		}
		/// <summary>
		/// 传递给下面的神经元
		/// </summary>
		public void Run()
		{
			foreach (Cell cell in nextcells)
			{
				cell.Act(output);
			}
		}
	}
}
