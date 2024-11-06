using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.Legends.Clear();
            chart1.ChartAreas[0].AxisX.RoundAxisValues();
            chart1.ChartAreas[0].AxisY.RoundAxisValues();
            dg.AllowUserToAddRows = false;
            ChartArea CA = chart1.ChartAreas[0];
            CA.AxisX.ScaleView.Zoomable = true;
            CA.CursorX.AutoScroll = true;
            CA.CursorX.IsUserSelectionEnabled = true;
            CA.AxisY.ScaleView.Zoomable = true;
            CA.CursorY.AutoScroll = true;
            CA.CursorY.IsUserSelectionEnabled = true;
        }

        double R_x_Y(List<double> v, List<double> v2, double aver1, double aver2, double g1, double g2)
        {
            double ser_xy = 0;
            double Sum = v.Count();
            for (int i = 0; i < Sum; i++)
            {
                ser_xy = ser_xy + (v[i] * v2[i]);
            }
            ser_xy = ser_xy / Sum;
            double r_x_y = (Sum / (Sum - 1)) * (ser_xy - aver1 * aver2) / (g1 * g2);
            return r_x_y;
        }
        double Kvantil_u(double a)
        {
            double kv_u = 0;
            if (a <= 0.0001) kv_u = 3.69;
            else if (a <= 0.001) kv_u = 3.08;
            else if (a <= 0.005) kv_u = 2.57;
            else if (a <= 0.01) kv_u = 2.33;
            else if (a <= 0.025) kv_u = 1.96;
            else if (a <= 0.05) kv_u = 1.64;
            else if (a <= 0.075) kv_u = 1.44;
            else if (a <= 0.1) kv_u = 1.28;
            else if (a <= 0.15) kv_u = 1.03;
            else if (a <= 0.2) kv_u = 0.84;
            else if (a <= 0.25) kv_u = 0.67;
            else if (a <= 0.35) kv_u = 0.39;
            else if (a <= 0.4) kv_u = 0.25;
            else if (a <= 0.45) kv_u = 0.13;
            else if (a <= 0.5) kv_u = 0;
            else if (a <= 0.55) kv_u = 0.13;
            else if (a <= 0.65) kv_u = 0.39;
            else if (a <= 0.7) kv_u = 0.53;
            else if (a <= 0.75) kv_u = 0.67;
            else if (a <= 0.8) kv_u = 0.84;
            else if (a <= 0.85) kv_u = 1.04;
            else if (a <= 0.9) kv_u = 1.28;
            else if (a <= 0.95) kv_u = 1.65;
            else if (a <= 0.97) kv_u = 1.88;
            else if (a <= 0.985) kv_u = 2.17;
            else if (a <= 0.99) kv_u = 2.33;
            else if (a <= 0.995) kv_u = 2.57;
            else kv_u = 3.69;
            return kv_u;
        }
        double Kvantil_hi(double v)
        {
            double a = 0.1;
            a = 1 - a;
            double kv_u = Kvantil_u(a);
            double kv_hi = v * Math.Pow(1 - 2 / (9 * v) + kv_u * Math.Sqrt(2 / (9 * v)), 3);
            return kv_hi;
        }

        void Calc()
        {
            chart1.Series.Clear();
            usedColors = new HashSet<Color>();
            int K = Convert.ToInt32(tbK.Text);
            List<double> m = new List<double>();
            List<double> g = new List<double>();
            List<double> N = new List<double>();
            List<double> a = new List<double>();
            List<double> b = new List<double>();
            List<double> A = new List<double>();
            List<double> B = new List<double>();
            List<Tuple<List<double>, List<double>>> X = new List<Tuple<List<double>, List<double>>>();
            List<Tuple<List<double>, List<double>>> X_test = new List<Tuple<List<double>, List<double>>>();
            Random r = new Random();
            if (radioButton1.Checked)
            {
                for (int i = 0; i < K; i++)
                {
                    m.Add(r.Next(-100, 100));
                    if (i != 0 && m[i] == m[i - 1]) m[i] = m[i - 1] + 30;
                    g.Add(r.Next(40, 60));
                    N.Add(r.Next(100, 800));
                    a.Add(r.Next(-1000, 1000));
                    b.Add(r.Next(-20, 20));
                    if (b[i] == 0) b[i] = 2;
                    A.Add(r.Next(0, 100));
                    B.Add(A[i] + r.Next(5, 30));
                }
            }
            else if (radioButton2.Checked)
            {
                Random r1 = new Random();
                double g1 = r1.Next(40, 60);
                double b1 = r.Next(-10, 10);
                if (b1 == 0) b1 = 2;
                double A1 = r1.Next(0, 100);
                double B1 = A1 + r1.Next(5, 40);
                double rizn = B1 - A1;
                for (int i = 0; i < K; i++)
                {
                    m.Add(r.Next(-100, 100));
                    if (i != 0 && m[i] == m[i - 1]) m[i] = m[i - 1] + 10;
                    g.Add(g1);
                    N.Add(r.Next(100, 800));
                    a.Add(r1.Next(-1000, 1000));
                    b.Add(b1);
                    A.Add(r1.Next(0, 100));
                    B.Add(A[i] + rizn);
                }
            }
            for(int i = 0; i < K; i++)
            {
                List<double> Eps = new List<double>();
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                double a_lin = a[i];
                double b_lin = b[i];
                List<double> iks = new List<double>();
                double N2 = N[i] * 5;
                double z2 = 0;
                int count2 = 0, count3 = 0;
                for (int j = 0; j < N2; j++)
                {
                    if (count2 < 5)
                    {
                        z2 += Convert.ToDouble(r.Next(Convert.ToInt32(m[i] - 3 * g[i]) * 1000, Convert.ToInt32((m[i] + 3 * g[i])) * 1000) / 1000.0);
                        count2++;

                    }
                    else
                    {
                        j--;
                        count2 = 0;
                        Eps.Add(z2 / 5.0);
                        z2 = 0;
                        iks.Add(Convert.ToDouble(r.Next(Convert.ToInt32(A[i]) * 1000, Convert.ToInt32(B[i]) * 1000) / 1000.0));
                        x.Add(iks[count3]);
                        count3++;
                    }
                }
                iks.Add(Convert.ToDouble(r.Next(Convert.ToInt32(A[i]) * 1000, Convert.ToInt32(B[i]) * 1000) / 1000.0));
                x.Add(iks[iks.Count - 1]);
                Eps.Add(z2 / 5.0);
                List<double> x1 = new List<double>();
                for (int j = 0; j < (N[i] * 0.7); j++)
                {
                    y.Add(a_lin + b_lin * iks[j] + Eps[j]);
                    x1.Add(iks[j]);
                }
                X.Add(new Tuple<List<double>, List<double>>(x1, y));

                List<double> x2 = new List<double>();
                List<double> y2 = new List<double>();
                for (int j = (int)(N[i] * 0.7); j < N[i]; j++)
                {
                    y2.Add(a_lin + b_lin * iks[j] + Eps[j]);
                    x2.Add(iks[j]);
                }
                X_test.Add(new Tuple<List<double>, List<double>>(x2, y2));
            }

            List<double> x_max = new List<double>();
            List<double> y_max = new List<double>();
            for (int i = 0; i < K; i++)
            {
                x_max.Add(X[i].Item1.Max());
                x_max.Add(X[i].Item1.Min());
                y_max.Add(X[i].Item2.Max());
                y_max.Add(X[i].Item2.Min());
            }
            
            List<Tuple<double,double>> X_ser = new List<Tuple<double, double>>();
            List<Tuple<double, double>> G = new List<Tuple<double, double>>();
            List<double[,]> DC = new List<double[,]>();
            for (int i = 0; i < X.Count(); i++)
            {
                double aver1 = 0;
                for(int j = 0; j < X[i].Item1.Count; j++)
                {
                    aver1 += X[i].Item1[j];
                }
                aver1 /= X[i].Item1.Count;
                double aver2 = 0;
                for (int j = 0; j < X[i].Item2.Count; j++)
                {
                    aver2 += X[i].Item2[j];
                }
                aver2 /= X[i].Item2.Count;
                X_ser.Add(new Tuple<double, double>(aver1, aver2));
            }
            for (int i = 0; i < X.Count(); i++)
            {
                double g1 = 0;
                for (int j = 0; j < X[i].Item1.Count; j++)
                {
                    g1 += Math.Pow(X[i].Item1[j] - X_ser[i].Item1,2);
                }
                g1 /= (X[i].Item1.Count - 1);
                double g2 = 0;
                for (int j = 0; j < X[i].Item2.Count; j++)
                {
                    g2 += Math.Pow(X[i].Item2[j] - X_ser[i].Item2, 2);
                }
                g2 /= (X[i].Item1.Count - 1);
                G.Add(new Tuple<double, double>(Math.Sqrt(g1), Math.Sqrt(g2)));
            }
            for (int i = 0; i < X.Count(); i++)
            {
                double[,] temp1 = new double[2,2];
                for(int j = 0; j < 2; j++)
                {
                    for(int k = 0; k < 2; k++)
                    {
                        if (k == j && k == 0) temp1[j, k] = Math.Pow(G[i].Item1, 2);
                        else if (k == j && k == 1) temp1[j, k] = (Math.Pow(G[i].Item2, 2));
                        else temp1[j, k] = (G[i].Item2 * G[i].Item1 * R_x_Y(X[i].Item1, X[i].Item2, X_ser[i].Item1, X_ser[i].Item2, G[i].Item1, G[i].Item2));
                    }
                }
                DC.Add(temp1);
            }
            
            double All_N = 0;
            for(int i = 0; i < X.Count(); i++)
            {
                All_N += X[i].Item1.Count();
            }
            List<double> P = new List<double>();
            for (int i = 0; i < X.Count(); i++)
            {
                P.Add(X[i].Item1.Count() / All_N);
            }

            List<Matrix<double>> S_i = new List<Matrix<double>>();
            double V1 = 0;
            for (int i = 0; i < DC.Count; i++)
            {
                Matrix<double> S_d = Matrix<double>.Build.DenseOfArray(DC[i]);
                S_i.Add((X[i].Item2.Count - 1) * S_d);
            }
            Matrix<double> S = S_i[0];
            for (int i = 1; i < S_i.Count; i++)
            {
                S += S_i[i];
            }
            S = S / (All_N - K);
            for (int i = 0; i < DC.Count; i++)
            {
                Matrix<double> S_d = Matrix<double>.Build.DenseOfArray(DC[i]);
                double res1 = S.Determinant();
                double res2 = S_d.Determinant();
                V1 += (X[i].Item2.Count - 1)/2.0 * Math.Log(res1/res2);
            }
            double hi = Kvantil_hi(6 * (K - 1) / 2.0);
            if(hi >= V1)
            {
                for(int i = 0; i < DC.Count; i++)
                {
                    DC[i] = S.ToArray();
                }
            }
            double[,] matrix = new double[K, K];
            for(int i = 0; i < X.Count(); i++)
            {
                for (int j = 0; j < X[i].Item1.Count; j++)
                {
                    double[,] v = new double [2,1];
                    v[0,0] = X[i].Item1[j];
                    v[1,0] = X[i].Item2[j];
                    Matrix<double> V = Matrix<double>.Build.DenseOfArray(v);
                    List<double> g_x = new List<double>();
                    for(int k = 0; k < X_ser.Count; k++)
                    {
                        double[,] v2 = new double[2,1];
                        v2[0,0] = X_ser[k].Item1;
                        v2[1,0] = X_ser[k].Item2;
                        Matrix<double> V_ser = Matrix<double>.Build.DenseOfArray(v2);
                        Matrix<double> dc = Matrix<double>.Build.DenseOfArray(DC[k]);
                        Matrix<double> res = Math.Log(P[k]) - 0.5 * Math.Log(dc.Determinant()) - 0.5 * (V - V_ser).Transpose() * dc.Inverse() * (V - V_ser);
                        g_x.Add(res[0, 0]);
                    }
                    int index = g_x.IndexOf(g_x.Max());
                    matrix[i, index] += 1.0 / X[i].Item1.Count;
                }
            }

            List<Series> s0 = new List<Series>();
            for(int i = 0; i < K; i++)
            {
                Series s = new Series();
                s.ChartType = SeriesChartType.Point;
                s.Color = GenerateLightColor();
                s0.Add(s);
            }
            double N1 = 400.0;
            double h_x = (x_max.Max() - x_max.Min()) / N1;
            double h_y = (y_max.Max() - y_max.Min()) / N1;
            double x_s;
            double y_s = y_max.Min();
            chart1.ChartAreas[0].AxisX.Minimum = x_max.Min();
            chart1.ChartAreas[0].AxisY.Minimum = y_max.Min();
            chart1.ChartAreas[0].AxisX.Maximum = x_max.Max();
            chart1.ChartAreas[0].AxisY.Maximum = y_max.Max();
            
            for (int i = 0; i < N1; i++)
            {
                x_s = x_max.Min();
                for (int j = 0; j < N1; j++)
                {
                    double[,] v = new double[2, 1];
                    v[0, 0] = x_s;
                    v[1, 0] = y_s;
                    Matrix<double> V = Matrix<double>.Build.DenseOfArray(v);
                    List<double> g_x = new List<double>();
                    for (int k = 0; k < X_ser.Count; k++)
                    {
                        double[,] v2 = new double[2, 1];
                        v2[0, 0] = X_ser[k].Item1;
                        v2[1, 0] = X_ser[k].Item2;
                        Matrix<double> V_ser = Matrix<double>.Build.DenseOfArray(v2);
                        Matrix<double> dc = Matrix<double>.Build.DenseOfArray(DC[k]);
                        Matrix<double> res = Math.Log(P[k]) - 0.5 * Math.Log(dc.Determinant()) - 0.5 * (V - V_ser).Transpose() * dc.Inverse() * (V - V_ser);
                        g_x.Add(res[0, 0]);
                    }
                    int index = g_x.IndexOf(g_x.Max());
                    s0[index].Points.AddXY(x_s, y_s);
                    x_s += h_x;
                }
                y_s += h_y;
            }
            for(int i = 0; i < s0.Count; i++)
            {
                chart1.Series.Add(s0[i]);
            }
            for (int i = 0; i < X.Count(); i++)
            {
                Series s = new Series();
                s.ChartType = SeriesChartType.Point;
                s.Color = GenerateDarkColor();
                for (int j = 0; j < X[i].Item1.Count; j++)
                {
                    s.Points.AddXY(X[i].Item1[j], X[i].Item2[j]);
                }
                chart1.Series.Add(s);
            }
            Draw_martix(matrix, K);
        }

        HashSet<Color> usedColors = new HashSet<Color>();

        Random rand = new Random();
        Color GenerateLightColor()
        {
            Color color;
            do
            {
                int r = rand.Next(128, 256);
                int g = rand.Next(128, 256);
                int b = rand.Next(128, 256);
                color = Color.FromArgb(r, g, b);
            } while (usedColors.Contains(color));

            usedColors.Add(color);
            return color;
        }

        Color GenerateDarkColor()
        {
            Color color;
            do
            {
                int r = rand.Next(0, 128);
                int g = rand.Next(0, 128);
                int b = rand.Next(0, 128);
                color = Color.FromArgb(r, g, b);
            } while (usedColors.Contains(color));

            usedColors.Add(color);
            return color;
        }

        void Draw_martix(double[,] matrix, int n)
        {
            dg.Rows.Clear();
            dg.Columns.Clear();
            for (int i = 0; i < n; i++)
            {
                dg.Columns.Add((i + 1).ToString(), (i + 1).ToString());
            }
            dg.Rows.Add(n);
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                dg.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    dg.Rows[i].Cells[j].Value = Math.Round(matrix[i,j],3).ToString();
                }
            }
        }
        

        private void btnCalc_Click(object sender, EventArgs e)
        {
            Calc();
        }
    }
}
