using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
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
        /// 模拟variable
        /// </summary>
        double[,] wx = new double[10, 200];
        double[,] wr = new double[4, 200];
        double[,] w0x = new double[10, 1];
        double[,] w0r = new double[4, 1];
        /// <summary>
        /// 模拟bp反向传播
        /// </summary>
        double[,] dw = new double[2, 200];
        double[,] dw0 = new double[2, 1];
        /// <summary>
        /// 模拟bp反向传播
        /// </summary>
        double[,] dw2x = new double[10, 200];
        double[,] dw2r = new double[10, 1];
        double[,] dw0x = new double[4, 200];
        double[,] dw0r = new double[4, 1];
        /// <summary>
        /// 模拟偏置量
        /// </summary>
        double[,] b = new double[2, 1];
        /// <summary>
        /// 模拟偏置量
        /// </summary>
        double[] bx = new double[10];
        double[] br = new double[4];
        /// <summary>
        /// 模拟输出
        /// </summary>
        double[,] y = new double[2, 1];
        /// <summary>
        /// 模拟输出2
        /// </summary>
        double[] y2r = new double[4];
        double[] y2x = new double[10];
        /// <summary>
        /// 损失
        /// </summary>
        double[,] loss = new double[2, 1];
        double[,] loss2 = new double[2, 1];
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
        private int batch = 50;
        /// <summary>
        /// 一号AI的控制函数
        /// </summary>
        private void AI_1()
        {
            Copyarr2();
            Brick testbrick = new Brick(curbrick.type);
            double index1 = -4.500158825082766;
            double index2 = 3.4181268101392694;
            double index3 = -3.2178882868487753;
            double index4 = -9.348695305445199;
            double index5 = -7.899265427351652;
            double index6 = -3.3855972247263626;
            int[] BuiHeight = new int[columns];//第一个砖块，下面往上数
            int Rowtransitions = 0;//行变换
            int Holes = 0;//空洞数
            int Columntransitions = 0;//列变换
            int Wellsum = 0;//井
            double LandingHeight = 0;//落地高度
            int clearrows = 0;//消行数
            int contribution = 0;//贡献数
            List<int> countrows;
            double flag = Double.MinValue;
            double result = 0;
            int ai_rotate = 0;
            int ai_posx = testbrick.pos.x;
            int ai_posy = 0;
            for (int k = 0; k < 4; k++)
            {
                testbrick.pos.x = columns / 2 - 1;
                testbrick.pos.y = rows - 1;
                testbrick.Rotate();
                for (int i0 = 0; i0 < columns; i0++)
                {
                    ////result = SeekBest(arr4, testbrick);
                    Copyarr2();
                    int Bui = 0;
                    for (int j = 0; j < rows; j++)
                        if (arr2[i0, j] == 0)
                        {
                            Bui = j; break;
                        }
                    testbrick.pos.x = i0;
                    testbrick.pos.y = Bui;
                    //按列寻找碰撞点
                    while (!testbrick.Canmove(testbrick.pos) && testbrick.pos.y < rows)
                    {
                        testbrick.pos.y++;
                    }
                    if (testbrick.pos.y >= rows) continue;
                    LandingHeight = testbrick.pos.y + (testbrick.typenodes[0].y + testbrick.typenodes[1].y + testbrick.typenodes[2].y + testbrick.typenodes[3].y) / 4;
                    testbrick.Canmove(testbrick.pos);
                    Fillarr(arr2, testbrick.posnodes);
                    countrows = CountRow(arr2);
                    //消行数
                    clearrows = countrows[0];
                    countrows.RemoveAt(0);
                    //贡献数
                    foreach (int item in countrows)
                        for (int j = 0; j < columns; j++)
                        {
                            if (arr3[j, item] == 0) contribution++;
                        }
                    //新背景图矩阵
                    foreach (int item in countrows)
                        for (int i = 0; i < columns; i++)
                            for (int j = item; j < rows - 1; j++)
                            {
                                arr2[i, j] = arr2[i, j + 1];
                            }
                    //这里是xy坐标系楼高
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < rows; j++)
                            if (arr2[i, j] == 0)
                            {
                                BuiHeight[i] = j - 1; break;
                            }
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < BuiHeight[i] - clearrows; j++)
                        {
                            //洞数
                            if (arr2[i, j] == 0)
                            {
                                Holes++;
                            }
                        }
                    //行变换
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < columns; j++)
                        {
                            if (j > 0 && arr2[j, i] != arr2[j - 1, i])
                            {
                                Rowtransitions++;
                            }
                        }
                    //列变换
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < rows; j++)
                        {
                            if (j > 0 && arr2[i, j] != arr2[i, j - 1])
                            {
                                Columntransitions++;
                            }
                        }
                    //井数
                    int temp = 0;
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < rows; j++)
                        {
                            int count = 0;
                            if (j > BuiHeight[i] - clearrows && ((i > 0 && arr2[i - 1, j] == 0) || (i < columns - 1 && arr2[i + 1, j] == 0)))
                            {
                                temp = j;
                                continue;
                            }
                            if (j == rows - 1 && temp > 0)
                            {
                                Wellsum += (temp - BuiHeight[i] + clearrows - 1) * (temp - BuiHeight[i] + clearrows) / 2;
                            }
                            if (j < BuiHeight[i] - clearrows)
                            {
                                if (arr2[i, j] == 0) count++;
                                else
                                {
                                    Wellsum += count * (count + 1) / 2;
                                    count = 0;
                                }
                            }
                        }
                    //double index1 = -4.500158825082766;
                    //double index2 = 3.4181268101392694;
                    //double index3 = -3.2178882868487753;
                    //double index4 = -9.348695305445199;
                    //double index5 = -7.899265427351652;
                    //double index6 = -3.3855972247263626;
                    result = index1 * LandingHeight + index2 * clearrows * contribution + index3 * Rowtransitions + index4 * Columntransitions + index5 * Holes + index6 * Wellsum;
                    //Console.WriteLine(result);
                    if (result > flag)
                    {
                        flag = result;
                        ai_rotate = k + 1;
                        ai_posx = i0;
                        ai_posy = testbrick.pos.y;
                        //Console.WriteLine(now[0]);
                        //Console.WriteLine(now[1]);
                    }
                    LandingHeight = 0;
                    clearrows = 0;
                    contribution = 0;
                    Rowtransitions = 0;
                    Columntransitions = 0;
                    Holes = 0;
                    Wellsum = 0;
                }
            }
            curbrick.Rotate(ai_rotate);
            curbrick.pos.x = ai_posx;
            curbrick.pos.y = ai_posy;
            textBox1.Text = "旋转" + ai_rotate + "横坐标" + ai_posx + "纵坐标" + ai_posy;
        }
        /// <summary>
        /// 二号AI的控制函数
        /// </summary>
        private void AI_2()
        {
            Copyarr2();
            Brick testbrick = new Brick(curbrick.type);
            double index1 = -4.500158825082766;
            double index2 = 3.4181268101392694;
            double index3 = -3.2178882868487753;
            double index4 = -9.348695305445199;
            double index5 = -7.899265427351652;
            double index6 = -3.3855972247263626;
            int[] BuiHeight = new int[columns];//第一个砖块，下面往上数
            int Rowtransitions = 0;//行变换
            int Holes = 0;//空洞数
            int Columntransitions = 0;//列变换
            int Wellsum = 0;//井
            double LandingHeight = 0;//落地高度
            int clearrows = 0;//消行数
            int contribution = 0;//贡献数
            List<int> countrows;
            double flag = Double.MinValue;
            double result = 0;
            int ai_rotate = 0;
            int ai_posx = testbrick.pos.x;
            int ai_posy = 0;
            for (int k = 0; k < 4; k++)
            {
                testbrick.pos.x = columns / 2 - 1;
                testbrick.pos.y = rows - 1;
                testbrick.Rotate();
                for (int i0 = 0; i0 < columns; i0++)
                {
                    ////result = SeekBest(arr4, testbrick);					
                    int Bui = 0;
                    for (int j = 0; j < rows; j++)
                        if (arr2[i0, j] == 0)
                        {
                            Bui = j; break;
                        }
                    testbrick.pos.x = i0;
                    testbrick.pos.y = Bui;
                    //按列寻找碰撞点
                    while (!testbrick.Canmove(testbrick.pos) && testbrick.pos.y <= rows)
                    {
                        testbrick.pos.y++;
                    }
                    if (testbrick.pos.y > rows) continue;
                    LandingHeight = testbrick.pos.y + (testbrick.typenodes[0].y + testbrick.typenodes[1].y + testbrick.typenodes[2].y + testbrick.typenodes[3].y) / 4;
                    testbrick.Canmove(testbrick.pos);
                    Fillarr(arr2, testbrick.posnodes);
                    countrows = CountRow(arr2);
                    //消行数
                    clearrows = countrows[0];
                    countrows.RemoveAt(0);
                    //贡献数
                    foreach (int item in countrows)
                        for (int j = 0; j < columns; j++)
                        {
                            if (arr3[j, item] == 0) contribution++;
                        }
                    //新背景图矩阵
                    foreach (int item in countrows)
                        for (int i = 0; i < columns; i++)
                            for (int j = item; j < rows - 1; j++)
                            {
                                arr2[i, j] = arr2[i, j + 1];
                            }
                    //这里是xy坐标系楼高
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < rows; j++)
                            if (arr2[i, j] == 0)
                            {
                                BuiHeight[i] = j - 1; break;
                            }
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < BuiHeight[i] - clearrows; j++)
                        {
                            //洞数
                            if (arr2[i, j] == 0)
                            {
                                Holes++;
                            }
                        }
                    arr2 = Expandarr(arr2);
                    //行变换
                    for (int i = 0; i < rows + 2; i++)
                        for (int j = 0; j < columns + 2; j++)
                        {
                            if (j > 0 && arr2[j, i] != arr2[j - 1, i])
                            {
                                Rowtransitions++;
                            }
                        }
                    //列变换
                    for (int i = 0; i < columns + 2; i++)
                        for (int j = 0; j < rows + 2; j++)
                        {
                            if (j > 0 && arr2[i, j] != arr2[i, j - 1])
                            {
                                Columntransitions++;
                            }
                        }
                    //井数
                    int temp = 0;
                    for (int i = 1; i <= columns; i++)
                        for (int j = 1; j <= rows; j++)
                        {
                            if (arr2[i, j] == 0 && arr2[i - 1, j] == 1 && arr2[i + 1, j] == 1)
                            {
                                temp++;
                            }
                            else
                            {
                                Wellsum += temp * (temp + 1) / 2;
                                temp = 0;
                            }
                        }
                    //double index1 = -4.500158825082766;
                    //double index2 = 3.4181268101392694;
                    //double index3 = -3.2178882868487753;
                    //double index4 = -9.348695305445199;
                    //double index5 = -7.899265427351652;
                    //double index6 = -3.3855972247263626;
                    result = index1 * LandingHeight + index2 * clearrows * contribution + index3 * Rowtransitions + index4 * Columntransitions + index5 * Holes + index6 * Wellsum;
                    //Console.WriteLine(result);
                    if (result > flag)
                    {
                        flag = result;
                        ai_rotate = k + 1;
                        ai_posx = i0;
                        ai_posy = testbrick.pos.y;
                        //Console.WriteLine(now[0]);
                        //Console.WriteLine(now[1]);
                    }
                    LandingHeight = 0;
                    clearrows = 0;
                    contribution = 0;
                    Rowtransitions = 0;
                    Columntransitions = 0;
                    Holes = 0;
                    Wellsum = 0;
                    Copyarr();
                }
            }
            curbrick.Rotate(ai_rotate);
            curbrick.pos.x = ai_posx;
            curbrick.pos.y = ai_posy;
            textBox1.Text = "旋转" + ai_rotate + "横坐标" + ai_posx + "纵坐标" + ai_posy;
        }
        /// <summary>
        /// 三号AI的控制函数
        /// </summary>
        private void AI_3()
        {
            Copyarr2();
            Brick testbrick = new Brick(curbrick.type);
            double index1 = -4.500158825082766;
            double index2 = 3.4181268101392694;
            double index3 = -3.2178882868487753;
            double index4 = -9.348695305445199;
            double index5 = -7.899265427351652;
            double index6 = -3.3855972247263626;
            int[] BuiHeight = new int[columns];//第一个砖块，下面往上数
            int Rowtransitions = 0;//行变换
            int Holes = 0;//空洞数
            int Columntransitions = 0;//列变换
            int Wellsum = 0;//井
            double LandingHeight = 0;//落地高度
            int clearrows = 0;//消行数
            int contribution = 0;//贡献数
            List<int> countrows;
            double flag = Double.MinValue;
            double result = 0;
            int ai_rotate = 0;
            int ai_posx = testbrick.pos.x;
            int ai_posy = testbrick.pos.y;
            for (int k = 0; k < 4; k++)
            {
                testbrick.pos.x = columns / 2 - 1;
                testbrick.pos.y = rows - 1;
                testbrick.Rotate();
                for (int i0 = 0; i0 < columns; i0++)
                {
                    //int Bui = 0;
                    //for (int j = 0; j < rows; j++)
                    //	if (arr2[i0, j] == 0)
                    //	{
                    //		Bui = j; break;
                    //	}
                    testbrick.pos.x = i0;
                    testbrick.pos.y = rows - 1;
                    //按列寻找碰撞点
                    if (!testbrick.Canmove(testbrick.pos)) continue;
                    while (testbrick.Canmove(testbrick.pos))
                    {
                        testbrick.pos.y--;
                    }
                    if (testbrick.pos.y < 0) continue;
                    LandingHeight = testbrick.pos.y + (testbrick.typenodes[0].y + testbrick.typenodes[1].y + testbrick.typenodes[2].y + testbrick.typenodes[3].y) / 4;
                    testbrick.Canmove(testbrick.pos);
                    Fillarr(arr2, testbrick.posnodes);
                    countrows = CountRow(arr2);
                    //消行数
                    clearrows = countrows[0];
                    countrows.RemoveAt(0);
                    //贡献数
                    foreach (int item in countrows)
                        for (int j = 0; j < columns; j++)
                        {
                            if (arr3[j, item] == 0) contribution++;
                        }
                    //新背景图矩阵
                    foreach (int item in countrows)
                        for (int i = 0; i < columns; i++)
                            for (int j = item; j < rows - 1; j++)
                            {
                                arr2[i, j] = arr2[i, j + 1];
                            }
                    //这里是xy坐标系楼高
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < rows; j++)
                            if (arr2[i, j] == 0)
                            {
                                BuiHeight[i] = j - 1; break;
                            }
                    for (int i = 0; i < columns; i++)
                        for (int j = 0; j < BuiHeight[i] - clearrows; j++)
                        {
                            //洞数
                            if (arr2[i, j] == 0)
                            {
                                Holes++;
                            }
                        }
                    arr2 = Expandarr(arr2);
                    //行变换
                    for (int i = 0; i < rows + 2; i++)
                        for (int j = 0; j < columns + 2; j++)
                        {
                            if (j > 0 && arr2[j, i] != arr2[j - 1, i])
                            {
                                Rowtransitions++;
                            }
                        }
                    //列变换
                    for (int i = 0; i < columns + 2; i++)
                        for (int j = 0; j < rows + 2; j++)
                        {
                            if (j > 0 && arr2[i, j] != arr2[i, j - 1])
                            {
                                Columntransitions++;
                            }
                        }
                    //井数
                    int temp = 0;
                    for (int i = 1; i <= columns; i++)
                        for (int j = 1; j <= rows; j++)
                        {
                            if (arr2[i, j] == 0 && arr2[i - 1, j] == 1 && arr2[i + 1, j] == 1)
                            {
                                temp++;
                            }
                            else
                            {
                                Wellsum += temp * (temp + 1) / 2;
                                temp = 0;
                            }
                        }
                    //double index1 = -4.500158825082766;
                    //double index2 = 3.4181268101392694;
                    //double index3 = -3.2178882868487753;
                    //double index4 = -9.348695305445199;
                    //double index5 = -7.899265427351652;
                    //double index6 = -3.3855972247263626;
                    result = index1 * LandingHeight + index2 * clearrows * contribution + index3 * Rowtransitions + index4 * Columntransitions + index5 * Holes + index6 * Wellsum;
                    //Console.WriteLine(result);
                    if (result > flag)
                    {
                        flag = result;
                        ai_rotate = k + 1;
                        ai_posx = i0;
                        ai_posy = testbrick.pos.y;
                        //Console.WriteLine(now[0]);
                        //Console.WriteLine(now[1]);
                    }
                    LandingHeight = 0;
                    clearrows = 0;
                    contribution = 0;
                    Rowtransitions = 0;
                    Columntransitions = 0;
                    Holes = 0;
                    Wellsum = 0;
                    Copyarr();
                }
            }
            if (!(!aiistraining && aichoose == 4))
            {
                curbrick.Rotate(ai_rotate);
                curbrick.pos.x = ai_posx;
            }
            if (aiistraining || aichoose == 3) textBox1.Text = "旋转" + ai_rotate + "横坐标" + ai_posx;
            target[0, 0] = ai_rotate;
            target[1, 0] = ai_posx;
        }
        /// <summary>
        /// 四号AI的控制函数
        /// </summary>
        private void AI_4()
        {
            //重置结果
            y[0, 0] = 0;
            y[1, 0] = 0;
            //通过AI3算出期望下落
            AI_3();
            Copyarr();
            int[] BuiHeight = new int[columns];//第一个砖块，下面往上数
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                    if (arr2[i, j] == 0)
                    {
                        BuiHeight[i] = j - 1; break;
                    }
            //模拟placeholder
            double[,] x = Plain(arr2);
            int all1 = Count1(x);
            double[,] x0 = new double[1, 1];
            x0[0, 0] = curbrick.type;
            //计入背景图全连接
            for (int cl = 0; cl < 2; cl++)
                for (int r = 0; r < 1; r++)
                    for (int c = 0; c < 200; c++)
                        y[cl, r] += w[cl, c] * x[c, r];
            //计入type影响
            for (int cl = 0; cl < 2; cl++)
                for (int r = 0; r < 1; r++)
                    for (int c = 0; c < 1; c++)
                        y[cl, r] += w0[cl, c] * x0[c, 0];
            //计入偏置量影响
            for (int cl = 0; cl < 2; cl++)
                for (int r = 0; r < 1; r++)
                    y[cl, r] += b[cl, r];
            //激活函数
            y[0, 0] = (int)Activionfun(y[0, 0], 1, 4, 1);
            y[1, 0] = (int)Activionfun(y[1, 0], 0, 9, 1);

            //计算损失
            for (int cl = 0; cl < 2; cl++)
                loss[cl, 0] = target[cl, 0] - y[cl, 0];

           

            //训练模式不控制游戏，优化参数
            if (aiistraining)
            { 
                textBox2.Text = "训练次数" + trainumber + "旋转" + y[0, 0] + "横坐标" + y[1, 0] + Environment.NewLine;
                textBox2.Text += "总方格数" + all1 + "旋转损失" + loss[0, 0] + "横坐标损失" + loss[1, 0] + Environment.NewLine;
                ////textBox2.Text = "训练次数" + trainumber + "旋转" + y[0, 0] + "横坐标" + y[1, 0] + Environment.NewLine;
                //textBox2.Text += loss[0, 0] + "," + loss[1, 0] + Environment.NewLine;
                // 达到batch修改偏置量

                if (trainumber % batch == 0 || all1 == 0)
                {
                    for (int cl = 0; cl < 2; cl++)
                        b[cl, 0] += loss[cl, 0];
                }
                else
                {
                    //补偿量
                    for (int cl = 0; cl < 2; cl++)
                        for (int r = 0; r < 200; r++)
                            dw[cl, r] = loss[cl, 0] * 1 / all1 * x[r, 0] * rate;

                    ////迁移学习
                    //double[,] dww = new double[2, 10];
                    //for (int cl = 0; cl < 2; cl++)
                    //	for (int r = 0; r < 10; r++)
                    //		for (int i = 0; i < 20; i++)
                    //			dww[cl, r] += w[cl, r * 20 + i];

                    //for (int cl = 0; cl < 2; cl++)
                    //	for (int r = 0; r < 10; r++)
                    //		if (BuiHeight[r] != 0) dww[cl, r] /= BuiHeight[r];

                    //for (int cl = 0; cl < 2; cl++)
                    //	for (int r = 0; r < 10; r++)
                    //		for (int i = 0; i < 20; i++)
                    //			if (w[cl, r * 20 + i] == 0) dw[cl, r * 20 + i] += dww[cl, r];

                    //补偿量
                    for (int cl = 0; cl < 2; cl++)
                        dw0[cl, 0] = loss[cl, 0] * 1 / all1 * x0[0, 0] * rate;
                    //反向传播
                    for (int cl = 0; cl < 2; cl++)
                        for (int r = 0; r < 200; r++)
                            w[cl, r] += dw[cl, r];
                    //反向传播
                    for (int cl = 0; cl < 2; cl++)
                        w0[cl, 0] += dw0[cl, 0];
                    //迁移学习
                    if (trainumber % 5 == 0)
                        for (int cl = 0; cl < 2; cl++)
                            for (int r = 0; r < 10; r++)
                                for (int i = 19; i > 0; i--)
                                    w[cl, r * 20 + i] = w[cl, r * 20 + i - 1];
                }
                if (trainumber % 5 == 0 && trainumber < 600)
                {
                    WriteXml(trainumber.ToString());
                }
            }
            //测试模式控制板块
            else
            {
                Brick test = new Brick(curbrick.type);
                test.Rotate((int)y[0, 0]);
                test.pos.x = (int)y[1, 0];
                for (int i = 0; i < 9; i++)
                {
                    if (test.Canmove(test.pos))
                    {
                        curbrick.Rotate((int)y[0, 0]);
                        curbrick.pos.x = test.pos.x;
                        return;
                    }
                    else
                    {
                        test.pos.x--;
                    }
                }
                test.pos.x = (int)y[1, 0];
                for (int i = 0; i < 9; i++)
                {
                    if (test.Canmove(test.pos))
                    {
                        curbrick.Rotate((int)y[0, 0]);
                        curbrick.pos.x = test.pos.x;
                        return;
                    }
                    else
                    {
                        test.pos.x++;
                    }
                }
            }
            if (aiistraining) trainumber++;
            //if (trainumber != 0 && trainumber % 5 == 0) WriteXml(trainumber.ToString());
            //WriteXml(trainumber.ToString());
        }
        /// <summary>
        /// 五号AI的控制函数
        /// </summary>
        private void AI_5()
        {
            Copyarr();
            int[] Height = new int[columns];
            int[] Height2 = new int[columns];
            //这里计算每一列的高度
            for (int i = 0; i < columns; i++)
            {
                Height[i] = 0;
                for (int j = rows - 1; j >= 0; j--)
                {
                    if (arr2[i, j] == 1)
                    {
                        Height[i] = j + 1;
                        break;
                    }

                }
            }
            textBox1.Text = "高度分布：" + Height[0] + " " + Height[1] + " " + Height[2] + " "
                + Height[3] + " " + Height[4] + " " + Height[5] + " " + Height[6] + " " + Height[7]
                + " " + Height[8] + " " + Height[9] + Environment.NewLine;
            //开始模式匹配
            int flagX = 0;
            int flagE = 0;
            int flagH = 999;
            bool match = true;
            for (int i0 = 0; i0 < 4; i0++)
            {
                int len = Brick.MatchPattern[4 * curbrick.type + i0, 0];
                for (int i = 0; i < columns; i++)
                {
                    match = true;
                    for (int i2 = 0; i2 < columns; i2++)
                    {
                        Height2[i2] = Height[i2] - Height[i];
                    }
                    //                    textBox2.Text += "i0=" + i0 + "i=" + i + " Height2高度：" + Height2[0] + " " + Height2[1] + " " + Height2[2] + " "
                    //+ Height2[3] + " " + Height2[4] + " " + Height2[5] + " " + Height2[6] + " " + Height2[7]
                    //+ " " + Height2[8] + " " + Height2[9] + Environment.NewLine;
                    for (int i2 = 0; i2 < len; i2++)
                    {
                        if (i + len <= 10)
                        {
                            //textBox2.Text += i + " " + i2 + " 左 " + Height2[i + i2] + "右 " + Brick.MatchPattern[4 * curbrick.type + i0, 1 + i2] + Environment.NewLine;
                            if (Height2[i + i2] != Brick.MatchPattern[4 * curbrick.type + i0, 1 + i2])
                            {
                                match = false;
                                break;
                            }
                        }
                        else
                            match = false;
                    }
                    if (match && Height[i] < flagH)
                    {
                        flagH = Height[i];
                        flagX = i;
                        flagE = i0;
                        //                        textBox2.Text += "i=" + i + " Height2高度：" + Height2[0] + " " + Height2[1] + " " + Height2[2] + " "
                        //+ Height2[3] + " " + Height2[4] + " " + Height2[5] + " " + Height2[6] + " " + Height2[7]
                        //+ " " + Height2[8] + " " + Height2[9] + Environment.NewLine;
                        //                        textBox2.Text += "目前的flag 高度：" + flagH + " 横坐标：" + flagX + " 旋转序号：" + flagE + Environment.NewLine;
                    }
                }
            }
            if (flagH == 999)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (Height[i] == Height.Min())
                    {
                        flagX = i;
                        break;
                    }
                }
            }
            //测试砖块
            y[0, 0] = flagE - curbrick.eswn;
            y[1, 0] = flagX;
            Brick test = new Brick(curbrick.type);
            test.Rotate((int)y[0, 0]);
            test.pos.x = (int)y[1, 0];
            int flagTypeNodeX = 999;
            foreach (Node item in test.typenodes)
            {
                if (item.x < flagTypeNodeX)
                    flagTypeNodeX = item.x;
            }
            test.pos.x = (int)y[1, 0] - flagTypeNodeX;
            textBox1.Text += "横坐标：" + test.pos.x + " 旋转序号：" + flagE + Environment.NewLine;
            for (int i = 0; i < 9; i++)
            {
                if (test.Canmove(test.pos))
                {
                    curbrick.Rotate((int)y[0, 0]);
                    curbrick.pos.x = test.pos.x;
                    return;
                }
                else
                {
                    test.pos.x--;
                }
            }
            test.pos.x = (int)y[1, 0];
            for (int i = 0; i < 9; i++)
            {
                if (test.Canmove(test.pos))
                {
                    curbrick.Rotate((int)y[0, 0]);
                    curbrick.pos.x = test.pos.x;
                    return;
                }
                else
                {
                    test.pos.x++;
                }
            }
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
        private double Activionfun(double y, double clow, double chigh, int type = 0)
        {
            if (type == 1)
            {
                double high = chigh;
                double low = clow;
                if (y > high) y = high;
                else if (y < low) y = low;
                return y;
            }
            else
            {
                double high = chigh;
                double low = clow;
                double multiple = high - low;
                double offset = low;
                y = multiple / (1 + Math.Pow(Math.E, y)) + offset;
                if (y > high) y = high;
                else if (y < low) y = low;
                return y;
            }
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
        ///// <summary>
        ///// 反构造激活函数
        ///// </summary>
        ///// <param name="y"></param>
        ///// <param name="clow"></param>
        ///// <param name="chigh"></param>
        ///// <returns></returns>
        //private double DiActive(double y, double clow, double chigh)
        //{
        //    double high = chigh;
        //    double low = clow;
        //    double multiple = high - low;
        //    double offset = low;
        //    return Math.Log((y - offset) / (multiple + y - offset));
        //}
    }
}
