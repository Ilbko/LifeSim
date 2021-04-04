using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Controller;
using WindowsFormsApp2.Model.Base;
using WindowsFormsApp2.View;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Random r;
        CellController controller;
        ElementController controller_el;
        readonly int PEN_SIZE = 7;
        readonly Color PEN_COLOR_GOOD;
        readonly Color PEN_COLOR_DIE;
        Color elementColor = Color.Black;
        readonly Pen myPen;
        readonly Pen diePen;
        readonly Pen elementPen;
        public Form1()
        {
            InitializeComponent();
            
            controller = new CellController();
            controller_el = new ElementController();
            r = new Random();

            myPen = new Pen(PEN_COLOR_GOOD, PEN_SIZE);
            diePen = new Pen(PEN_COLOR_DIE, PEN_SIZE);

            this.DoubleBuffered = true;

            System.Windows.Forms.Timer timerMove = new System.Windows.Forms.Timer();
            timerMove.Tick += TimerMove_Tick;
            timerMove.Interval = 10;
            timerMove.Start();

            System.Windows.Forms.Timer timerKiller = new System.Windows.Forms.Timer();
            timerKiller.Tick += TimerKiller_Tick;
            timerKiller.Interval = 1000;
            timerKiller.Start();

            System.Windows.Forms.Timer timerBirth = new System.Windows.Forms.Timer();
            timerBirth.Tick += TimerBirth_Tick;
            timerBirth.Interval = 5000;
            timerBirth.Start();

            if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour <= 6)
            {
                this.BackColor = Color.Black;
                this.elementColor = Color.White;
            }

            elementPen = new Pen(elementColor, 3);
        }

        private async void TimerBirth_Tick(object sender, EventArgs e)
        {
            int limit = controller.cellCollection.Count();

            for (int i = 0; i < limit; i++)
            {
                try
                {
                    await Task.Run(() => controller.GenerateNewCell(controller.cellCollection[i]));
                    //new Task(() => controller.GenerateNewCell(controller.cellCollection[i]));
                    //controller.GenerateNewCell(controller.cellCollection[i]);
                }
                catch (System.ArgumentOutOfRangeException) { }           
            }
            Task.WaitAll();
            this.Refresh();
        }

        private async void TimerKiller_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => controller.LifeCycle(this.ClientSize, controller_el));
            await Task.Run(() => controller_el.AgeElement());
        }

        private async void TimerMove_Tick(object sender, EventArgs e)
        {
            if (controller.cellCollection.Count() > 0)
            {
                //Не успевает за клетками (ошибка индексации в цикле)
                int limit = controller.cellCollection.Count();

                //Лагает, "некрасиво" работает
                //for(int i = 0; i < controller.cellCollection.Count(); i++)

                for (int i = 0; i < limit; i++)
                {
                    //Этот блок никак не будет влиять на работу программы, ведь мёртвая клетка не может породить новую клетку
                    try
                    {
                        await Task.Run(() => controller.MoveCell(controller.cellCollection[i], this.ClientSize, controller_el));
                    }
                    catch (System.ArgumentOutOfRangeException) { }
                }
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

            foreach (Element item in controller_el.elementCollection)
            {
                e.Graphics.DrawRectangle(elementPen, new Rectangle(item.PosX, item.PosY, (int)item.SizeH, (int)item.SizeW));
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                controller.GenerateNewCellClick(e.Location);
            else if (e.Button == MouseButtons.Right)
            {
                foreach(Cell item in controller.cellCollection)
                {
                    if (e.Location.X > item.PosX && e.Location.X < item.PosX + item.SizeW &&
                        e.Location.Y > item.PosY && e.Location.Y < item.PosY + item.SizeH)
                    {
                        Cell tmp = item;
                        List<int> childId = new List<int>();
                        foreach (Cell child in controller.cellCollection)
                            if (child.ParentId == item.Id)
                                childId.Add(child.Id);

                        new FormInfo(tmp, controller).Show();
                        break;
                    }
                }
            }
        }
    }
}
