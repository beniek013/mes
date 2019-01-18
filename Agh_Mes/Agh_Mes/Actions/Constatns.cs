using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agh_Mes.Actions
{
    public class Constatns
    {
        public static double jp3 = 1 / Math.Sqrt(3);
        public static double c = 700;   //specific heat ciepło właściwe
        public static double ro = 7800; //density gęstość
        public static double alpha = 300;   //
        public static int k = 25;
        public static double ambientTemperature = 1200;
        public static double initialTemperature = 100;
        public static double simulationStepTime = 50;   //1
        public static double simulationTime = 500;      //100
        public static double h = 0.1;
        public static double w = 0.1;
        public static double nH = 4;       //31
        public static double nW = 4;       //31
        public static double nHnW = nH * nW; //szerokosc * wysokosc
    }
    
}
