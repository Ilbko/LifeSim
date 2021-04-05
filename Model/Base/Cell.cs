using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp2.Model.Base
{
    public class Cell 
    {
        public Cell(int posX, int posY, int id, int parentId, Color cellColor, int maxChildren)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.Id = id;
            this.ParentId = parentId;
            this.CellColor = cellColor;
            //this.Children = 0;
            this.MaxChildren = maxChildren;
            this.SizeH = 20;
            this.SizeW = 20;
        }
        public Cell(int posX, int posY, int sizeW, int sizeH, int id, int parentId, Color cellColor, int maxChildren)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.SizeH = sizeH;
            this.SizeW = sizeW;
            this.Id = id;
            this.ParentId = parentId;
            this.CellColor = cellColor;
            this.MaxChildren = maxChildren;
        }
        public Cell(Cell cell, int id, int parentId, Color cellColor, int maxChildren) : this((int)cell.PosX, (int)cell.PosY, id, parentId, cellColor, maxChildren){}
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public Color CellColor { get; private set; }

        //Количество детей будет заменять список идентификаторов детей
        //public int Children { get; set; }
        /*Lazy, чтобы не приходилось выделять новую память под список, если клетка не может иметь детей
          или чтобы не выделять память с помощью if (ChildrenId == null)*/
        public Lazy<List<int>> ChildrenId { get; set; } = new Lazy<List<int>>();
        public int MaxChildren { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public int SizeH { get; set; }
        public int SizeW { get; set; }
        public int Age { get; set; }

        public UInt16 FoodEaten { get; set; }

        public bool isSamePosition(Cell other) => other.PosX == PosX && other.PosY == PosY;
        public void Old() => Age++;
    }
}
