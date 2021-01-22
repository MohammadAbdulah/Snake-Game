using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FinalExam
{
    public partial class Form1 : Form
    {
        List<SnakeBody> snake = new List<SnakeBody>();
        Graphics g1;
        Graphics g2;
        SolidBrush brush = new SolidBrush(Color.Black);
        Random rnd = new Random();
        Bitmap bitMap;
        Point[] steps = new Point[4];
        int stepCounter = 0;

        public Form1()
        {
            InitializeComponent();
            g1 = this.CreateGraphics();
            bitMap = new Bitmap(this.Width, this.Height);
            g2 = Graphics.FromImage(bitMap);
            g2.Clear(Color.White);
            start();
        }
        public void reDraw()
        {
            g1.Clear(Color.White);
            foreach (var part in snake)
            {
                if (part == snake[0])
                {
                    part.draw(g1, brush, Color.Red);
                    part.draw(g2, brush, Color.Red);
                }
                else
                {
                    part.draw(g1, brush, Color.Black);
                    part.draw(g2, brush, Color.Black);
                }
            }
        }
        public void addBody()
        {
            int x = snake[snake.Count - 1].Location.X;
            int y = snake[snake.Count - 1].Location.Y;
            int dir = snake[snake.Count - 1].Direction;
            SnakeBody b1 = new SnakeBody();
            b1.Direction = dir;

            switch (dir)
            {
                case 0:
                    b1.setX(x);
                    b1.setY(y + 17);
                    break;
                case 1:
                    b1.setX(x);
                    b1.setY(y - 17);
                    break;
                case 2:
                    b1.setX(x + 17);
                    b1.setY(y);
                    break;
                case 3:
                    b1.setX(x - 17);
                    b1.setY(y);
                    break;
            }
            snake.Add(b1);
        }
        public void resetFood()
        {
            pictureBox1.Location = new Point(rnd.Next(0, this.Size.Width - 50), rnd.Next(0, this.Size.Height - 50));
            pictureBox1.Visible = true;
        }
        public void start()
        {
            resetFood();
            SnakeBody head = new SnakeBody();
            head.setX(rnd.Next(0, this.Size.Width - 50));
            head.setY(rnd.Next(0, this.Size.Height - 50));
            head.Direction = rnd.Next(0, 4);
            snake.Add(head);
            addBody();
            addBody();
            reDraw();
        }
        public void redirect(SnakeBody s, int index)
        {
            switch (snake[index - 1].Direction)
            {
                case 0:
                case 1:
                    if (s.getX() == snake[index - 1].getX())
                        s.Direction = snake[index - 1].Direction;
                    break;
                case 2:
                case 3:
                    if (s.getY() == snake[index - 1].getY())
                        s.Direction = snake[index - 1].Direction;
                    break;
            }
        }
        public void moveSnake()
        {
            for (int i = 0; i < snake.Count; i++)
            {
                if(i == 0)
                    moveBody(snake[i], 1);
                else if (snake[i].Direction == snake[i - 1].Direction)
                    moveBody(snake[i], 1);
                else if (snake[i].Direction != snake[i - 1].Direction)
                {
                    redirect(snake[i], i);
                    moveBody(snake[i], 1);
                }
                reDraw();
            }
        }
        public void moveBody(SnakeBody s, int speed)
        {
            int x = s.Location.X;
            int y = s.Location.Y;

            switch (s.Direction)
            {
                case 0:
                    s.setX(x);
                    s.setY(y - speed);
                    break;
                case 1:
                    s.setX(x);
                    s.setY(y + speed);
                    break;
                case 2:
                    s.setX(x - speed);
                    s.setY(y);
                    break;
                case 3:
                    s.setX(x + speed);
                    s.setY(y);
                    break;
            }
        }
        public void saveStep(int x, int y)
        {
            steps[stepCounter % steps.Length].X = x;
            steps[stepCounter % steps.Length].Y = y;
            stepCounter++;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            moveSnake();
            if (snake[0].Location.X >= pictureBox1.Location.X && snake[0].Location.X <= pictureBox1.Location.X + pictureBox1.Width)
                if (snake[0].Location.Y >= pictureBox1.Location.Y && snake[0].Location.Y <= pictureBox1.Location.Y + pictureBox1.Height)
                {
                    resetFood();
                    addBody();
                }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //reDraw();
            //g1.DrawImage(bitMap, 0, 0, bitMap.Width, bitMap.Height);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    if(snake[0].Direction != 1)
                    {
                        snake[0].Direction = 0;
                        saveStep(snake[0].getX(), snake[0].getY());
                    }
                    break;
                case Keys.Down:
                    if (snake[0].Direction != 0)
                    {
                        snake[0].Direction = 1;
                        saveStep(snake[0].getX(), snake[0].getY());
                    }
                    break;
                case Keys.Left:
                    if (snake[0].Direction != 3)
                    {
                        snake[0].Direction = 2;
                        saveStep(snake[0].getX(), snake[0].getY());
                    }
                    break;
                case Keys.Right:
                    if (snake[0].Direction != 2)
                    {
                        snake[0].Direction = 3;
                        saveStep(snake[0].getX(), snake[0].getY());
                    }
                    break;
            }
        }
        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "textfile|*.txt";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter inFile = new StreamWriter(dlg.FileName);
                    for (int i = 0; i < snake.Count; i++)
                    {
                        if (i == 0)
                            inFile.WriteLine("Snake head direction = " + snake[i].Direction + " and location = " + snake[i].Location);
                        else
                            inFile.WriteLine("Snake part direction = " + snake[i].Direction + " and location = " + snake[i].Location);
                    }
                    inFile.Close();
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }
        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "jpegImage|*.jpeg|png|*.png|gif|*.gif";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FilterIndex == 1)
                    bitMap.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (dlg.FilterIndex == 2)
                    bitMap.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                else if (dlg.FilterIndex == 3)
                    bitMap.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Gif);
            }
        }
        private void stratToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}