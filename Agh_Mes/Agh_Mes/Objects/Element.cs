using System;
using System.Collections.Generic;
using Agh_Mes.Actions;
using Agh_Mes.Objects;

namespace Agh_Mes
{
    public class Element
    {
        private readonly int id;
        public int K;
        public List<Node> nodes;
        public double[] isSurface = new double[4] { 0, 0, 0, 0 };

        public double[,] H = new double[(int)Constatns.nH, (int)Constatns.nH];
        public double[] P = new double[(int)Constatns.nH];
        public double[,] C = new double[(int)Constatns.nH, (int)Constatns.nH];
        public double[,] dNdx = new double[4, 4];
        public double[,] dNdy = new double[4, 4];
        public double[,,] dNdxT = new double[4, 4, 4];
        public double[,,] dNdyT = new double[4, 4, 4];
        public double[,,] dNdxTdetJ = new double[4, 4, 4];
        public double[,,] dNdyTdetJ = new double[4, 4, 4];
        public double[,,] sum = new double[4, 4, 4];

        public double[,] HBC = new double[4, 4];

        public Element(int Id, List<Node> nodes)
        {
            id = Id;
            this.nodes = nodes;
            K = Constatns.k;
        }

        public void CalcluateH(Jakobian jakobian)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) {
                    dNdx[j,i] = jakobian.invertedJacobian[j,0] * jakobian.dNdksi[i,j] + jakobian.invertedJacobian[j, 1] * jakobian.dNdeta[i, j];
                    dNdy[j,i] = jakobian.invertedJacobian[j,2] * jakobian.dNdksi[i,j] + jakobian.invertedJacobian[j, 3] * jakobian.dNdeta[i, j];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        dNdxT[i, z, j] = dNdx[i, j] * dNdx[i, z];
                        dNdyT[i, z, j] = dNdy[i, j] * dNdy[i, z];

                        dNdxTdetJ[i, z, j] = dNdxT[i, z, j] * jakobian.detJacobian[i];
                        dNdyTdetJ[i, z, j] = dNdyT[i, z, j] * jakobian.detJacobian[i];

                        sum[i, z, j] = (dNdxTdetJ[i, z, j] + dNdyTdetJ[i, z, j]) * K;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    H[j, i] = sum[0, j, i] + sum[1, j, i] + sum[2, j, i] + sum[3, j, i];
                }
            }

        }

        public void CalulateC(Jakobian jakobian)
        {
            double[,,] fourMatrixs = new double[4, 4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    fourMatrixs[i, j, 0] = jakobian.N[0, j] * jakobian.N[0, i] * jakobian.detJacobian[0] * Constatns.c * Constatns.ro;
                    fourMatrixs[i, j, 1] = jakobian.N[1, j] * jakobian.N[1, i] * jakobian.detJacobian[1] * Constatns.c * Constatns.ro;
                    fourMatrixs[i, j, 2] = jakobian.N[2, j] * jakobian.N[2, i] * jakobian.detJacobian[2] * Constatns.c * Constatns.ro;
                    fourMatrixs[i, j, 3] = jakobian.N[3, j] * jakobian.N[3, i] * jakobian.detJacobian[3] * Constatns.c * Constatns.ro;

                    for (int k = 0; k < 4; k++)
                    {
                        C[i, j] += fourMatrixs[i, j, k];
                    }
                }
            }
        }

        public void CalculateHBC()
        {
            double[] pc1 = new double[4];
            double[] pc2 = new double[4];
            double[] length = new double[4];//długośc boku
            double[] detJ = new double[4];

            double[,,] sum = new double[4, 4, 4];
            double[,,] IntegrationPoints = new double[4, 2, 2]
            {
                { {-Constatns.jp3,-1 },{Constatns.jp3,-1 } },
                { {1,-Constatns.jp3 },{1,Constatns.jp3 } },
                { {Constatns.jp3,1 },{-Constatns.jp3,1 } },
                { {-1,Constatns.jp3 },{-1,-Constatns.jp3 } }
            };

            length[0] = Math.Sqrt(Math.Pow(nodes[1].x - nodes[0].x, 2) + Math.Pow(nodes[1].y - nodes[0].y, 2));
            length[1] = Math.Sqrt(Math.Pow(nodes[1].x - nodes[2].x, 2) + Math.Pow(nodes[1].y - nodes[2].y, 2));
            length[2] = Math.Sqrt(Math.Pow(nodes[2].x - nodes[3].x, 2) + Math.Pow(nodes[2].y - nodes[3].y, 2));
            length[3] = Math.Sqrt(Math.Pow(nodes[0].x - nodes[3].x, 2) + Math.Pow(nodes[0].y - nodes[3].y, 2));

            for (int i = 0; i < 4; i++) {
                detJ[i] = length[i] / 2;
            }

            for (int i = 0; i < 4; i++)
            {
                pc1[0] = Functions.N1(IntegrationPoints[i, 0, 0], IntegrationPoints[i, 0, 1]);
                pc1[1] = Functions.N2(IntegrationPoints[i, 0, 0], IntegrationPoints[i, 0, 1]);
                pc1[2] = Functions.N3(IntegrationPoints[i, 0, 0], IntegrationPoints[i, 0, 1]);
                pc1[3] = Functions.N4(IntegrationPoints[i, 0, 0], IntegrationPoints[i, 0, 1]);

                pc2[0] = Functions.N1(IntegrationPoints[i, 1, 0], IntegrationPoints[i, 1, 1]);
                pc2[1] = Functions.N2(IntegrationPoints[i, 1, 0], IntegrationPoints[i, 1, 1]);
                pc2[2] = Functions.N3(IntegrationPoints[i, 1, 0], IntegrationPoints[i, 1, 1]);
                pc2[3] = Functions.N4(IntegrationPoints[i, 1, 0], IntegrationPoints[i, 1, 1]);

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        sum[i, j, k] = ((Constatns.alpha * pc1[j] * pc1[k]) + (Constatns.alpha * pc2[j] * pc2[k])) * detJ[i];
                        HBC[j, k] += isSurface[i] * sum[i, j, k];
                    }
                }
            }
        }

        public void CalculateP() {
            double[] vector_p = new double[4];
            double[] vector_p_sum = new double[4];
            var vector_p_multiply = new List<double[]>();
            double[] P_sum = new double[4];
            double[] length = new double[4];
            double[] detJ = new double[4];
            double[,] PowPc = new double[8, 2]
            {
                {-Constatns.jp3,-1 },
                {Constatns.jp3, -1},
                {1, -Constatns.jp3 },
                {1, Constatns.jp3},
                {Constatns.jp3,1 },
                {-Constatns.jp3,1 },
                {-1,Constatns.jp3 },
                {-1,-Constatns.jp3 } 
            };

            length[0] = Math.Sqrt(Math.Pow(nodes[1].x - nodes[0].x, 2) + Math.Pow(nodes[1].y - nodes[0].y, 2));
            length[1] = Math.Sqrt(Math.Pow(nodes[1].x - nodes[2].x, 2) + Math.Pow(nodes[1].y - nodes[2].y, 2));
            length[2] = Math.Sqrt(Math.Pow(nodes[2].x - nodes[3].x, 2) + Math.Pow(nodes[2].y - nodes[3].y, 2));
            length[3] = Math.Sqrt(Math.Pow(nodes[0].x - nodes[3].x, 2) + Math.Pow(nodes[0].y - nodes[3].y, 2));

            for (int i = 0; i < 4; i++)
            {
                detJ[i] = length[i] / 2;
            }

            for (int i = 0; i < 4; i++)
            {
                vector_p_sum = new double[4];
                for (int j = 0; j < 2; j++)
                {
                    vector_p[0] = Functions.N1(PowPc[i*2+j, 0], PowPc[i*2+j, 1]);
                    vector_p[1] = Functions.N2(PowPc[i*2+j, 0], PowPc[i*2+j, 1]);
                    vector_p[2] = Functions.N3(PowPc[i*2+j, 0], PowPc[i*2+j, 1]);
                    vector_p[3] = Functions.N4(PowPc[i*2+j, 0], PowPc[i*2+j, 1]);

                    for (int k = 0; k < 4; k++)
                    {
                        vector_p_sum[k] += vector_p[k];
                    }
                }
                for (int k = 0; k < 4; k++)
                {
                    vector_p_sum[k] *= detJ[k] * isSurface[i] * (Constatns.alpha) * -Constatns.ambientTemperature;
                }
                vector_p_multiply.Add(vector_p_sum);
            }
            P = new double[4];
            for (int i = 0; i < 4; i++) {
                for (int k = 0; k < 4; k++)
                {
                    P[i] += vector_p_multiply[k][i];
                }
            }
        }
    }
}