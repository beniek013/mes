using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agh_Mes
{
    public class Element
    {
        public int id;
        public string K; // współczynnik przewodzenia

        public Tuple<int, int, int, int> nodeIds;

        public void PrintInfo()
        {
            Console.WriteLine($"{id}. ({nodeIds.Item1}, {nodeIds.Item2}, {nodeIds.Item3}, {nodeIds.Item4}), {K}");
        }
        
        public Element(int Id, Tuple<int,int,int,int> nodes) {
            id = Id;
            nodeIds = nodes;
        }

    }
}
