using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agh_Mes.Actions;

namespace Agh_Mes
{
    public static class Functions
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
                    nodeList.Add(new Node(i * nH + j + 1 , Constatns.initialTemperature, i * (Constatns.w/(Constatns.nW - 1)), j * (Constatns.h / (Constatns.nH - 1))));
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
                    elementList.Add(new Element(elementList.Count + 1, new List<Node> { node, leftNode, leftUpNode, upNode}));
                }
            }
            grid.elements = elementList;
            grid.SetHeatedSurfaces();
            return grid;
        }

        public static double N1(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 - eta);
        }

        public static double N2(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 - eta);
        }

        public static double N3(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 + eta);
        }

        public static double N4(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 + eta);
        }

        public static double dN1dKsi(double eta)
        {
            return -0.25 * (1 - eta);
        }

        public static double dN2dKsi(double eta)
        {
            return 0.25 * (1 - eta);
        }

        public static double dN3dKsi(double eta)
        {
            return 0.25 * (1 + eta);
        }

        public static double dN4dKsi(double eta)
        {
            return -0.25 * (1 + eta);
        }

        public static double dN1dEta(double ksi)
        {
            return -0.25 * (1 - ksi);
        }

        public static double dN2dEta(double ksi)
        {
            return -0.25 * (1 + ksi);
        }

        public static double dN3dEta(double ksi)
        {
            return 0.25 * (1 + ksi);
        }

        public static double dN4dEta(double ksi)
        {
            return 0.25 * (1 - ksi);
        }

    }
}
