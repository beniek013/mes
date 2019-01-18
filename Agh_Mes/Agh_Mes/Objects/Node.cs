using Agh_Mes.Actions;

namespace Agh_Mes
{
    public class Node
    {
        public int id;
        public double temperature;
        public double x, y;
        public bool isHeated = true;
        public Node(int Id, double temp, double x0, double y0)
        {
            id = Id;
            temperature = temp;
            x = x0;
            y = y0;
            if (x == 0.0 || y == 0.0 || x == (int)Constatns.h || y == (int)Constatns.w)
                isHeated = true;
            else isHeated = false;
        }
    }
}
