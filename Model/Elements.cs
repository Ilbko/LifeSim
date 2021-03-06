using System.Collections;
using System.Collections.Generic;
using WindowsFormsApp2.Model.Base;

namespace WindowsFormsApp2.Model
{
    //Класс полиморфической оболочки для списка микроэлементов
    public class Elements : IEnumerable
    {
        public List<Element> elements { get; private set; } = new List<Element>();

        public Element this[int key]
        {
            get => elements[key];
        }

        public int Count() => elements.Count;

        public void Add(Element element) => elements.Add(element);
        public void Remove(Element element) => elements.Remove(element);

        public void Clear() => elements.Clear();

        public IEnumerator GetEnumerator() => elements.GetEnumerator();
    }
}
