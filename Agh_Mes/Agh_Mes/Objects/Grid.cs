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

        public double[,] globalH = new double[16,16];
        public double[,] globalHBC = new double[16,16];
        public double[,] globalC = new double[16,16];
        public double[] globalP = new double[16];
        public double[] PC = new double[16];
        public double[,] HC = new double[16,16];
        public double[] t0 = new double[16];
        public double[] t1 = new double[16];
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

        public void Aggregate()
        {
            double[] temp = new double[16];
            
            globalH = new double[16,16];
            globalHBC = new double[16,16];
            globalC = new double[16, 16];
            globalP = new double[16];

            for (int i = 0; i < 9; i++)
            {
                var element = elements[i];
                Jakobian jakobian = new Jakobian();
                jakobian.LiczWspolrzednePunktopwCalkowania(element);
                jakobian.LiczPochodneFunkcjiKształtu();
                jakobian.LiczJakobian(element);
                element.CalcluateH(jakobian);
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalH[element.nodess[j].id-1, element.nodess[k].id-1] += element.H[j, k];
                    }
                }
                element.CalulateC(jakobian);
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalC[element.nodess[j].id - 1, element.nodess[k].id - 1] += element.C[j, k];
                    }
                }
                element.CalculateHBC();
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        globalHBC[element.nodess[j].id - 1, element.nodess[k].id - 1] += element.HBC[j, k];
                    }
                }
                element.CalculateP();
                for (int j = 0; j < 4; j++) {
                    globalP[element.nodess[j].id - 1] += element.P[j];
                }

                t0 = new double[16];
                for (int j = 0; j < 4 * 4; j++)
                {
                    t0[j] = Constatns.initialTemperature; ;
                    temp[j] = 0;
                }
            }
            for (int i = 0; i < 4 * 4; i++) {
                for (int j = 0; j < 4 * 4; j++)
                {
                    globalH[i,j] += globalHBC[i,j] + (globalC[i,j] / Constatns.simulationStepTime);
                }
            }

        }

        public void PrintGlobalMatrixC()
        {
            for (int j = 0; j < 16; j++)
            {
                for (int k = 0; k < 16; k++)
                {
                    Console.Write($"{(int)(globalC[j,k])} ");
                }
                Console.WriteLine();
            }
        }

        public void CalculateTemperature()
        {
            for (int i = 0; i < Constatns.simulationTime; i += (int)Constatns.simulationStepTime)
            {
                foreach (var node in nodes)
                {
                    t0[nodes.IndexOf(node)] = node.temperature;
                }
                PC = (-Vector.Create(globalP) + (Matrix.Create(globalC) / Constatns.simulationStepTime) * t0).ToArray();
                t1 = Solver.gaussElimination(16, globalH, PC);
                foreach (var node in nodes)
                {
                    node.temperature = t1[nodes.IndexOf(node)];
                }
                Console.WriteLine($"Time[s] {i + Constatns.simulationStepTime} MinTemp: {t1.Min()} MaxTemp: {t1.Max()}");
            }
        }

        public static double[,] ConvertMatrix(double[] flat, int m, int n)
        {
            if (flat.Length != m * n)
            {
                throw new ArgumentException("Invalid length");
            }
            double[,] ret = new double[m, n];
            // BlockCopy uses byte lengths: a double is 8 bytes
            Buffer.BlockCopy(flat, 0, ret, 0, flat.Length * sizeof(double));
            return ret;
        }


        public void SetHeatedSurfaces()
        {

            for (int i = 0; i < 4 - 1; i++)
            {
                elements[i].isSurface[3] = 1;
            }

            for (int i = ((4 - 1) * (4 - 1)) - 1; i > ((4 - 1) * (4 - 1)) - 1 - (4 - 1); i--)
            {
                elements[i].isSurface[1] = 1;
            }

            for (int i = 0; i < 4 - 1; i++)
            {
                elements[i * (4 - 1)].isSurface[0] = 1;
            }

            for (int i = 0; i < 4 - 1; i++)
            {
                elements[i * (4 - 1) + (4 - 2)].isSurface[2] = 1;
            }
        }


        /* vec = new long double[nL * nH];
         for (int j = 0; j < nL * nH; j++)
         {
             vec[j] = initialTemperature;
             temp[j] = 0;
         }

         for (int i = 0; i < nL * nH; i++)
         {
             for (int j = 0; j < nL * nH; j++)
             {
                 temp[i] += ((globalMatrixC[i][j] / simulationStepTime) * vec[j]);
             }
         }

         VectorP tempVectorP;
         tempVectorP.alpha = alpha;
         tempVectorP.ambientTemperature = ambientTemperature;
         tempVectorP.CalculateVectorP(elementList[i]);

         modifyIndexes(i, tempVectorP);
     }
     for (int j = 0; j < nL * nH; j++)
     {
         vectorP[j] = globalVectorP[j];
         globalVectorP[j] = -globalVectorP[j] + temp[j];
         for (int k = 0; k < nL * nH; k++)
         {
             globalMatrixH[j][k] += globalMatrixHBC[j][k] + (globalMatrixC[j][k] / simulationStepTime);
         }
     }
 }*/
    }
}
