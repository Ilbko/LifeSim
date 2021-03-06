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
    //Класс главной формы
    public partial class Form1 : Form
    {
        Random r;
        CellController controller;
        ElementController controller_el;
        //readonly int PEN_SIZE = 7;
        int penSize = 7;
        readonly Color PEN_COLOR_GOOD;
        readonly Color PEN_COLOR_DIE;
        Color elementColor = Color.Black;
        readonly Pen myPen;
        readonly Pen diePen;
        readonly Pen elementPen;
        readonly ToolTip buttonTip;
        public Form1()
        {
            InitializeComponent();
            
            controller = new CellController();
            controller_el = new ElementController();
            buttonTip = new ToolTip();
            r = new Random();

            //Ручки для рисования клеток
            //myPen = new Pen(PEN_COLOR_GOOD, PEN_SIZE);
            //diePen = new Pen(PEN_COLOR_DIE, PEN_SIZE);
            myPen = new Pen(PEN_COLOR_GOOD, penSize);
            diePen = new Pen(PEN_COLOR_DIE, penSize);

            //Убирает мигание. Очень важно
            this.DoubleBuffered = true;

            //Таймер движения с тиком каждые 10 милисекунд
            System.Windows.Forms.Timer timerMove = new System.Windows.Forms.Timer();
            timerMove.Tick += TimerMove_Tick;
            timerMove.Interval = 10;
            timerMove.Start();

            //Таймер старения с тиком каждую секунду
            System.Windows.Forms.Timer timerKiller = new System.Windows.Forms.Timer();
            timerKiller.Tick += TimerKiller_Tick;
            timerKiller.Interval = 1000;
            timerKiller.Start();

            //Таймер потомства с тиком каждые пять секунд
            System.Windows.Forms.Timer timerBirth = new System.Windows.Forms.Timer();
            timerBirth.Tick += TimerBirth_Tick;
            timerBirth.Interval = 5000;
            timerBirth.Start();

            //Таймер кнопки с информацией с тиком в одну милисекунду (изменение цвета для привлечения внимания)
            System.Windows.Forms.Timer timerInfoButton = new System.Windows.Forms.Timer();
            timerInfoButton.Tick += TimerInfoButton_Tick;
            timerInfoButton.Interval = 1;
            timerInfoButton.Start();

            if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour <= 6)
            {
                this.BackColor = Color.Black;
                this.elementColor = Color.White;
            }

            elementPen = new Pen(elementColor, 3);

            buttonTip.ShowAlways = true;
            buttonTip.SetToolTip(button1, "ЛКМ - создать рандомную клетку;\nПКМ (по клетке) - открыть информацию и редактировать;\n" +
                "Shift+ЛКМ - создать кастомную клетку.\n\n" +
                "Горячие клавиши:\nZ - создать микроэлемент на месте курсора;\nX - убить всех клеток;\nC - убрать все микроэлементы.\n\n" +
                "W - увеличить толщину клеток;\nS - уменьшить толщину клеток.\n\n" +
                "Размер игрового поля можно изменить!");

            this.button1.BackColor = Color.Red;
        }

        //Событие тика таймера изменения цвета инфокнопки
        private void TimerInfoButton_Tick(object sender, EventArgs e)
        {
            //this.button1.BackColor = Color.FromArgb((this.button1.BackColor.R + 1) % 255, 
            //                                        (this.button1.BackColor.G + 1) % 255, 
            //                                        (this.button1.BackColor.B + 1) % 255);
            //Радужное изменение цвета инфокнопки
            if      (this.button1.BackColor.R == 255 && this.button1.BackColor.B != 255 && this.button1.BackColor.G == 0)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R, this.button1.BackColor.G, this.button1.BackColor.B + 1);
            
            else if (this.button1.BackColor.R != 0   && this.button1.BackColor.B == 255)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R - 1, this.button1.BackColor.G, this.button1.BackColor.B);
            
            else if (this.button1.BackColor.R != 255 && this.button1.BackColor.G != 255 && this.button1.BackColor.B == 255)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R, this.button1.BackColor.G + 1, this.button1.BackColor.B);
            
            else if (                                   this.button1.BackColor.G == 255 && this.button1.BackColor.B != 0)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R, this.button1.BackColor.G, this.button1.BackColor.B - 1);
            
            else if (this.button1.BackColor.R != 255 && this.button1.BackColor.G == 255 && this.button1.BackColor.B != 255)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R + 1, this.button1.BackColor.G, this.button1.BackColor.B);
            
            else if (this.button1.BackColor.R == 255 && this.button1.BackColor.G != 0)
                this.button1.BackColor = Color.FromArgb(this.button1.BackColor.R, this.button1.BackColor.G - 1, this.button1.BackColor.B);
        }

        //Событие тика таймера рождения клетки
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

        //Событие тика таймера старения клетки и элемента
        private async void TimerKiller_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => controller.LifeCycle(this.ClientSize, controller_el));
            await Task.Run(() => controller_el.AgeElement());
        }

        //Событие тика таймера движения клетки
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

        //Событие отрисовки на форме
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                foreach (Cell item in controller.cellCollection)
                {
                    myPen.Color = item.CellColor;
                    diePen.Color = Color.FromArgb(30, item.CellColor);
                    e.Graphics.DrawRectangle(item.Age < controller.maxCellLife - 1 ? myPen : diePen, new Rectangle((int)item.PosX, (int)item.PosY, (int)item.SizeW, (int)item.SizeH));
                }
            } catch (System.InvalidOperationException) { }

            try
            {
                foreach (Element item in controller_el.elementCollection)
                {
                    e.Graphics.DrawRectangle(elementPen, new Rectangle(item.PosX, item.PosY, (int)item.SizeH, (int)item.SizeW));
                }
            } catch (System.InvalidOperationException) { }
        }

        //Событие нажатия клавиши мыши 
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Shift)
                new FormCreate(controller, this.ClientSize, e.Location).Show();
            else if (e.Button == MouseButtons.Left)
                controller.GenerateNewCellClick(e.Location);
            else if (e.Button == MouseButtons.Right)
            {
                foreach (Cell item in controller.cellCollection)
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

        //Событие нажатия кнопки клавиатуры
        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'z' || e.KeyChar == 'Z' || e.KeyChar == 'я' || e.KeyChar == 'Я')
                controller_el.elementCollection.Add(new Element(this.PointToClient(Cursor.Position).X, this.PointToClient(Cursor.Position).Y));
            //Point loc = this.PointToClient(Cursor.Position);

            else if (e.KeyChar == 'x' || e.KeyChar == 'X' || e.KeyChar == 'ч' || e.KeyChar == 'Ч')
                controller.cellCollection.Clear();

            else if (e.KeyChar == 'c' || e.KeyChar == 'C' || e.KeyChar == 'с' || e.KeyChar == 'С')
                controller_el.elementCollection.Clear();

            else if (e.KeyChar == 'w' || e.KeyChar == 'W' || e.KeyChar == 'ц' || e.KeyChar == 'Ц')
            {
                if (penSize < 20)
                    myPen.Width = ++penSize;
            }

            else if (e.KeyChar == 's' || e.KeyChar == 'S' || e.KeyChar == 'ы' || e.KeyChar == 'Ы')
            {
                if (penSize > 0) 
                    myPen.Width = --penSize;
            }
        }
    }
}
