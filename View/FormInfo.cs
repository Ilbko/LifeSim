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
    public partial class FormInfo : Form
    {
        Cell infoCell;
        string childIdStr = string.Empty;
        Timer updateTimer = new Timer();

        CellController controller = new CellController();
        public FormInfo()
        {
            InitializeComponent();
        }

        public FormInfo(Cell tmp)
        {
            InitializeComponent();
            this.infoCell = tmp;

            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Start();

            this.button4.BackColor = tmp.CellColor;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            foreach (int item in infoCell.ChildrenId.Value)
                this.childIdStr += item.ToString() + " ";

            this.textBox1.Text = $"Id: {infoCell.Id}\r\n" +
                                 $"Родитель (Id): {infoCell.ParentId}\r\n" +
                                 $"Время жизни: {infoCell.Age}\r\n" +
                                 $"Цвет: {infoCell.CellColor.ToString()}\r\n" +
                                 $"До смерти: {controller.maxCellLife - infoCell.Age}\r\n" +
                                 $"Количество детей: {infoCell.ChildrenId.Value.Count()}\r\n" +
                                 $"Дети (Id): {childIdStr}\r\n";

            childIdStr = string.Empty;

            if (controller.maxCellLife - infoCell.Age <= 0)
                this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            infoCell.Age = controller.maxCellLife;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            infoCell.Age -= 4;
            if (infoCell.Age < 0)
                infoCell.Age = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            controller.GenerateNewCell(infoCell);
        }
    }
}
