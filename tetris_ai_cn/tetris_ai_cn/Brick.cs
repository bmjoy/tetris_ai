using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace tetris_ai_cn
{
	class Brick
	{
		/// <summary>
		/// 说明板块类型
		/// </summary>
		public int type = 0;
		/// <summary>
		/// 以稀疏矩阵的方式存储每个方块相对板块的位置
		/// </summary>
		public List<Node> typenodes;
		/// <summary>
		/// 以稀疏矩阵的方式存储每个方块对应背景矩阵arr的位置
		/// </summary>
		public List<Node> posnodes;
		/// <summary>
		/// 板块中心位置
		/// </summary>
		public Node pos = new Node
		{
			x = Form1.columns / 2,
			y = Form1.rows - 1
		};
		/// <summary>
		/// 新建板块
		/// </summary>
		public Brick()
		{
			Random random = new Random();
			int index = random.Next(0, 49) / 7;
			//int index = 0;
			typenodes = new List<Node>();
			Node node1, node2, node3, node4;
			switch (index)
			{
				case 0:
					//田字形
					type = 0;
					node1 = new Node() { x = 0, y = 0 };
					typenodes.Add(node1);
					node2 = new Node() { x = 1, y = 0 };
					typenodes.Add(node2);
					node3 = new Node() { x = 1, y = -1 };
					typenodes.Add(node3);
					node4 = new Node() { x = 0, y = -1 };
					typenodes.Add(node4);
					break;
				case 1:
					//|字形
					type = 1;
					node1 = new Node() { x = 2, y = 0 };
					typenodes.Add(node1);
					node2 = new Node() { x = 1, y = 0 };
					typenodes.Add(node2);
					node3 = new Node() { x = 0, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = -1, y = 0 };
					typenodes.Add(node4);
					break;
				case 2:
					//T字形
					type = 2;
					node1 = new Node() { x = -1, y = 0 };
					typenodes.Add(node1);
					node2 = new Node() { x = 1, y = 0 };
					typenodes.Add(node2);
					node3 = new Node() { x = 0, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = 0, y = -1 };
					typenodes.Add(node4);
					break;
				case 3:
					//z字形
					type = 3;
					node1 = new Node() { x = 0, y = 0 };
					typenodes.Add(node1);
					node2 = new Node() { x = 0, y = -1 };
					typenodes.Add(node2);
					node3 = new Node() { x = 1, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = 1, y = 1 };
					typenodes.Add(node4);
					break;
				case 4:
					//s字形
					type = 4;
					node1 = new Node() { x = 0, y = 0 };
					typenodes.Add(node1);
					node2 = new Node() { x = 0, y = -1 };
					typenodes.Add(node2);
					node3 = new Node() { x = -1, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = -1, y = 1 };
					typenodes.Add(node4);
					break;
				case 5:
					//J字形
					type = 5;
					node1 = new Node() { x = 0, y = 2 };
					typenodes.Add(node1);
					node2 = new Node() { x = 0, y = 1 };
					typenodes.Add(node2);
					node3 = new Node() { x = 0, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = -1, y = 0 };
					typenodes.Add(node4);
					break;
				case 6:
					//L字形
					type = 6;
					node1 = new Node() { x = 0, y = 2 };
					typenodes.Add(node1);
					node2 = new Node() { x = 0, y = 1 };
					typenodes.Add(node2);
					node3 = new Node() { x = 0, y = 0 };
					typenodes.Add(node3);
					node4 = new Node() { x = 1, y = 0 };
					typenodes.Add(node4);
					break;
			}
		}
		/// <summary>
		/// 使板块逆时针旋转90度
		/// </summary>
		/// <returns>返回false表示旋转失败</returns>
		public bool Transform()
		{
			List<Node> new_posnodes = new List<Node>();
			List<Node> new_typenodes = new List<Node>();
			foreach (Node item in typenodes)
			{
				new_item = item.Trans() + pos;
				if (new_item.y > Form1.rows - 1) continue;
				if (new_item.x > Form1.columns - 1 || new_item.x < 0 || new_item.y < 0 || Form1.arr[new_item.x, new_item.y] == 1) return false;
				new_posnodes.Add(new_item);
				new_typenodes.Add(item.Trans());
			}
			posnodes = new_posnodes;
			typenodes = new_typenodes;
			return true;
		}
		/// <summary>
		/// 使板块逆时针旋转90度
		/// </summary>
		/// <param name="rotatetime">旋转次数</param>
		/// <returns>返回false表示旋转失败</returns>
		public bool Transform(int rotatetime)
		{
			bool can = true;
			for (int i = 0; i < rotatetime && can == true; i++) can = Transform();
			return can;
		}
		/// <summary>
		/// 尝试左移，如能就左移
		/// </summary>
		public void Leftmove()
		{
			if (Canmove(lpos + pos)) pos += lpos;
		}
		/// <summary>
		/// 尝试右移，如能就右移
		/// </summary>
		public void Rightmove()
		{
			if (Canmove(rpos + pos)) pos += rpos;
		}
		/// <summary>
		/// 尝试下移，如能就下移
		/// </summary>
		/// <returns></returns>
		public bool Dropmove()
		{
			if (Canmove(dpos + pos)) { pos += dpos; return true; } else return false;
		}
		/// <summary>
		/// 判断能否移动到new_pos
		/// </summary>
		/// <param name="new_pos">Node类坐标</param>
		/// <returns>返回能否</returns>
		public bool Canmove(Node new_pos)
		{
			List<Node> new_posnodes = new List<Node>();
			foreach (Node item in typenodes)
			{
				Node new_item = new_pos + item;
				if (new_item.y > Form1.rows - 1) continue;
				if (new_item.x > Form1.columns - 1 || new_item.x < 0 || new_item.y < 0 || Form1.arr[new_item.x, new_item.y] == 1) return false;
				new_posnodes.Add(new_item);
			}
			posnodes = new_posnodes;
			return true;
		}
		/// <summary>
		/// 中间变量，请勿打扰
		/// </summary>
		private Node new_item;
		/// <summary>
		/// 预定义的左方偏移
		/// </summary>
		private static Node lpos = new Node
		{
			x = -1,
			y = 0
		};
		/// <summary>
		/// 预定义右方偏移
		/// </summary>
		private static Node rpos = new Node
		{
			x = 1,
			y = 0
		};
		/// <summary>
		/// 预定义向下偏移
		/// </summary>
		private static Node dpos = new Node
		{
			x = 0,
			y = -1
		};
	}
}
