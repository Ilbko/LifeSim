using System;
using WindowsFormsApp2.Model;
using WindowsFormsApp2.Model.Base;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace WindowsFormsApp2.Controller
{
    public class CellController
    {
        public Cells cellCollection { get; private set; } = new Cells();
        //public ElementController controller_el { get; private set; }
        protected Random r = new Random();
        protected readonly int radius = 25;
        protected readonly int minCellCount = 2;
        public readonly int maxCellLife = 20;
        protected int totalId = 0;

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

        public void GenerateNewCell(Cell tmp)
        {
            lock (this)
            {
                if (tmp.ChildrenId.Value.Count() < tmp.MaxChildren)
                {
                    int newId = totalId++;
                    Cell newCell = new Cell(tmp, newId, tmp.Id, tmp.CellColor, r.Next(0, 5));

                    do
                    {
                        if (r.Next((int)newCell.SizeH * radius) < (((int)newCell.SizeH * radius) / 2))
                            newCell.PosX += newCell.SizeH;
                        else
                            newCell.PosX -= newCell.SizeH;
                        if (r.Next((int)newCell.SizeW * radius) < (((int)newCell.SizeW * radius) / 2))
                            newCell.PosY += newCell.SizeW;
                        else
                            newCell.PosY -= newCell.SizeW;
                    } while (this.cellCollection.cells.Any(x => x.isSamePosition(newCell)));

                    //this.cellCollection.Add(new Cell(tmp, newId, tmp.Id, tmp.CellColor, r.Next(0, 5)));
                    this.cellCollection.Add(newCell);

                    tmp.ChildrenId.Value.Add(newId);
                    //tmp.Children++;
                }
            }
        }

        public void MoveCell(Cell tmp, Size field)
        {
            //циклы по типу do while не подойдут, ибо если клетка родится далеко за пределами поля, программа зависнет
            //do
            //{
            //    tmp.PosX = r.Next((int)tmp.PosX - 3, (int)tmp.PosX - 4);
            //}
            //while (tmp.PosX < 0 || tmp.PosX > field.Width - tmp.SizeW);
            lock (this)
            {
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
        }

        public void GenerateNewCellClick(Point loc)
        {
             //if (cellCollection.Count() == 0)
                 this.cellCollection.Add(new Cell(loc.X, loc.Y, totalId++, 0, Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), r.Next(3, 5)));                
        }
        public void LifeCycle(Size clientSize, ElementController controller_el)
        {
            lock(this){
                for (int i = 0; i < cellCollection.Count(); i++)
                {
                    cellCollection[i].Old();
                    if (cellCollection[i].Age >= maxCellLife)
                    {
                        controller_el.GenerateNewElement((int)cellCollection[i].PosX, (int)cellCollection[i].PosY,
                                                         (int)cellCollection[i].SizeW, (int)cellCollection[i].SizeH,
                                                         clientSize);
                        Remove(cellCollection[i]);
                    }
                } 
            }
        }
        public void KillCell(int index = 0)
        {
            if (cellCollection.Count() < minCellCount) return;

            cellCollection.cells.Remove((Cell)this.cellCollection[index]);
        }
        public void Add(Cell cell) => this.cellCollection.Add(cell);
        public void Remove(Cell cell) => this.cellCollection.Remove(cell);
    }
}
