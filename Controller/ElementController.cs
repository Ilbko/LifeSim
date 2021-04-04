using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Model;
using WindowsFormsApp2.Model.Base;

namespace WindowsFormsApp2.Controller
{
    public class ElementController
    {
        public Elements elementCollection { get; private set; } = new Elements();
        protected Random r = new Random();
        protected int radius;
        protected int maxElementAge = 60;

        public Element ElementFound(Cell tmp)
        {
            bool found = false;
            radius = (int)((tmp.SizeH / 3) * (tmp.SizeW / 3) * (int)Math.PI);
            Element tmp_el = new Element();

            foreach (Element item in elementCollection)
            {
                if (item.PosX > tmp.PosX - radius && item.PosX < tmp.PosX + tmp.SizeW + radius &&
                    item.PosY > tmp.PosY - radius && item.PosY < tmp.PosY + tmp.SizeH + radius)
                {
                    found = true;
                    tmp_el = item;
                    break;
                }
            }

            if (!found)
            {
                tmp_el.PosX = -1;
                tmp_el.PosY = -1;
            }

            return tmp_el;
            //    return new Point (tmp_el.PosX, tmp_el.PosY);
            
            //return new Point(-1, -1);
        }

        public void ElementEat(Cell cell, Element element)
        {
            cell.Age -= 4;
            if (cell.Age < 0)
                cell.Age = 0;

            elementCollection.Remove(element);
        }
        public void GenerateNewElement(int CellPosX, int CellPosY, int CellSizeW, int CellSizeH, Size clientSize)
        {
            lock(this)
            {
                //слишком большой радиус
                //radius = CellSizeH * CellSizeW * (int)Math.PI;
                radius = (CellSizeH / 5) * (CellSizeW / 5) * (int)Math.PI;

                for (int i = 0; i < r.Next(1, 5); i++)
                {
                    Element newElement = new Element();

                    newElement.PosX += r.Next(CellPosX - radius, CellPosX - (int)newElement.SizeW + radius);
                    if (newElement.PosX < 0)
                        newElement.PosX = 0;
                    else if (newElement.PosX > clientSize.Width - newElement.SizeW)
                        newElement.PosX = (int)(clientSize.Width - newElement.SizeW);

                    newElement.PosY += r.Next(CellPosY - radius, CellPosY - (int)newElement.SizeH + radius);
                    if (newElement.PosY < 0)
                        newElement.PosY = 0;
                    else if (newElement.PosY > clientSize.Height - newElement.SizeH)
                        newElement.PosY = (int)(clientSize.Height - newElement.SizeH);

                    this.elementCollection.Add(newElement);
                }
            }
        }

        public void AgeElement()
        {
            lock(this)
            {
                for (int i = 0; i < elementCollection.Count(); i++)
                {
                    elementCollection[i].Old();
                    if (elementCollection[i].Age >= maxElementAge)
                        Remove(elementCollection[i]);
                }
            }
        }

        public void Remove(Element element) => this.elementCollection.Remove(element);
    }
}
