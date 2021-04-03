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

                    do
                    {
                        newElement.PosX += r.Next(CellPosX - radius, CellPosX - (int)newElement.SizeW + radius);
                        newElement.PosY += r.Next(CellPosY - radius, CellPosY - (int)newElement.SizeH + radius);
                    } while (newElement.PosX < 0 || newElement.PosX > clientSize.Width - newElement.SizeW ||
                             newElement.PosY < 0 || newElement.PosY > clientSize.Height - newElement.SizeH);

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
