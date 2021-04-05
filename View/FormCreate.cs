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

namespace WindowsFormsApp2.View
{
    public partial class FormCreate : Form
    {
        protected Random r = new Random();
        protected Timer buttonTimer = new Timer();
        protected CellController controller;
        protected Point location;

        public FormCreate()
        {
            InitializeComponent();
        }

        public FormCreate(CellController controller, Size clientSize, Point loc)
        {
            InitializeComponent();
            this.trackBar1.Value = r.Next(0, this.trackBar1.Maximum);
            this.trackBar2.Value = r.Next(0, this.trackBar2.Maximum);
            this.trackBar3.Value = r.Next(0, this.trackBar3.Maximum);

            this.numericUpDown1.Maximum = clientSize.Width / 2;
            this.numericUpDown2.Maximum = clientSize.Height / 2;

            buttonTimer.Interval = 1;
            buttonTimer.Tick += ButtonTimer_Tick;
            buttonTimer.Start();

            this.controller = controller;
            this.location = loc;

            //Трекбары плохо работают с цветами. Ужасно выглядит.
            //if (DateTime.Now.Hour >= 18 || DateTime.Now.Hour <= 6)
            //{
            //    this.BackColor = Color.Black;
            //    this.numericUpDown1.BackColor = this.numericUpDown2.BackColor = this.numericUpDown3.ForeColor = Color.Black;
            //}
        }

        private void ButtonTimer_Tick(object sender, EventArgs e)
        {
            this.button2.BackColor = Color.FromArgb((this.button2.BackColor.R + 1) % 255,
                                                    (this.button2.BackColor.B + 1) % 255,
                                                    (this.button2.BackColor.G + 1) % 255);
            this.button2.ForeColor = Color.FromArgb(255 - this.button2.BackColor.R,
                                                    255 - this.button2.BackColor.R,
                                                    255 - this.button2.BackColor.R);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            this.button1.BackColor = Color.FromArgb(this.trackBar1.Value,
                                                    this.trackBar2.Value,
                                                    this.trackBar3.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            controller.cellCollection.Add(new Model.Base.Cell(location.X, location.Y, (int)numericUpDown1.Value, (int)numericUpDown2.Value, controller.totalId++, -1,
                Color.FromArgb(this.trackBar1.Value, this.trackBar2.Value, this.trackBar3.Value), (int)numericUpDown3.Value));

            this.Dispose();
        }
    }
}
