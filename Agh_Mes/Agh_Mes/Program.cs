using System;
namespace Agh_Mes
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = Functions.GenerateGrid();
            grid.PrintInfo();
            Console.ReadKey();
        }
    }
}
