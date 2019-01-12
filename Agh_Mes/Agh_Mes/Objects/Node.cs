using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agh_Mes
{
    public class Node
    {
        public int id;
        public double temperature;
        public double x, y;
        public bool status;

        public void PrintInfo()
        {
            Console.WriteLine($"{id}. ({x}, {y}),  t: {temperature}, status: {status}");
        }

        public Node(int Id, double temp, double x0, double y0)
        {
            id = Id;
            temperature = temp;
            x = x0;
            y = y0;
            if (this.x == 0 || this.y == 0 || this.x == 3 || this.y == 3)
                this.status = true;
            else
                this.status = false;
        }
    }
}
