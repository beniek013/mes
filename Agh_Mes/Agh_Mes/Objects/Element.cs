using System;

namespace Agh_Mes
{
    public class Element
    {
        private readonly int id;
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
