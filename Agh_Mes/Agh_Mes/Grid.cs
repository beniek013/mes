using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agh_Mes
{
    public class Grid
    {
        public List<Node> nodes;
        public List<Element> elements;

        public void PrintInfo() {
            Console.WriteLine("\nNodes: ");
            if (nodes.Any())
            {
                foreach (var node in nodes)
                {
                    node.PrintInfo();
                }
            }
            Console.WriteLine("\nElements: ");
            if (elements.Any()) {
                foreach (var element in elements)
                {
                    element.PrintInfo();
                }
            }
            Console.WriteLine($"\nNodes: {nodes.Count}, Elements: {elements.Count}");
        }
    }
}
