using System;
using System.Collections.Generic;
using Agh_Mes.Actions;

namespace Agh_Mes
{
    public class Element
    {
        private readonly int id;
        public int K; // współczynnik przewodzenia
        public List<Node> nodess;
        public List<KeyValuePair<double, double>> ksiEta;
        public List<List<double>> H;
        public void PrintInfo()
        {
            Console.WriteLine($"{id}. ({nodess[0].id}, {nodess[1].id}, {nodess[2].id}, {nodess[3].id}), {K}");
        }

        public void PrintMatrixH() {
            this.PrintInfo();
            var message = String.Empty;
            foreach (var list in H) {
                foreach (var dub in list)
                    message += $"{dub} ";
                message += "\n";
            }
            Console.WriteLine($"H: {message}");
        }

        public Element(int Id, List<Node> nodess)
        {
            id = Id;
            this.nodess = nodess;
            K = 30;
        }

        public void CalculateMatrxH() {
            var ksiEtaList = new List<KeyValuePair<double, double>>
            {
                new KeyValuePair<double, double>(-Constatns.jp3, -Constatns.jp3),
                new KeyValuePair<double, double>(Constatns.jp3, -Constatns.jp3),
                new KeyValuePair<double, double>(Constatns.jp3, Constatns.jp3),
                new KeyValuePair<double, double>(-Constatns.jp3, Constatns.jp3)
            };
            ksiEta = ksiEtaList;
            double ksi1, ksi2, ksi3, ksi4, eta1, eta2, eta3, eta4;
            double dxdksi, dxdeta, dydksi, dydeta, detJ;
            double j111, j112, j121, j122;
            double dn1dx, dn2dx, dn3dx, dn4dx;
            double dn1dy, dn2dy, dn3dy, dn4dy;
            List<double> detJList = new List<double>();
            List<List<double>> dndxList = new List<List<double>>();
            List<List<double>> dndyList = new List<List<double>>();
            List<List<List<double>>> konList = new List<List<List<double>>>();
            List<List<double>> Hh = new List<List<double>>();
            List<List<List<double>>> dndxTList = new List<List<List<double>>>();
            List<List<List<double>>> dndyTList = new List<List<List<double>>>();
            List<List<List<double>>> dndxdetJTList = new List<List<List<double>>>();
            List<List<List<double>>> dndydetJTList = new List<List<List<double>>>();
            foreach (var node in nodess) {
                //pochodne funkcje ksztaltu dla ksi
                ksi1 = -0.25 * (1 - ksiEtaList[nodess.IndexOf(node)].Value);
                ksi2 = 0.25 * (1 - ksiEtaList[nodess.IndexOf(node)].Value);
                ksi3 = 0.25 * (1 + ksiEtaList[nodess.IndexOf(node)].Value);
                ksi4 = -0.25 * (1 + ksiEtaList[nodess.IndexOf(node)].Value);
                //pochodne funkcji ksztaltu dla eta
                eta1 = -0.25 * (1 - ksiEtaList[nodess.IndexOf(node)].Key);
                eta2 = -0.25 * (1 + ksiEtaList[nodess.IndexOf(node)].Key);
                eta3 = 0.25 * (1 + ksiEtaList[nodess.IndexOf(node)].Key);
                eta4 = 0.25 * (1 - ksiEtaList[nodess.IndexOf(node)].Key);
                //
                dxdksi = ksi1 * nodess[0].x + ksi2 * nodess[1].x + ksi3 * nodess[2].x + ksi4 * nodess[3].x;
                dydksi = ksi1 * nodess[0].y + ksi2 * nodess[1].y + ksi3 * nodess[2].y + ksi4 * nodess[3].y;
                dxdeta = eta1 * nodess[0].x + eta2 * nodess[1].x + eta3 * nodess[2].x + eta4 * nodess[3].x;
                dydeta = eta1 * nodess[0].y + eta2 * nodess[1].y + eta3 * nodess[2].y + eta4 * nodess[3].y;
                //detJ
                detJ = dxdksi * dydeta - dydksi * dxdeta;
                detJList.Add(detJ);
                //
                j111 = dydeta / detJ;
                j112 = dydksi / detJ;
                j121 = dxdeta / detJ;
                j122 = dxdksi / detJ;
                //dndx
                dn1dx = ksi1 * j111 + eta1 * j112;
                dn2dx = ksi2 * j111 + eta2 * j112;
                dn3dx = ksi3 * j111 + eta3 * j112;
                dn4dx = ksi4 * j111 + eta4 * j112;
                var dndxPom = new List<double>() { dn1dx, dn2dx, dn3dx, dn4dx };
                dndxList.Add(dndxPom);
                //dndy
                dn1dy = ksi1 * j121 + eta1 * j122;
                dn2dy = ksi2 * j121 + eta2 * j122;
                dn3dy = ksi3 * j121 + eta3 * j122;
                dn4dy = ksi4 * j121 + eta4 * j122;
                var dndyPom = new List<double>() { dn1dy, dn2dy, dn3dy, dn4dy };
                dndyList.Add(dndyPom);
            }
            //{dN/dx}{dN/dx}T i {dN/dy}{dN/dy}T
            foreach (var punktCalkowaniaList in dndxList) {
                var dndxTpom2 = new List<List<double>>();
                foreach (var dub in punktCalkowaniaList) {
                    var dndxTPom = new List<double>();
                    dndxTPom.Add(dub * punktCalkowaniaList[0]);
                    dndxTPom.Add(dub * punktCalkowaniaList[1]);
                    dndxTPom.Add(dub * punktCalkowaniaList[2]);
                    dndxTPom.Add(dub * punktCalkowaniaList[3]);
                    dndxTpom2.Add(dndxTPom);
                }
                dndxTList.Add(dndxTpom2);
            }

            foreach (var punktCalkowaniaList in dndyList)
            {
                var dndyTpom2 = new List<List<double>>();
                foreach (var dub in punktCalkowaniaList)
                {
                    var dndyTPom = new List<double>();
                    dndyTPom.Add(dub * punktCalkowaniaList[0]);
                    dndyTPom.Add(dub * punktCalkowaniaList[1]);
                    dndyTPom.Add(dub * punktCalkowaniaList[2]);
                    dndyTPom.Add(dub * punktCalkowaniaList[3]);
                    dndyTpom2.Add(dndyTPom);
                }
                dndyTList.Add(dndyTpom2);
            }
            //{dN/dx}{dN/dx}T*DetJ

            foreach (var list2 in dndxTList) {
                var lvl2 = new List<List<double>>();
                foreach (var list in list2) {
                    var lvl3 = new List<double>();
                    foreach (var dub in list) {
                        lvl3.Add(dub * detJList[dndxTList.IndexOf(list2)]);
                    }
                    lvl2.Add(lvl3);
                }
                dndxdetJTList.Add(lvl2);
            }

            foreach (var list2 in dndyTList)
            {
                var lvl2 = new List<List<double>>();
                foreach (var list in list2)
                {
                    var lvl3 = new List<double>();
                    foreach (var dub in list)
                    {
                        lvl3.Add(dub * detJList[dndyTList.IndexOf(list2)]);
                    }
                    lvl2.Add(lvl3);
                }
                dndydetJTList.Add(lvl2);
            }
            //"K*(     {dN/dx}{dN/dx}T  +  {dN/dy}{dN/dy}T)*DetJ
            foreach (var list2 in dndxdetJTList) {
                var tempList2 = new List<List<double>>();
                foreach (var list in list2) {
                    var tempList = new List<double>();
                    foreach (var dub in list) {
                        var dndydetJListSameSpotMember = dndydetJTList[dndxdetJTList.IndexOf(list2)][list2.IndexOf(list)][list.IndexOf(dub)];
                        tempList.Add(K * (dndydetJListSameSpotMember + dub));
                    }
                    tempList2.Add(tempList);
                }
                konList.Add(tempList2);
            }
            //H
            for (int i = 0; i < konList.Count; i++)
            {
                var tempList = new List<double>();
                for (int j = 0; j < konList.Count; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < konList.Count; k++) {
                        temp += konList[k][i][j];
                    }
                    tempList.Add(temp);
                }
                Hh.Add(tempList);
            }
            H = Hh;
        }
    }
}
