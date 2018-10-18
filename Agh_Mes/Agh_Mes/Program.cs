using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
