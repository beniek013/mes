using System;
using Agh_Mes.Objects;
namespace Agh_Mes
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = Functions.GenerateGrid();
            grid.PrintInfo(); 
            foreach (var element in grid.elements) {
                /*element.CalculateMatrixH();
                //element.PrintMatrixH();
                element.CalculateMatrixC();*/
                //element.PrintMatrixC();
                var jakobian = new Jakobian();
                jakobian.calculateInterpolatedCoordinates(element);
                jakobian.calculateShapeFunctionsDerivatives();
                jakobian.calculateJacobian(element);
                
            }

            Console.ReadKey();
        }
    }
}
