using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agh_Mes.Actions;

namespace Agh_Mes.Objects
{
    public class Jakobian
    {
        public double[,] N = new double[4, 4];
        public double[] ksi = new double[4];
        public double[] eta = new double[4];
        public double[,] InterpolatedCoordinates = new double[4, 2];
        public double[,] dNdksi = new double[4, 4];
        public double[,] dNdeta = new double[4, 4];
        public double[,] jacobian = new double[4, 4];
        public double[] detJacobian = new double[4];
        public double[,] inversedJacobian = new double[4, 4];

        public Jakobian()
        {
            ksi[0] = ksi[3] = -Constatns.jp3;
            ksi[1] = ksi[2] = Constatns.jp3;

            eta[0] = eta[1] = -Constatns.jp3;
            eta[3] = eta[2] = Constatns.jp3;

            for (int i = 0; i < 4; i++)
            {
                N[i, 0] = Functions.N1(ksi[i], eta[i]);
                N[i, 1] = Functions.N2(ksi[i], eta[i]);
                N[i, 2] = Functions.N3(ksi[i], eta[i]);
                N[i, 3] = Functions.N4(ksi[i], eta[i]);
            }
        }

        public void LiczWspolrzednePunktopwCalkowania(Element element)
        {
            for (int i = 0; i < 4; i++)
            {
                InterpolatedCoordinates[i, 0] = 
                    N[i, 0] * element.nodess[0].x + 
                    N[i, 1] * element.nodess[1].x + 
                    N[i, 2] * element.nodess[2].x + 
                    N[i, 3] * element.nodess[3].x;
                InterpolatedCoordinates[i, 1] = 
                    N[i, 0] * element.nodess[0].y + 
                    N[i, 1] * element.nodess[1].y + 
                    N[i, 2] * element.nodess[2].y + 
                    N[i, 3] * element.nodess[3].y;
            }
        }

        public void LiczPochodneFunkcjiKształtu()
        {
            for (int i = 0; i < 4; i++)
            {
                dNdksi[0, i] = Functions.dN1dKsi(eta[i]);
                dNdksi[1, i] = Functions.dN2dKsi(eta[i]);
                dNdksi[2, i] = Functions.dN3dKsi(eta[i]);
                dNdksi[3, i] = Functions.dN4dKsi(eta[i]);

                dNdeta[0, i] = Functions.dN1dEta(ksi[i]);
                dNdeta[1, i] = Functions.dN2dEta(ksi[i]);
                dNdeta[2, i] = Functions.dN3dEta(ksi[i]);
                dNdeta[3, i] = Functions.dN4dEta(ksi[i]);
            }
        }

        public void LiczJakobian(Element element)
        {
            // i - punkt calkowania 
            // jacobian[i][0-1] - eta
            // jacobian[i][2-3] - ksi

            for (int i = 0; i < 4; i++)
            {
                jacobian[i, 0] = element.nodess[0].x * dNdksi[0, i] + element.nodess[1].x * dNdksi[1, i] + element.nodess[2].x *
                    dNdksi[2, i] + element.nodess[3].x * dNdksi[3, i];
                jacobian[i, 1] = element.nodess[0].y * dNdksi[0, i] + element.nodess[1].y * dNdksi[1, i] + element.nodess[2].y *
                    dNdksi[2, i] + element.nodess[3].y * dNdksi[3, i];
                jacobian[i, 2] = element.nodess[0].x * dNdeta[0, i] + element.nodess[1].x * dNdeta[1, i] + element.nodess[2].x *
                    dNdeta[2, i] + element.nodess[3].x * dNdeta[3, i];
                jacobian[i, 3] = element.nodess[0].y * dNdeta[0, i] + element.nodess[1].y * dNdeta[1, i] + element.nodess[2].y *
                    dNdeta[2, i] + element.nodess[3].y * dNdeta[3, i];

                detJacobian[i] = jacobian[i, 0] * jacobian[i, 3] - jacobian[i, 1] * jacobian[i, 2];
                inversedJacobian[i, 0] = jacobian[i, 3] / detJacobian[i];
                inversedJacobian[i, 1] = -(jacobian[i, 1] / detJacobian[i]);
                inversedJacobian[i, 2] = -(jacobian[i, 2] / detJacobian[i]);
                inversedJacobian[i, 3] = jacobian[i, 0] / detJacobian[i];
            }
        }
    }
}
