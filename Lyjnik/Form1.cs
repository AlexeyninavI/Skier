using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Timers;

namespace Lyjnik
{
    public partial class Form1 : Form
    {
 
        private System.Timers.Timer animTimer;
        
        int x;
        int y;

        int angleForCalc; // угол наклона
        double startSpeed; //начальная скорость
        double startLocation; // стартовая позиций
        int time; //время

        // свойства горы
        double heightMountain; 
        double weightMountain;

        //накопитель пикселей
        double storageX = 0;
        double storageY = 0;

        //свойства горы для рисования
        double lengthX;
        double lengthY;
    
   
        public void GetDrawParameters() // получить параметры для рисования
        {
            if (angleForCalc <= 45)
            {
                int b = (int)(Math.Tan(angleForCalc * (Math.PI / 180)) * 250);
                heightMountain = 250 - b-10;
                weightMountain = 0;
                lengthX = 250;
                lengthY = b;
            }

            else if
                (angleForCalc > 45)
            {
                double b = (250 / Math.Tan(angleForCalc * (Math.PI / 180)));
                heightMountain = 0;
                lengthX = (int)b+15;
                lengthY = 250;
                weightMountain = 0;
            }
           
        }

        public void DrawMountain(PaintEventArgs e)

        {
            if(angleForCalc <= 45)
            {
                
                int b = (int)(Math.Tan(angleForCalc * (Math.PI / 180)) * 250);
                heightMountain =  250-b;
                weightMountain = 0;
                
                e.Graphics.DrawLine(new Pen(Brushes.Black), 0, 250 - b, 250, 250);
                DrawPlato(e, 250);
            }

            else if 
                (angleForCalc > 45) {
                

                double b = (250/ Math.Tan(angleForCalc * (Math.PI / 180)));
                heightMountain = 0;
                weightMountain = 0;
                
                e.Graphics.DrawLine(new Pen(Brushes.Black), 0, 0, (int)b , 250);
                DrawPlato(e, b);
            }
           
            
        }
       
      
        public void DrawPlato(PaintEventArgs e, double b)

        {
           e.Graphics.DrawLine(new Pen(Brushes.Black), 550, pictureBox3.Height-1, (int)b , pictureBox3.Height-1);
        }


        public Form1()
        {
            InitializeComponent();
            pictureBox2.Parent = pictureBox3;
          


        }
        private void SetAnimTimer()
        {
            animTimer = new System.Timers.Timer(20);
            animTimer.Elapsed += OnTimedEventAnim;
            animTimer.AutoReset = true;
            animTimer.Enabled = true;
            x = (int)weightMountain;
            y = (int)heightMountain;
        }
       
        private void OnTimedEventAnim(Object source, ElapsedEventArgs e) // Moving time
        {



            double changeX = lengthX  /lengthY;
            double changeY = lengthY / lengthX;
            if(angleForCalc < 15)
            {
                changeY *= 1.5;
                changeX /= 2;


            }
            if (angleForCalc >= 15 && angleForCalc <20)
            {
                
                changeX /= 2;


            }
            if (angleForCalc < 27 && angleForCalc >=20)
            {
                changeY *= 1.8;
               


            }
            if (angleForCalc > 70)
            {
                changeX *= 2;

            }
            if (changeY > 1)
            {
                storageY = storageY + changeY;
                double ostatok = Math.Round(changeY% 1,3);
                double iY = storageY - ostatok;
               storageY =  storageY - iY;
                y+=(int)iY;
            }
           
            else if (changeY <= 1)
            {
                storageY += changeY;
                if(storageY >= 1)
                {
                    y++;
                    storageY = storageY % 1;
                }
            }

            if (changeX > 1)
            {
                storageX = storageX + changeX;
                double ostatok = Math.Round(changeX % 1, 3);
                double iX = storageX - ostatok;
                storageX = storageX - iX;
                x += (int)iX;
            }

            else if (changeX <= 1)
            {
                storageX += changeX;
                if (storageX >= 1)
                {
                    x++;
                    storageX = storageX % 1;
                }
            }
            if (y >= 230)
            {
                animTimer.Enabled = false;
                button1.Enabled = true;
                x = y = 0;
                return;

            }

            pictureBox2.Location = new Point(x, y);


        }
        public void SetPicBox()
        {
            pictureBox2.Location = new Point((int)weightMountain, (int)heightMountain);
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            double massa = double.Parse(textBox2.Text.ToString());
            angleForCalc = int.Parse(textBox4.Text.ToString());
            startLocation = double.Parse(textBox1.Text.ToString());
            startSpeed = double.Parse(textBox5.Text.ToString());
            time = int.Parse(textBox3.Text.ToString());

            if (angleForCalc < 10 || angleForCalc > 80)
            {
                MessageBox.Show("Недопустимый угол");
                return;
            }

            pictureBox3.Invalidate();
            pictureBox3.Update();
            GetDrawParameters();
            SetPicBox();

            label3.Text ="Ускорение лыжника: " + calculateAcceleration();
            label4.Text = "Путь: " + calculateDistance(calculateSpeed(calculateAcceleration()),calculateAcceleration());
            label8.Text = "Скорость: " + calculateSpeed(calculateAcceleration());
            button1.Enabled = false;
            SetAnimTimer();

        }

        private double calculateAcceleration()
        {
            double acceleration = Math.Round(9.8 * (Math.Sin(angleForCalc * (Math.PI / 180)))  - 0.45 * Math.Cos(angleForCalc * (Math.PI / 180)),4);
            return acceleration;
        }
        private double calculateDistance(double speed,double acceleration)
        {
            double distance = startSpeed +speed*time+acceleration*time;
            return distance;
        }
        private double calculateSpeed(double acceleration)
        {
            double speed = startSpeed+acceleration*time;
            return speed ;
        }


      
        private Font fnt = new Font("Arial", 10);
        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            DrawMountain(e);
        }

      
    }
}

