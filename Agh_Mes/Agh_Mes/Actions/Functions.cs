using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Agh_Mes
{
    public class Functions
    {
        public static Grid GenerateGrid() {
            double height, width;
            int nH, nL;
            using (TextReader reader = File.OpenText("../../Files/inputs.txt")) {
                height = double.Parse(reader.ReadLine());
                width = double.Parse(reader.ReadLine());
                nH = int.Parse(reader.ReadLine());
                nL = int.Parse(reader.ReadLine());
            }
            var grid = new Grid();
            var nodeList = new List<Node>();
            var elementList = new List<Element>();
            for (int i = 0; i < nL; i++) {
                for (int j = 0; j < nH; j++) {
                    nodeList.Add(new Node(i * nH + j + 1 , 10, i, j));
                }
            }
            grid.nodes = nodeList;
            
            foreach (var node in nodeList)
            {
                if (nodeList.Any(x => x.id == node.id + 1)
                    && nodeList.Any(y => y.id == node.id + nH)
                    && nodeList.Any(z => z.id == node.id + nH + 1)
                    && node.id % nH != 0)
                {
                    var upNode = nodeList.Find(x => x.id == node.id + 1);
                    var leftNode = nodeList.Find(x => x.id == node.id + nH);
                    var leftUpNode = nodeList.Find(x => x.id == node.id + nH + 1);
                    elementList.Add(new Element(elementList.Count + 1, new Tuple<int, int, int, int>(node.id, leftNode.id, leftUpNode.id, upNode.id)));
                }
            }
            grid.elements = elementList;

            return grid;
        }
    }
}
