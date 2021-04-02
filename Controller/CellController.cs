﻿using System;
using WindowsFormsApp2.Model;
using WindowsFormsApp2.Model.Base;
using System.Linq;
using System.Drawing;

namespace WindowsFormsApp2.Controller
{
    class CellController
    {
        public Cells cellCollection { get; private set; } = new Cells();
        protected Random r = new Random();
        protected readonly int radius = 25;
        protected readonly int minCellCount = 2;
        public readonly int maxCellLife = 4;

        public int Count { get => cellCollection.Count(); }
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
            if (tmp.Children < tmp.MaxChildren)
            {
                do
                {
                    if (r.Next((int)tmp.SizeH * radius) < (((int)tmp.SizeH * radius) / 2))
                        tmp.PosX += tmp.SizeH;
                    else
                        tmp.PosX -= tmp.SizeH;
                    if (r.Next((int)tmp.SizeW * radius) < (((int)tmp.SizeW * radius) / 2))
                        tmp.PosY += tmp.SizeW;
                    else
                        tmp.PosY -= tmp.SizeW;
                } while (this.cellCollection.Count() == 0 && this.cellCollection.cells.Any<Cell>(x => x.isSamePosition(tmp)));

                this.cellCollection.Add(new Cell(tmp, tmp.Id, tmp.ParentId, tmp.CellColor, r.Next(0, 3)));
                tmp.Children++;
            }
        }

        public void GenerateNewCellClick(Point loc)
        {
             //if (cellCollection.Count() == 0)
                 this.cellCollection.Add(new Cell(loc.X, loc.Y, 0, 0, Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), r.Next(0, 3)));                
        }
        public void LifeCicle()
        {
            lock(this){
                for (int i = 0; i < cellCollection.Count(); i++)
                {
                    cellCollection[i].Old();
                    if (cellCollection[i].Age >= maxCellLife)
                        Remove(cellCollection[i]);
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