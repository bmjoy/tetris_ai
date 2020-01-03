# 注意：非原创，仅在来自 http://yuncode.net/code/c_5c1476152418f82 云代码的基础上修改
# -*- coding: utf-8 -*-

from tkinter import *
import random
from tkinter.messagebox import askquestion

# 尚需改进，有待提高

# 各种方块的表示，与参考方块的相对位置
shapedic = {1: ((0, 0), (1, 0), (0, -1), (1, -1)),  # 正方形
            2: ((0, 0), (0, -1), (0, -2), (0, 1)),  # 长条
            3: ((0, 0), (0, -1), (1, 0), (1, 1)),  # 之字型
            4: ((0, 0), (0, -1), (-1, 0), (-1, 1)),  # 反之字型
            5: ((0, 0), (1, 0), (-1, 0), (-1, -1)),  # L型
            6: ((0, 0), (1, 0), (-1, 0), (1, -1)),  # 反L型
            7: ((0, 0), (1, 0), (-1, 0), (0, -1))  # T型
            }

# 旋转函数，顺时针旋转90度，相对于参考方块
change_dic = {(0, 0): (0, 0), (0, 1): (-1, 0), (-1, 0): (0, -1), (0, -1): (1, 0), (1, 0): (0, 1),
              (1, -1): (1, 1), (1, 1): (-1, 1), (-1, 1): (-1, -1), (-1, -1): (1, -1),
              (2, 0): (0, 2), (0, 2): (-2, 0), (-2, 0): (0, -2), (0, -2): (2, 0)}

# 随机颜色
color = 'lightblue'
'''
colorDict = {
    0: '#CCC0B4',
    1: '#EEE4DA',
    2: '#EDE0C8',
    3: '#F2B179',
    4: '#EC8D54',
    5: '#F67C5F',
    6: '#EA5937',
    7: '#804000',
    8: '#F1D04B',
    9: '#E4C02A',
    10: '#EE7600',
    11: '#D5A500',
    12: '#E4C02A',
    13: '#804000',
    14: '#EA5937',
    15: '#EE7600',
    16: '#776E65',
    17: '#776E65',
    18: '#FFFFFF',
    19: 'yellow',
    20: 'blue',
    21: 'lightblue',
    22: 'red'
}
'''

# 俄罗斯方块


