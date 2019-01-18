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
        public double[,] invertedJacobian = new double[4, 4];

        public Jakobian()
        {
            ksi[0] = ksi[3] = eta[3] = eta[2] = Constatns.jp3;
            ksi[1] = ksi[2] = eta[0] = eta[1] = -Constatns.jp3;

            for (int i = 0; i < 4; i++)
            {
                N[i, 0] = Functions.N1(ksi[i], eta[i]);
                N[i, 1] = Functions.N2(ksi[i], eta[i]);
                N[i, 2] = Functions.N3(ksi[i], eta[i]);
                N[i, 3] = Functions.N4(ksi[i], eta[i]);
            }
            CaclulateShapeFuncions();
        }

        private void CaclulateShapeFuncions()
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

        public void CalulateJakobian(Element element)
        {
            for (int i = 0; i < 4; i++)
            {
                jacobian[i, 0] = element.nodes[0].x * dNdksi[0, i] + element.nodes[1].x * dNdksi[1, i] + element.nodes[2].x * dNdksi[2, i] + element.nodes[3].x * dNdksi[3, i];
                jacobian[i, 1] = element.nodes[0].y * dNdksi[0, i] + element.nodes[1].y * dNdksi[1, i] + element.nodes[2].y * dNdksi[2, i] + element.nodes[3].y * dNdksi[3, i];
                jacobian[i, 2] = element.nodes[0].x * dNdeta[0, i] + element.nodes[1].x * dNdeta[1, i] + element.nodes[2].x * dNdeta[2, i] + element.nodes[3].x * dNdeta[3, i];
                jacobian[i, 3] = element.nodes[0].y * dNdeta[0, i] + element.nodes[1].y * dNdeta[1, i] + element.nodes[2].y * dNdeta[2, i] + element.nodes[3].y * dNdeta[3, i];

                detJacobian[i] = jacobian[i, 0] * jacobian[i, 3] - jacobian[i, 1] * jacobian[i, 2];

                invertedJacobian[i, 0] = jacobian[i, 0] / detJacobian[i];
                invertedJacobian[i, 1] = -(jacobian[i, 1] / detJacobian[i]);
                invertedJacobian[i, 2] = -(jacobian[i, 2] / detJacobian[i]);
                invertedJacobian[i, 3] = jacobian[i, 3] / detJacobian[i];
            }
        }
    }
}
