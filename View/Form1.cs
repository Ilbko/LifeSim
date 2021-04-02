using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Controller;
using WindowsFormsApp2.Model.Base;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Random r;
        CellController controller;
        readonly int PEN_SIZE = 3;
        readonly Color PEN_COLOR_GOOD;
        readonly Color PEN_COLOR_DIE;
        readonly Pen myPen;
        readonly Pen diePen;
        public Form1()
        {
            InitializeComponent();
            
            controller = new CellController();
            r = new Random();

            myPen = new Pen(PEN_COLOR_GOOD, PEN_SIZE);
            diePen = new Pen(PEN_COLOR_DIE, PEN_SIZE);

            this.DoubleBuffered = true;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Start();

            System.Windows.Forms.Timer timerKiller = new System.Windows.Forms.Timer();
            timerKiller.Tick += TimerKiller_Tick;
            timerKiller.Interval = 200;
            timerKiller.Start();
        }

        private void TimerKiller_Tick(object sender, EventArgs e)
        {
            controller.LifeCicle();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (controller.cellCollection.Count() > 0)
            {
                //Не успевает за клетками (ошибка индексации в цикле)
                int limit = controller.cellCollection.Count();

                //Лагает, "некрасиво" работает
                //for(int i = 0; i < controller.cellCollection.Count(); i++)
                try
                {
                    for (int i = 0; i < limit; i++)
                    {
                        await Task.Run(() => controller.GenerateNewCell(controller.cellCollection[i]));
                        //new Task(() => controller.GenerateNewCell(controller.cellCollection[i]));
                    }
                }
                catch (System.ArgumentOutOfRangeException){ }
            }
            Task.WaitAll();
            this.Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {     
            foreach (Cell item in controller.cellCollection)
            {
                myPen.Color = item.CellColor;
                diePen.Color = Color.FromArgb(30, item.CellColor);
                e.Graphics.DrawRectangle(item.Age < controller.maxCellLife - 1 ? myPen : diePen, new Rectangle((int)item.PosX, (int)item.PosY, (int)item.SizeH, (int)item.SizeW));
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            controller.GenerateNewCellClick(e.Location); 
        }
    }
}
