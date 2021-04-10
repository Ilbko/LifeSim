using System;
using System.Collections.Generic;

namespace WindowsFormsApp2.Model.Base
{
    //Класс микроэлемента
    public class Element
    {
        public Element()
        {
            this.SizeH = 4;
            this.SizeW = 4;
        }

        //Конструктор элемента с его позицией
        public Element(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;

            this.SizeH = 4;
            this.SizeW = 4;
        }

        //Возраст элемента
        public int Age { get; set; }
        //Локация элемента
        public int PosX { get; set; }
        public int PosY { get; set; }
        //Размеры элемента
        public float SizeH { get; private set; }
        public float SizeW { get; private set; }

        //Метод старения
        public void Old() => Age++;
    }
}
