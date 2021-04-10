using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Controller;
using WindowsFormsApp2.Model.Base;

namespace WindowsFormsApp2.View
{
    //Форма, содержащая информацию о клетке
    public partial class FormInfo : Form
    {
        Cell infoCell;
        string childIdStr = string.Empty;
        Timer updateTimer = new Timer();

        public CellController controller;
        public FormInfo()
        {
            InitializeComponent();
        }

        public FormInfo(Cell tmp, CellController controller)
        {
            InitializeComponent();
            this.infoCell = tmp;

            //Таймер для обновления окна информации
            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Interval = 500;
            this.updateTimer.Start();

            this.controller = controller;
            this.button4.BackColor = tmp.CellColor;

            if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour <= 6)
                this.BackColor = Color.Black;
        }

        //Событие тика таймера обновления текстбокса информации
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            //Id всех детей помещается в строку
            foreach (int item in infoCell.ChildrenId.Value)
                this.childIdStr += item.ToString() + " ";

            //Текст для текстбокса с информацией
            this.textBox1.Text = $"Id: {infoCell.Id}\r\n" +
                                 $"Родитель (Id): {infoCell.ParentId}\r\n" +
                                 $"Время жизни: {infoCell.Age}\r\n" +
                                 $"Цвет: {infoCell.CellColor.ToString()}\r\n" +
                                 $"Съеденно микроэлементов: {infoCell.FoodEaten}\r\n" +
                                 $"До смерти: {controller.maxCellLife - infoCell.Age}\r\n" +
                                 $"Количество детей: {infoCell.ChildrenId.Value.Count()} из {infoCell.MaxChildren}\r\n" +
                                 $"Дети (Id): {childIdStr}\r\n";

            childIdStr = string.Empty;

            //Если клетка умерла
            if (controller.maxCellLife - infoCell.Age <= 0)
                this.Dispose();
                //controller.cellCollection.Remove(infoCell);
        }

        //Событие нажатия на кнопку "Убить"
        private void button1_Click(object sender, EventArgs e)
        {
            infoCell.Age = controller.maxCellLife;
            controller.cellCollection.Remove(infoCell);
            //this.Dispose();
        }

        //Событие нажатия на кнопку "Покоромить"
        private void button2_Click(object sender, EventArgs e)
        {
            infoCell.Age -= 4;
            if (infoCell.Age < 0)
                infoCell.Age = 0;
        }

        //Событие нажатия на кнопку "Дать потомство"
        private async void button3_Click(object sender, EventArgs e)
        {
            await Task.Run(() => controller.GenerateNewCell(infoCell));           
        }

        //Событие нажатия на кнопку "Убить потомство"
        private void button5_Click(object sender, EventArgs e)
        {
            //List<Cell> children = controller.cellCollection.Where(x => x.ParentId == infoCell.Id);
            //foreach(Cell item in children)
            //{
            //    item.Age = controller.maxCellLife;
            //    controller.cellCollection.Remove(item);
            //}

            /*Для всего потомства (потомство - те клетки, id родителя которых совпадает с id клетки, с которой сейчас производится работа)
              выполняется старение, а затем удаление со списка*/
            controller.cellCollection.Where(x => x.ParentId == infoCell.Id).ForEach(x => x.Age = controller.maxCellLife);
            controller.cellCollection.RemoveAll(x => x.ParentId == infoCell.Id);
        }

        //Событие нажатия на кнопку "Убить геном"
        private void button6_Click(object sender, EventArgs e)
        {
            controller.cellCollection.Where(x => x.CellColor == infoCell.CellColor).ForEach(x => x.Age = controller.maxCellLife);
            controller.cellCollection.RemoveAll(x => x.CellColor == infoCell.CellColor);
        }

        //Событие нажатия на кнопку "Макс. дети++"
        private void button7_Click(object sender, EventArgs e)
        {
            infoCell.MaxChildren++;
        }
    }
}
