using System;
using System.Collections.Generic;
using System.Linq;
using Agh_Mes.Objects;
using Agh_Mes.Actions;
//using Math = MathNet.Numerics.LinearAlgebra;
using Extreme.Mathematics;
namespace Agh_Mes
{
    public class Grid
    {
        public List<Node> nodes;
        public List<Element> elements;
        
        public double[,] globalH = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
        public double[,] globalHBC = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
        public double[,] globalC = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
        public double[] globalP = new double[(int)Constatns.nHnW];
        public double[] PC = new double[(int)Constatns.nHnW];
        public double[,] HC = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
        public double[] t0 = new double[(int)Constatns.nHnW];
        public double[] t1 = new double[(int)Constatns.nHnW];

        public void CreateNet()
        {
            nodes = new List<Node>();
            elements = new List<Element>();
            for (int i = 0; i < (int)Constatns.nH; i++)
            {
                for (int j = 0; j < (int)Constatns.nW; j++)
                {
                    nodes.Add(new Node(i * (int)Constatns.nH + j + 1, Constatns.initialTemperature, i * (Constatns.w / ((int)Constatns.nW - 1)), j * (Constatns.h / ((int)Constatns.nH - 1))));
                }
            }
            foreach (var node in nodes)
            {
                if (nodes.Any(x => x.id == node.id + 1)
                    && nodes.Any(y => y.id == node.id + (int)Constatns.nH)
                    && nodes.Any(z => z.id == node.id + (int)Constatns.nH + 1)
                    && node.id % (int)Constatns.nH != 0)
                {
                    var upNode = nodes.Find(x => x.id == node.id + 1);
                    var leftNode = nodes.Find(x => x.id == node.id + (int)Constatns.nH);
                    var leftUpNode = nodes.Find(x => x.id == node.id + (int)Constatns.nH + 1);
                    elements.Add(new Element(elements.Count + 1, new List<Node> { node, leftNode, leftUpNode, upNode }));
                }
            }
            SetHeatedSurfaces();
        }
        public void Aggregate()
        {
            
            globalH = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
            globalHBC = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
            globalC = new double[(int)Constatns.nHnW, (int)Constatns.nHnW];
            globalP = new double[(int)Constatns.nHnW];

            foreach(var element in elements)
            {
                Jakobian jakobian = new Jakobian();
                jakobian.CaclulateShapeFuncions();
                jakobian.CalulateJakobian(element);
                element.CalcluateH(jakobian);
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalH[element.nodes[j].id-1, element.nodes[k].id-1] += element.H[j, k];
                    }
                }
                element.CalulateC(jakobian);
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalC[element.nodes[j].id - 1, element.nodes[k].id - 1] += element.C[j, k];
                    }
                }
                element.CalculateHBC();
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalHBC[element.nodes[j].id - 1, element.nodes[k].id - 1] += element.HBC[j, k];
                    }
                }
                element.CalculateP();
                for (int j = 0; j < 4; j++) {
                    globalP[element.nodes[j].id - 1] += element.P[j];
                }

                t0 = new double[(int)Constatns.nHnW];
                for (int j = 0; j < Constatns.nHnW; j++)
                {
                    t0[j] = Constatns.initialTemperature; ;
                }
            }
        }
        public void CalculateTemperature()
        {
            for (int j = 0; j < Constatns.nHnW; j++)
            {
                for (int k = 0; k < Constatns.nHnW; k++)
                {
                    globalH[j, k] += globalHBC[j, k] + (globalC[j, k] / Constatns.simulationStepTime);
                }
            }
            for (int i = 0; i < Constatns.simulationTime; i += (int)Constatns.simulationStepTime)
            {
                foreach (var node in nodes)
                {
                    t0[nodes.IndexOf(node)] = node.temperature;
                }
                PC = (-Vector.Create(globalP) + (Matrix.Create(globalC) / Constatns.simulationStepTime) * t0).ToArray();
                t1 = Solver.GaussElimination((int)Constatns.nHnW, globalH, PC);
                foreach (var node in nodes)
                {
                    node.temperature = t1[nodes.IndexOf(node)];
                }
                Console.WriteLine($"Time[s] {i + Constatns.simulationStepTime} MinTemp: {t1.Min()} MaxTemp: {t1.Max()}");
            }
        }
        private void SetHeatedSurfaces() {
            foreach (var element in elements) {
                if (element.nodes[0].y == 0 && element.nodes[1].y == 0)
                    element.isSurface[0] = 1;
                if (element.nodes[1].x == Constatns.w && element.nodes[2].x == Constatns.w)
                    element.isSurface[1] = 1;
                if (element.nodes[2].y == Constatns.h && element.nodes[3].y == Constatns.h)
                    element.isSurface[2] = 1;
                if (element.nodes[3].x == 0 && element.nodes[0].x == 0)
                    element.isSurface[3] = 1;
            }
        }
    }
}
