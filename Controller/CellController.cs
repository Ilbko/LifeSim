using System;
using WindowsFormsApp2.Model;
using WindowsFormsApp2.Model.Base;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp2.Controller
{
    //Контроллер клеток, через который производятся манипуляции с коллекцией всех клеток
    public class CellController
    {
        public Cells cellCollection { get; private set; } = new Cells();
        //public ElementController controller_el { get; private set; }
        protected Random r = new Random();
        //protected readonly int radius = 25;
        protected int radius;
        protected readonly int minCellCount = 2;
        public readonly int maxCellLife = 20;
        public int totalId = 0;

        //public int Count { get => cellCollection.Count(); }
        //public void GenerateNewCell()
        //{
        //    lock (this)
        //    {
        //        Cell tmp = new Cell(this.cellCollection[r.Next(0, this.cellCollection.Count())], cellCollection.Count(), this.cellCollection[this.cellCollection.Count() - 1].Id,
        //            this.cellCollection[this.cellCollection.Count() - 1].CellColor);
        //        do
        //        {
        //            if (r.Next((int)tmp.SizeH * radius) < (((int)tmp.SizeH * radius) / 2))
        //                tmp.PosX += tmp.SizeH;
        //            else
        //                tmp.PosX -= tmp.SizeH;
        //            if (r.Next((int)tmp.SizeW * radius) < (((int)tmp.SizeW * radius) / 2))
        //                tmp.PosY += tmp.SizeW;
        //            else
        //                tmp.PosX -= tmp.SizeW;
        //        } while (this.cellCollection.cells.Any<Cell>(x => x.isSamePosition(tmp)));

        //        this.cellCollection.Add(new Cell(tmp, tmp.Id, tmp.ParentId, tmp.CellColor));

        //        GC.Collect(GC.GetGeneration(tmp));
        //    }
        //}

        //Метод генерации новой клетки от родителя
        public void GenerateNewCell(Cell tmp)
        {
            lock (this)
            {
                //Если клетка ещё может давать детей
                if (tmp.ChildrenId.Value.Count() < tmp.MaxChildren)
                {
                    //Новой клетке даётся глобальный id
                    int newId = totalId++;
                    //Создаётся новая клетка
                    Cell newCell = new Cell(tmp, newId, tmp.Id, tmp.CellColor, r.Next(0, 5));
                    //Формула радиуса круга зоны возможного спавна
                    radius = (tmp.SizeH + tmp.SizeW) * (int)Math.PI;

                    //Подбирается место спавна новой клетки
                    do
                    {
                        if (r.Next(newCell.SizeH * radius) < ((newCell.SizeH * radius) / 2))
                            newCell.PosX += newCell.SizeH;
                        else
                            newCell.PosX -= newCell.SizeH;
                        if (r.Next(newCell.SizeW * radius) < ((newCell.SizeW * radius) / 2))
                            newCell.PosY += newCell.SizeW;
                        else
                            newCell.PosY -= newCell.SizeW;
                    } while (this.cellCollection.cells.Any(x => x.isSamePosition(newCell)));

                    //Новая клетка добавляется в коллекцию
                    //this.cellCollection.Add(new Cell(tmp, newId, tmp.Id, tmp.CellColor, r.Next(0, 5)));
                    this.cellCollection.Add(newCell);

                    //В клетку-родителя записывается id новой клетки
                    tmp.ChildrenId.Value.Add(newId);
                    //tmp.Children++;
                }
            }
        }

        //Метод передвижения 
        public void MoveCell(Cell tmp, Size field, ElementController controller_el)
        {
            //циклы по типу do while не подойдут, ибо если клетка родится далеко за пределами поля, программа зависнет, пытаясь подобрать новую позицию для клетки
            //do
            //{
            //    tmp.PosX = r.Next((int)tmp.PosX - 3, (int)tmp.PosX - 4);
            //}
            //while (tmp.PosX < 0 || tmp.PosX > field.Width - tmp.SizeW);
            lock (this)
            {
                //Point wayPoint = controller_el.ElementFound(tmp);
                //Метод поиска микроэлемента
                Element wayElement = controller_el.ElementFound(tmp);

                //Если элемент не был найден
                if (wayElement.PosX == -1)
                {
                    //Хаотичное передвижение в пределах поля
                    tmp.PosX = r.Next((int)tmp.PosX - 3, (int)tmp.PosX + 4);
                    if (tmp.PosX < 0)
                        tmp.PosX = 0;
                    else if (tmp.PosX > field.Width - tmp.SizeW)
                        tmp.PosX = field.Width - tmp.SizeW;

                    tmp.PosY = r.Next((int)tmp.PosY - 3, (int)tmp.PosY + 4);
                    if (tmp.PosY < 0)
                        tmp.PosY = 0;
                    else if (tmp.PosY > field.Height - tmp.SizeH)
                        tmp.PosY = field.Height - tmp.SizeH;
                }
                else
                {
                    //Высчитывание расстояния x и y до микроэлемента
                    //int wayX = (int)Math.Abs(tmp.PosX - way.X);
                    int wayX = (int)(tmp.PosX - wayElement.PosX);
                    //int wayY = (int)Math.Abs(tmp.PosY - way.Y);
                    int wayY = (int)(tmp.PosY - wayElement.PosY);

                    //Если модуль х больше модуля у, то клетка будет идти по х
                    if (Math.Abs(wayX) > Math.Abs(wayY))
                    {
                        if (wayX > 0)
                            tmp.PosX -= 3;
                        else
                            tmp.PosX += 3;                       
                    }
                    else
                    {
                        if (wayY > 0)
                            tmp.PosY -= 3;
                        else
                            tmp.PosY += 3;
                    }

                    //Если клетка находится в непосредственной близости к элементу
                    if ((Math.Abs(tmp.PosX - wayElement.PosX) <= 3 || Math.Abs(tmp.PosX + tmp.SizeW - wayElement.PosX) <= 3) &&
                        (Math.Abs(tmp.PosY - wayElement.PosY) <= 3 || Math.Abs(tmp.PosY + tmp.SizeH - wayElement.PosY) <= 3))
                    {
                        //Метод поедания микроэлемента и увеличение клетки в размерах каждые три поедания микроэлемента
                        controller_el.ElementEat(tmp, wayElement);
                        if (++tmp.FoodEaten % 3 == 0)
                        {
                            tmp.SizeH++;
                            tmp.SizeW++;
                        }
                    }
                }
            }
        }

        //Метод генерации новой клетки по клику мыши
        public void GenerateNewCellClick(Point loc)
        {
            //if (cellCollection.Count() == 0)
            //Добавление новой клетки, у которой нет родителя (-1)
            this.cellCollection.Add(new Cell(loc.X, loc.Y, totalId++, -1, Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), r.Next(0, 5)));                
        }

        //Метод цикла жизни, который вызывает метод старения у каждой клетки
        public void LifeCycle(Size clientSize, ElementController controller_el)
        {
            lock(this){
                for (int i = 0; i < cellCollection.Count(); i++)
                {
                    cellCollection[i].Old();
                    //Если клетка умирает, то генерируются микроэлементы и она удаляется со списка
                    if (cellCollection[i].Age >= maxCellLife)
                    {
                        controller_el.GenerateNewElement((int)cellCollection[i].PosX, (int)cellCollection[i].PosY,
                                                         (int)cellCollection[i].SizeW, (int)cellCollection[i].SizeH,
                                                         clientSize);
                        //Remove(cellCollection[i]);
                        cellCollection.Remove(cellCollection[i]);
                    }
                } 
            }
        }
        //public void Remove(Cell cell) => this.cellCollection.Remove(cell);
    }
}


