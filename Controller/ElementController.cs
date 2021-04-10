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
        public readonly int maxElementAge = 60;

        //Метод поиска элемента
        public Element ElementFound(Cell tmp)
        {
            bool found = false;
            int distance;
            radius = (tmp.SizeH + tmp.SizeW) * (int)Math.PI;
            //Словарь, хранящий себе дистанцию к элементу от клетки и сам элемент
            Dictionary<int, Element> elements = new Dictionary<int, Element>();

            //К моменту обращения элемент может уже не существовать, поэтому цикл обёрнут в try catch.
            try
            {
                foreach (Element item in elementCollection)
                {
                    //Если элемент в радиусе видимости был найден
                    if (item.PosX > tmp.PosX - radius && item.PosX < tmp.PosX + tmp.SizeW + radius &&
                        item.PosY > tmp.PosY - radius && item.PosY < tmp.PosY + tmp.SizeH + radius)
                    {
                        found = true;
                        //Расчёт дистанции с помощью модулей
                        distance = (int)(Math.Abs(item.PosX - tmp.PosX) + Math.Abs(item.PosY - tmp.PosY));

                        try
                        {
                            elements.Add(distance, item);
                        }
                        catch (System.ArgumentException) { }
                    }
                }
            } catch(System.InvalidOperationException) { }

            Element tmp_el = new Element();
            //Если элемент не был найден, то локация элемента устанавливается данными числами
            if (!found)
            {
                tmp_el.PosX = -1;
                tmp_el.PosY = -1;
            }
            else
            {
                //Выбирается минимальный с ключей словаря дистанций и элементов
                int min = elements.Min(x => x.Key);
                //Достаётся элемент с минимальной дистанцией
                Dictionary<int, Element> dictionary_el = elements.Where(x => x.Key == min).ToDictionary(x => x.Key, x => x.Value);
                //Локация элемента устанавливается
                tmp_el = dictionary_el.Values.ElementAt(0);
            }

            elements.Clear();
            return tmp_el;
        }

        //Метод поедания элемента
        public void ElementEat(Cell cell, Element element)
        {
            cell.Age -= 4;
            if (cell.Age < 0)
                cell.Age = 0;

            elementCollection.Remove(element);
        }

        //Метод генерации новых элементов с учётом границ окна
        public void GenerateNewElement(int CellPosX, int CellPosY, int CellSizeW, int CellSizeH, Size clientSize)
        {
            lock (this)
            {
                //слишком большой радиус
                //radius = CellSizeH * CellSizeW * (int)Math.PI;
                radius = (CellSizeH + CellSizeW) * (int)Math.PI;

                //Спавнится от одного до четырёх элементов
                for (int i = 0; i < r.Next(1, 5); i++)
                {
                    Element newElement = new Element();

                    //Определяется позиция элемента с проверками пределов поля
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

        //Метод старения элементов
        public void AgeElement()
        {
            lock (this)
            {
                for (int i = 0; i < elementCollection.Count(); i++)
                {
                    elementCollection[i].Old();
                    if (elementCollection[i].Age >= maxElementAge)
                        elementCollection.Remove(elementCollection[i]);
                        //Remove(elementCollection[i]);
                }
            }
        }

        //public void Remove(Element element) => this.elementCollection.Remove(element);
    }
}
