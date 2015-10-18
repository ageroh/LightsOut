using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{

    public enum Light
    {
        Off = 0, On
    }

    public class Logic
    {
        public Cell[][] Table { get; set; }
        
        public Logic(int N)
        {
            Init(N);
            Table = MapToInterface(N);
            RandomSolutionInit(N);
        }

        private void Init(int n)
        {
            throw new NotImplementedException();
        }

        private bool[] MapToInterface(int n)
        {
            throw new NotImplementedException();
        }
        
        public Cell GetCellPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public Cell[] GetAdjacent(Cell cell)
        {
            throw new NotImplementedException();
        }
     }
}
