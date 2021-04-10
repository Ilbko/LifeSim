using System.Collections;
using System.Collections.Generic;
using WindowsFormsApp2.Model.Base;
using System.Linq;

namespace WindowsFormsApp2.Model
{
    //Полиморфная оболочка для списка объектов класса клетки
    public class Cells : IEnumerable
    {
        //Список клеток с приватным сетом, что запрещает взаимодейстовать со списком как-либо помимо методов этого класса
        public List<Cell> cells { get; private set; } = new List<Cell>();
        //Индексатор для списка
        public Cell this[int key] 
        {
            get => cells[key];
        }
        public int Count() => cells.Count;
        public void Add(Cell cell) => cells.Add(cell);
        public void Remove(Cell cell) => cells.Remove(cell);

        public void RemoveAll(System.Predicate<Cell> match) => cells.RemoveAll(match);

        public void ForEach(System.Action<Cell> action) => cells.ForEach(action);

        public void Clear() => cells.Clear();

        public List<Cell> Where(System.Func<Cell, bool> predicate) => cells.Where(predicate).ToList();
        
        public IEnumerator GetEnumerator()
        {
            return cells.GetEnumerator();
        }
    }
}
