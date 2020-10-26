using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolation
{
    class SplineInterpolation
    {
        private double[,] arr;
        private double[] alpha;
        private double[] barr;
        private double a, b, h;
        int m, n;


        public static double LinearSpline(double x)
            {
            if (Math.Abs(x) <= 1)
                return 1 - Math.Abs(x);
            else return 0;
           
            }

            public static double QuadraticSpline(double x)
            {
                if (Math.Abs(x) < 0.5)
                    return 2 - Math.Pow(Math.Abs(x) - 0.5, 2) - Math.Pow(Math.Abs(x) + 0.5, 2);
                if (Math.Abs(x) < 1.5)
                    return Math.Pow(Math.Abs(x) - 1.5, 2);
                return 0;
            }

            public static double CubicSpline(double x)
            {
                if (Math.Abs(x) < 1)
                    return ((double)1 / 6) * (Math.Pow(2 - Math.Abs(x), 3) - 4 * Math.Pow(1 - Math.Abs(x), 3));
                if (Math.Abs(x) < 2)
                    return ((double)1 / 6) * Math.Pow(2 - Math.Abs(x), 3);

                return 0;
            }
        public static double F1(double x)
        {// функція, яку треба інтерполювати
            return 5*Math.Cos(7*x) * Math.Sin(x);
        }
        public  double F(double x)
        {// функція, яку треба інтерполювати
            return 5 * Math.Cos(7 * x) * Math.Sin(x);
        }
     
        public double Spline(double x)
            {
                double res = 0;
                for (int i = 0; i < n + 3; i++)
                {
                    res += alpha[i] * CubicSpline((x - (a + h * (i - 1))) / h);//від рівновіддалених вузлів
                }
                return res;
            }
        public double Spline1(double x)
        {
            double res = 0;
            for (int i = 0; i < n + 3; i++)
            {
                res += alpha[i] * LinearSpline((x - (a + h * (i - 1))) / h);//від рівновіддалених вузлів
            }
            return res;
        }

        public double Derivative(double x)
        {
            return 5 * (-7 * Math.Sin(x) * Math.Sin(7 * x) + Math.Cos(x) * Math.Cos(7 * x));
        }

        public SplineInterpolation(int n, int m, double a, double b)
        {
            arr = new double[n + m, n + m];
            barr = new double[n + m];
            this.a = a;
            this.b = b;
            h = (b - a) / n;
            this.m = 2;
            this.n = n;
        }

        public void CalcSpline3()
        {
            arr = new double[n + 3, n + 3];
            barr = new double[n + 3];
            arr[0, 0] = 1.0;
            arr[0, 1] = -2.0;
            arr[n + 2, n + 1] = 2.0;
            arr[n + 2, n + 2] = 1.0;

            barr[0] = Derivative(a) * h;
            for (int i = 0; i < n + 1; i++)
            {
                arr[i + 1, i] = 1.0 / 6.0;
                arr[i + 1, i + 1] = 2.0 / 3.0;
                arr[i + 1, i + 2] = 1.0 / 6.0;
                barr[i + 1] = F(a + h * i);
            }

            barr[n + 2] = h * Derivative(b);

            for (int i = 0; i < n+3; i++)
            {
                for (int j = 0; j < n+3; j++)
                {
                    Console.Write(arr[i, j] + " ");

                }
                Console.WriteLine();
            }



            alpha = Progonka(n,arr, barr);
            

        }

        public void CalcSpline1()
        {

        }
        private void Swap(ref double a, ref double b)
            {
                double temp = a;
                a = b;
                b = temp;
            }

            private void Swap(ref int x, ref int y)
            {
                int temp = x;
                x= y;
                y = temp;
            }


        public double[] Progonka(int n, double[,] a, double[] f)
        {
            
            double[] x = new double[f.Length];

            double[] A = new double[f.Length];
            double[] B = new double[f.Length];
            double[] C = new double[f.Length];

            
            for (int i = 0; i < n - 1; i++)
            {
                B[i] = new double();
                B[i] = a[i, i + 1];
            }
            for (int i = 0; i < n; i++)
            {
                C[i] = new double();
                C[i] = a[i, i];
            }
            for (int i = 1; i < n; i++)
            {
                A[i] = new double();
                A[i] = a[i, i - 1];
            }
            
            double m;
            for (int i = 1; i < n; i++)
            {
                m = A[i] / C[i - 1];
                C[i] = C[i] - m * B[i - 1];
                f[i] = f[i] - m * f[i - 1];

            }

            x[n - 1] = f[n - 1] / C[n - 1];

            for (int i = n - 2; i >= 0; i--)
            {
                x[i] = (f[i] - B[i] * x[i + 1]) / C[i];
            }

            return x;
        }
        public double[] Gauss(double[,] a, double[] b)
            {
                const double eps = 0.000001;
                double[] x = new double[b.Length];
                for (int k = 0; k < b.Length - 1; k++)
                {
                    int maxx = k;
                    for (int j = k; j < k; j++)
                        if (a[maxx, k] < a[j, k])
                            maxx = j;
                    for (int j = k; j < b.Length; j++)
                        Swap(ref a[maxx, j], ref a[k, j]);
                    Swap(ref maxx, ref k);
                    if (Math.Abs(a[k, k]) < eps)
                        return x;
                    for (int i = k + 1; i < b.Length; i++)
                    {
                        double m = -a[i, k] / a[k, k];
                        for (int j = k; j < b.Length; j++)
                            a[i, j] += a[k, j] * m;
                        b[i] += b[k] * m;
                    }
                }
                if (Math.Abs(a[b.Length - 1, b.Length - 1]) < eps)
                    return x;

                for (int i = b.Length - 1; i > -1; i--)
                {
                    double temp = b[i];
                    for (int j = i + 1; j < b.Length; j++)
                        temp -= a[i, j] * x[j];

                    x[i] = temp / a[i, i];
                }
                return x;
            }
        
    }
}
