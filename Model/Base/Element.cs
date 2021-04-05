using System;
using System.Collections.Generic;

namespace WindowsFormsApp2.Model.Base
{
    public class Element
    {
        public Element()
        {
            this.SizeH = 4;
            this.SizeW = 4;
        }

        public Element(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;

            this.SizeH = 4;
            this.SizeW = 4;
        }

        public int Age { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public float SizeH { get; private set; }
        public float SizeW { get; private set; }

        public void Old() => Age++;
    }
}
