using System;
using Agh_Mes.Objects;
namespace Agh_Mes
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = Functions.GenerateGrid();
            //grid.PrintInfo();
            /* foreach (var element in grid.elements) {
                 element.CalculateMatrixH();
                 //element.PrintMatrixH();
                 element.CalculateMatrixC();
                 //element.PrintMatrixC();
                 var jakobian = new Jakobian();
                 jakobian.LiczWspolrzednePunktopwCalkowania(element);
                 jakobian.LiczPochodneFunkcjiKształtu();
                 jakobian.LiczJakobian(element);
                 element.CalcluateH(jakobian);
                 element.CalulateC(jakobian);
                 element.CalculateHBC();
             }*/

            grid.Aggregate();
            grid.CalculateTemperature();
            //grid.printGlobalMatrixC();
            Console.ReadKey();
        }
    }
}
