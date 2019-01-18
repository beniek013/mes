using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agh_Mes.Objects
{
    public static class Solver
    {
        public static double[] GaussElimination(int n, double[,] gik, double[] rok)
        {
            bool r = false;
            double m, s, e;
            e = Math.Pow(10, -12);
            double[] tabResult = new double[n];

            double[,] tabAB = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tabAB[j, i] = gik[j, i];
                }
            }
            for (int i = 0; i < n; i++)
            {
                tabAB[i, n] = rok[i];
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(tabAB[i, i]) < e)
                    {
                        throw new Exception("dzielnik rowny 0");
                    }
                    m = -tabAB[j, i] / tabAB[i, i];
                    for (int k = 0; k < n + 1; k++)
                    {
                        tabAB[j, k] += m * tabAB[i, k];
                    }
                }
            }
            for (int i = n - 1; i >= 0; i--)
            {
                s = tabAB[i, n];
                for (int j = n - 1; j >= 0; j--)
                {
                    s -= tabAB[i, j] * tabResult[j];
                }
                if (Math.Abs(tabAB[i, i]) < e)
                {
                    throw new Exception("dzielnik rowny 0");
                }
                tabResult[i] = s / tabAB[i, i];
                r = true;
            }
            return tabResult;
        }
    }
}