class Game_Russia:
    def __init__(self):

        # 每个方块的大小
        self.width = 20

        # 方块数目，长和宽
        self.row = 28
        self.column = 19

        #        #初始化
        #        self.scores=0
        #        self.all_square={}#坐标系网格中个位置方块的存在性
        #        self.head_square=[]#参考方块绝对位置
        #        self.new_square=[]#移动方块相对位置
        #        self.direction=-1#方块初始方向
        #        #规定界限
        #        #i表示第i列，0在左边
        #        #j表示第j行，零在上面
        #        for j in range(-4,self.row):
        #            for i in range(self.column):
        #                self.all_square[i,j]=0
        #        #划界，开口向上
        #        for j in range(self.row+1):
        #            self.all_square[19,j]=1
        #            self.all_square[-1,j]=1
        #        for i in range(-1,self.column+1):
        #            self.all_square[i,28]=1

        """
        用来debug
        for j in range(self.row+1):
            for i in range(-1,self.column+1):
                print self.all_square[i,j],
            print
        """

        self.window = Tk()
        self.window.geometry()
        #self.window.maxsize(400, 610)
        self.window.minsize(400, 610)
        self.window.title(u"俄罗斯方块")
        self.ai = IntVar()  # 选择AI
        self.ai.set(1)
        self.index1 = -4.500158825082766
        self.index2 = 3.4181268101392694
        self.index3 = -3.2178882868487753
        self.index4 = -9.348695305445199
        self.index5 = -7.899265427351652
        self.index6 = -3.3855972247263626
        self.landingHeight = 0
        self.clearrows = 0
        self.contribution = 0
        self.rowtransitions = 0
        self.columntransitions = 0
        self.holes = 0
        self.wellsum = 0
        self.aichange = 0
        self.aimove = 0
        self.frame1 = Frame(self.window, bg="white",
                            relief=GROOVE, borderwidth=5)
        self.frame2 = Frame(self.window, bg="white", relief=RAISED, borderwidth=2, height=40,
                            width=570)
        self.canvas = Canvas(self.frame1, bg='white', width=400, height=570)
        self.score_label1 = Label(self.frame2, text="Score: 0", height=2)
        self.score_label2 = Label(
            self.frame2, text="SpeedControlBar:", height=2)
        self.speed = Scale(self.frame2, length=160, from_=0,
                           to=500, showvalue=200, orient=HORIZONTAL, resolution=20)
        self.speed.set(100)
        self.ai0 = Radiobutton(
            self.window, variable=self.ai, value=0, text="无AI")
        self.ai1 = Radiobutton(
            self.window, variable=self.ai, value=1, text="一号AI")
        self.ai2 = Radiobutton(
            self.window, variable=self.ai, value=2, text="二号AI")
        self.frame1.pack()
        self.frame2.pack(fill=BOTH)
        self.score_label1.pack(side=LEFT)
        self.score_label2.pack(side=LEFT)
        self.speed.pack(side=LEFT)
        self.ai0.pack(side=LEFT)
        self.ai1.pack(side=LEFT)
        self.ai2.pack(side=LEFT)
        self.canvas.pack(fill=BOTH)

        self.draw_wall()

        self.initial()

        self.get_new_square()

        self.draw_new_square()

        self.play()

        self.window.mainloop()

    "=== View Part ==="

    def draw_wall(self):
        '''
        画边界
        '''
        self.canvas.create_line(5, 5, 385, 5, fill='blue', width=1)
        self.canvas.create_line(385, 5, 385, 565, fill='blue', width=1)
        self.canvas.create_line(5, 5, 5, 565, fill='blue', width=1)
        self.canvas.create_line(5, 565, 385, 565, fill='blue', width=1)

    def draw_score(self):
        '''
        画得分
        '''
        self.get_score()
        self.score_label1.config(
            self.score_label1, text="Score: " + str(self.scores))

    def draw_square(self):
        '''
        画下面所有不动的方块
        '''
       # color = colorDict[random.randint(0, len(colorDict) - 1)]
        for j in range(self.row):
            self.canvas.delete("line" + str(j))
            for i in range(self.column):
                if self.all_square[i, j]:
                    self.canvas.create_rectangle(5 + i * self.width,
                                                 5 + j * self.width, 5 +
                                                 (i + 1) * self.width,
                                                 5 + (j + 1) * self.width, fill=color, tags="line" + str(j))

    def draw_new_square(self):
        '''
        画移动的方块
        '''
        self.canvas.delete("new")
        self.head_square[1] += 1
    #    color = colorDict[random.randint(0, len(colorDict) - 1)]
        for i in range(4):
            self.canvas.create_rectangle(5 + (self.head_square[0] + self.new_square[i][0]) * self.width,
                                         5 +
                                         (self.head_square[1] +
                                          self.new_square[i][1]) * self.width,
                                         5 +
                                         (self.head_square[0] +
                                          self.new_square[i][0] + 1) * self.width,
                                         5 + (self.head_square[1] + 1 + self.new_square[i][1]) * self.width, fill=color,
                                         tags="new")

    "=== Model Part ==="

    def initial(self):
        '''
        初始化
        '''
        self.scores = 0
        self.all_square = {}  # 坐标系网格中个位置方块的存在性
        self.head_square = []  # 参考方块绝对位置
        self.new_square = []  # 移动方块相对位置
        self.direction = -1  # 方块初始方向
        # 规定界限
        # i表示第i列，0在左边
        # j表示第j行，零在上面
        for j in range(-4, self.row):
            for i in range(self.column):
                self.all_square[i, j] = 0
        # 划界，开口向上,0~18,0~27
        for j in range(self.row + 1):
            self.all_square[19, j] = 1
            self.all_square[-1, j] = 1
        for i in range(-1, self.column + 1):
            self.all_square[i, 28] = 1

    def is_dead(self):
        '''
        判断死亡与否，最上方中间四个方块
        '''
        for i in {8, 9, 10, 11}:
            if self.all_square[i, 0]:
                return True
        else:
            return False

    def get_new_square(self):
        '''
        获得新的方块，初始位置均为（9,-2）
        '''

        self.new = random.randrange(1, 8)  # 随机方块
        # 主方块（参考方块）的位置
        self.direction = random.randrange(4)
        self.head_square = [9, -2]
        self.new_square = list(shapedic[self.new])
        for i in range(self.direction):
            self.change()

    def delete_one_line(self, j):
        '''
        得分后删除整行
        '''

        for t in range(j, 2, -1):
            for i in range(self.column):
                self.all_square[i, t] = self.all_square[i, t - 1]
        for i in range(self.column):
            self.all_square[i, 0] = 0

    def get_score(self):
        '''
        计算得分
        '''
        for j in range(self.row):
            for i in range(self.column):
                # 判断某行是否全满
                if not self.all_square[i, j]:
                    break
            else:
                self.scores += 10
                self.delete_one_line(j)

    def get_seated(self):
        '''
        移动方块停止
        '''
        self.all_square[tuple(self.head_square)] = 1
        for i in range(4):
            self.all_square[self.head_square[0] + self.new_square[i][0],
                            self.head_square[1] + self.new_square[i][1]] = 1

    def is_seated(self):
        '''
        方块是否到了最底端
        '''
        for i in range(4):
            # print('point',self.head_square[0],self.head_square[1])
            # if self.head_square[0] + self.new_square[i][0] < 0 or self.head_square[0] + self.new_square[i][0] > 19 or self.head_square[1] + self.new_square[i][1] + 1 < 0 or  self.head_square[1] + self.new_square[i][1] + 1 > 27:
            #     return False
            if self.all_square[self.head_square[0] + self.new_square[i][0],
                                 self.head_square[1] + self.new_square[i][1] + 1]:
                return True
        return False

    "=== Control Part ==="

    def change(self, step=1):
        '''
        改变方块朝向
        # 通过旋转改变，主方块不动
        '''
        for _ in range(step):
            if self.new > 1:
                for i in range(4):
                    if self.all_square[self.head_square[0] + change_dic[self.new_square[i]][0], self.head_square[1] +
                                       change_dic[self.new_square[i]][1]]:
                        return
                else:
                    for i in range(4):
                        self.new_square[i] = change_dic[self.new_square[i]]
            else:
                return

    def right_move(self, step=1):
        '''
        右移
        '''
        # 先判断是否可以移动
        for _ in range(step):
            for i in range(4):
                a = self.head_square[0] + self.new_square[i][0] - 1
                b = self.head_square[1] + self.new_square[i][1]
                if a > 19 or a < 0 or b < 0 or b > 28:
                    break
                if self.all_square[a, b]:
                    return
            self.head_square[0] -= 1

    def left_move(self, step=1):
        '''
        左移
        '''
        for _ in range(step):
            # print("step:",step,end=" ")
            for i in range(4):
                # print("i:",i,end=" ")
                a = self.head_square[0] + self.new_square[i][0] + 1
                b = self.head_square[1] + self.new_square[i][1]
                if a > 19 or a < 0 or b < 0 or b > 28:
                    break
                if self.all_square[a, b]:
                    return
            self.head_square[0] += 1

    def down_quicker(self):
        '''
        向下加速
        '''
        while (not self.is_seated()):
            self.draw_new_square()

            self.canvas.after(50)
            self.canvas.update()

    def move(self, event):
        '''
         # 方向键控制
        '''
        if event.keycode == 39:
            self.left_move()
        elif event.keycode == 38:
            self.change()
        elif event.keycode == 37:
            self.right_move()
        elif event.keycode == 40:
            self.down_quicker()
        else:
            pass

    def play(self):
        '''
        开始游戏
        '''
        if(self.ai.get() == 0):
            self.canvas.bind('<Key>', self.move)  # 修改此处
        self.canvas.focus_set()

        while (self.ai.get() == 0):
            if self.is_dead():
                self.gameover()
                break

            if self.is_seated():  # 修改此处

                self.get_seated()
                self.get_new_square()

                self.draw_score()
                self.draw_square()
                self.draw_new_square()

            else:  # 修改此处
                self.draw_new_square()

                self.canvas.after(self.speed.get())
                self.canvas.update()

        while (self.ai.get() != 0):
            if self.is_dead():
                self.gameover()
                break
            self.act(self.ai.get())

    def gameover(self):
        '''
        游戏结束
        '''
        if askquestion("LOSE", u"你输了!\n重新开始吗？") == 'yes':
            return self.restart()
        else:
            return self.window.destroy()

    def restart(self):
        '''
        重新开始
        '''
        self.initial()

        self.draw_square()

        self.get_new_square()

        self.draw_new_square()

        self.play()

    def act(self, num):
        '''
        AI控制
        '''
        global color
        color = 'white'
        result = -99999999
        fall = 0
        if(num == 1):
            for i in range(4):
                self.change()
                for j in range(19):
                    if(j < 9):
                        #print("j:",j,end=" ")
                        self.left_move(9-j)
                    elif(j > 9):
                        self.right_move(j-9)

                    while not self.is_seated():  # 修改此处
                        a = self.head_square[0] + self.new_square[i][0]
                        b = self.head_square[1] + self.new_square[i][1]+1
                        if a > 18 or a < 0 or b < 0 or b > 27:
                            break
                        self.draw_new_square()
                        fall += 1
                        self.canvas.after(1)
                    self.canvas.update()

                    self.get_seated()
                    self.sim_score()
                    self.rt()
                    self.ct()
                    self.ht()
                    self.wt()
                    self.landingHeight = self.head_square[1]-self.clearrows
                    result0 = self.index1 * self.landingHeight + self.index2 * self.clearrows * self.contribution + self.index3 * \
                        self.rowtransitions + self.index4 * self.columntransitions + \
                        self.index5 * self.holes + self.index6 * self.wellsum
                    # print('result0,', result0)
                    if result0 > result:
                        result = result0
                        self.aichange = i
                        self.aimove = j
                        print('result,', result, 'change,', i, 'move,', j)
                    self.get_unseated()
                    # print('fall',fall)
                    for _ in range(fall):
                        self.draw_old_square()
                    fall = 0
                    if(j < 9):
                        self.right_move(9-j)
                    elif(j > 9):
                        self.left_move(j-9)
                    self.landingHeight = 0
                    self.clearrows = 0
                    self.contribution = 0
                    self.rowtransitions = 0
                    self.columntransitions = 0
                    self.holes = 0
                    self.wellsum = 0
            for i in range(self.aichange):
                self.change()
            j = self.aimove
            if(j < 9):
                self.left_move(9-j)
            elif(j > 9):
                self.right_move(j-9)
            color = 'lightblue'

            self.aichange = 0
            self.aimove = 0
            while not self.is_seated():
                self.draw_new_square()
                self.canvas.after(self.speed.get())
                self.canvas.update()

            self.get_seated()
            self.get_new_square()

            self.draw_score()
            self.draw_square()
            self.draw_new_square()

    def draw_old_square(self):
        '''
        画移动的方块
        '''
        self.canvas.delete("new")
        self.head_square[1] -= 1
    #    color = colorDict[random.randint(0, len(colorDict) - 1)]
        for i in range(4):
            self.canvas.create_rectangle(5 + (self.head_square[0] + self.new_square[i][0]) * self.width,
                                         5 +
                                         (self.head_square[1] +
                                          self.new_square[i][1]) * self.width,
                                         5 +
                                         (self.head_square[0] +
                                          self.new_square[i][0] + 1) * self.width,
                                         5 + (self.head_square[1] + 1 + self.new_square[i][1]) * self.width, fill=color,
                                         tags="new")

    def back(self, step=1):
        '''
        改变方块朝向的逆操作
        # 通过旋转改变，主方块不动
        '''
        for _ in range(4-step):
            if self.new > 1:
                for i in range(4):
                    if self.all_square[self.head_square[0] + change_dic[self.new_square[i]][0], self.head_square[1] +
                                       change_dic[self.new_square[i]][1]]:
                        return
                else:
                    for i in range(4):
                        self.new_square[i] = change_dic[self.new_square[i]]
            else:
                return

    def sim_score(self):
        '''
        计算模拟得分
        '''
        for j in range(self.row):
            for i in range(self.column):
                # 判断某行是否全满
                if not self.all_square[i, j]:
                    break
                else:
                    self.clearrows += 1

    def rt(self):
        '''
        计算行变换
        '''
        for j in range(self.row):
            for i in range(self.column):
                if self.all_square[i, j] != self.all_square[i+1, j]:
                    self.rowtransitions += 1

    def ct(self):
        '''
        计算列变换
        '''
        for j in range(self.column):
            for i in range(self.row):
                if self.all_square[j, i] != self.all_square[j-1, i]:
                    self.columntransitions += 1

    def ht(self):
        '''
        计算洞数
        '''
        flag = 0
        for j in range(self.column):
            for i in range(self.row):
                if self.all_square[j, i] == 1:
                    flag = 1
                if self.all_square[j, i] == 0 and flag == 1:
                    self.holes += 1

    def wt(self):
        '''
        计算井数
        '''
        flag = 0
        for j in range(self.row):
            for i in range(self.column):
                if self.all_square[i-1, j] == 1 and self.all_square[i+1, j] == 1 and self.all_square[i, j] == 0:
                    flag += 1
                else:
                    if(flag > 0):
                        self.wellsum += (1+flag)*flag/2
                        flag = 0

    def get_unseated(self):
        '''
        移动方块倒流
        '''
        self.all_square[tuple(self.head_square)] = 0
        for i in range(4):
            self.all_square[self.head_square[0] + self.new_square[i][0],
                            self.head_square[1] + self.new_square[i][1]] = 0


# 主程序
if __name__ == "__main__":
    Game_Russia()
