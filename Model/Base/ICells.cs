using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Model.Base
{
    interface ICells
    {
        void Add(Cell cell);
        void Remove(Cell cell);
        int Count();
        
    }
}
