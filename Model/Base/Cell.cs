using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp2.Model.Base
{
    public class Cell 
    {
        //Конструктор клетки с фиксированными размерами
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
        //Конструктор клетки с задаными размерами (для создания кастомной клетки)
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
        //Конструктор клетки с родительской клеткой в качестве параметра
        public Cell(Cell cell, int id, int parentId, Color cellColor, int maxChildren) : this((int)cell.PosX, (int)cell.PosY, id, parentId, cellColor, maxChildren){}
        //Id клетки
        public int Id { get; private set; }
        //Id родителя
        public int ParentId { get; private set; }
        //Цвет клетки
        public Color CellColor { get; private set; }

        //Количество детей будет заменять список идентификаторов детей
        //public int Children { get; set; }
        /*Lazy, чтобы не приходилось выделять новую память под список, если клетка не может иметь детей
          или чтобы не выделять память с помощью if (ChildrenId == null)*/
        public Lazy<List<int>> ChildrenId { get; set; } = new Lazy<List<int>>();
        //Максимальное количество детей у клетки 
        public int MaxChildren { get; set; }
        //Локация клетки
        public float PosX { get; set; }
        public float PosY { get; set; }
        //Размер клетки (Высота и ширина)
        public int SizeH { get; set; }
        public int SizeW { get; set; }
        //Возраст клетки (в секундах)
        public int Age { get; set; }
        //Количество съеденной еды
        public int FoodEaten { get; set; }
        //Метод для проверки локации клетки относительно другой клетки (используется при рождении новой клетки)
        public bool isSamePosition(Cell other) => other.PosX == PosX && other.PosY == PosY;
        //Метод старения, выполняющийся посекундно 
        public void Old() => Age++;
    }
}
