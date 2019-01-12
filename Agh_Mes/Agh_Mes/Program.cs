using System;
namespace Agh_Mes
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid();
            grid.CreateNet();
            grid.Aggregate();
            grid.CalculateTemperature();
            Console.ReadKey();
        }
    }
}
