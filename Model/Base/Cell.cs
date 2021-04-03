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
            this.Children = 0;
            this.MaxChildren = maxChildren;
            this.SizeH = 20;
            this.SizeW = 20;
        }
        public Cell(Cell cell, int id, int parentId, Color cellColor, int maxChildren) : this((int)cell.PosX, (int)cell.PosY, id, parentId, cellColor, maxChildren){}
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public Color CellColor { get; private set; }

        public int Children { get; set; }
        public int MaxChildren { get; private set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float SizeH { get; private set; }
        public float SizeW { get; private set; }
        public int Age { get; private set; }
        public bool isSamePosition(Cell other) => other.PosX == PosX && other.PosY == PosY;
        public void Old() => Age++;
    }
}
