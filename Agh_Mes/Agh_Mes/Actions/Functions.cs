using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agh_Mes.Actions;

namespace Agh_Mes
{
    public static class Functions
    {

        public static double N1(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 - eta);
        }

        public static double N2(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 - eta);
        }

        public static double N3(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 + eta);
        }

        public static double N4(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 + eta);
        }

        public static double dN1dKsi(double eta)
        {
            return -0.25 * (1 - eta);
        }

        public static double dN2dKsi(double eta)
        {
            return 0.25 * (1 - eta);
        }

        public static double dN3dKsi(double eta)
        {
            return 0.25 * (1 + eta);
        }

        public static double dN4dKsi(double eta)
        {
            return -0.25 * (1 + eta);
        }

        public static double dN1dEta(double ksi)
        {
            return -0.25 * (1 - ksi);
        }

        public static double dN2dEta(double ksi)
        {
            return -0.25 * (1 + ksi);
        }

        public static double dN3dEta(double ksi)
        {
            return 0.25 * (1 + ksi);
        }

        public static double dN4dEta(double ksi)
        {
            return 0.25 * (1 - ksi);
        }

    }
}
