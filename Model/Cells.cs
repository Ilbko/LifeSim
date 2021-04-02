using System.Collections;
using System.Collections.Generic;
using WindowsFormsApp2.Model.Base;

namespace WindowsFormsApp2.Model
{
    class Cells : IEnumerable
    {
        public List<Cell> cells { get; private set; } = new List<Cell>();
        public Cell this[int key] 
        {
            get => cells[key];
        }
        public int Count() => cells.Count;
        public void Add(Cell cell) => cells.Add(cell);
        public void Remove(Cell cell) => cells.Remove(cell);
        
        public IEnumerator GetEnumerator()
        {
            return cells.GetEnumerator();
        }
    }
}
