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
            if (this.x == 0.0 || this.y == 0.0 || this.x == (int)Constatns.h || this.y == (int)Constatns.w)
                isHeated = true;
            else isHeated = false;
        }
    }
}
