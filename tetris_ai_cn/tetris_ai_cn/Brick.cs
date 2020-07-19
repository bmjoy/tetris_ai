using System;
using System.Collections.Generic;
namespace tetris_ai_cn
{
    public class Brick
    {
        /// <summary>
        /// 说明板块类型，0田字形,1|字形,2T字形,3Z字形,4S字形,5J字形,6L字形
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
            x = Form1.columns / 2 - 1,
            y = Form1.rows + 1
        };
        /// <summary>
        /// 新建板块
        /// </summary>
        public Brick()
        {
            Random random = new Random();
            int index = random.Next(0, 49) / 7;
            //int index = 1;
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
        /// 指定类型新建板块
        /// </summary>
        /// <param name="index">index整数，0~7分别代指一种板块类型，具体看type注释</param>
        public Brick(int index)
        {
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
        /// 仅改变typenode来逆时针旋转，不考虑越界
        /// </summary>
        public void Rotate()
        {
            List<Node> new_typenodes = new List<Node>();
            foreach (Node item in typenodes)
                new_typenodes.Add(item.Trans());
            typenodes = new_typenodes;
        }
        /// <summary>
        /// 仅改变typenode来逆时针旋转，不考虑越界
        /// </summary>
        /// <param name="time">旋转次数</param>
        public void Rotate(int time)
        {
            for (int i = 0; i < time; i++)
                Rotate();
        }
        /// <summary>
        /// 使板块逆时针旋转90度，只忽略背景图矩阵上界
        /// </summary>
        /// <returns>返回false表示旋转失败</returns>
        public bool Transform()
        {
            List<Node> new_posnodes = new List<Node>();
            List<Node> new_typenodes = new List<Node>();
            foreach (Node item in typenodes)
            {
                new_item = item.Trans() + pos;
                if (new_item.y <= Form1.rows - 1)
                {
                    if (new_item.x > Form1.columns - 1 || new_item.x < 0 || new_item.y < 0 || Form1.arr[new_item.x, new_item.y] == 1) return false;
                    new_posnodes.Add(new_item);
                }
                new_typenodes.Add(item.Trans());
            }
            posnodes = new_posnodes;
            typenodes = new_typenodes;
            eswn = (eswn + 1) % 4;
            return true;
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
                //三边满足
                if (new_item.x >= 0 && new_item.x < Form1.columns && new_item.y >= 0)
                {
                    //上越界
                    if (new_item.y > Form1.rows - 1) continue;
                    //四边满足有重合
                    else if (Form1.arr[new_item.x, new_item.y] == 1) return false;
                    //四边满足无重合
                    else new_posnodes.Add(new_item);
                }
                else return false;
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
        /// <summary>
        /// 模式匹配数组
        /// </summary>
        public static int[,] MatchPattern = new int[,] {
        { 2,0,0,0,0},{ 2,0,0,0,0},{ 2,0,0,0,0},{ 2,0,0,0,0},//田字
        { 4,0,0,0,0},{ 1,0,0,0,0},{ 4,0,0,0,0},{ 1,0,0,0,0},//一字
        { 3,0,-1,0,0},{ 2,0, 1,0,0 },{ 3,0,0,0,0 },{ 2,0,-1,0,0  },//T字
        { 2,0,1,0,0 },{ 3,0,-1,-1,0},{ 2,0,1,0,0 },{ 3,0,-1,-1,0},//Z字，2改成3增强不死性（概率不均时）
        { 2,0,-1,0,0},{ 3,0,0,1,0  },{ 2,0,-1,0,0},{ 3,0,0,1,0  },//S字，2改成3增强不死性（概率不均时）
        { 2,0,0,0,0 },{ 3,0,0,-1,0 },{ 2,0,2,0,0 },{ 3,0,0,0,0  },//J字
        { 2,0,0,0,0 },{ 3,0,0,0,0  },{ 2,0,-2,0,0},{ 2,0,1,1,0  },//L字
        };
        /// <summary>
        /// 标记砖块的旋转状态
        /// </summary>
        public int eswn = 0;
    }
}
