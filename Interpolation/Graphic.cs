//-----------------------------------------------------------------------
// <copyright file="Graphic.cs" company="LNU">

// Copyright (c) LNU. All rights reserved.

// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
//вузли мусять бути цілочисельними для B1 сплайнів

namespace Project
{//непарна - б, парна  -а
    public partial class Graphic : Form
    {
        static Random rnd = new Random();
        private int n;
        private double eps = 0.01, a, b, h;
        private const double STEP = 0.01;
        public const int AMOUNT_OF_DATA_POINTS = 10;//n
        public const double POINT = 0.5;
        public const int NUMBER_OF_UNCERTAINTIES = 100;

        public delegate double BaseSpline(double x);
        public Graphic()
        {
            InitializeComponent();
            zedGraphControl1.GraphPane.Title = "Spline interpolation";
           
        }

     public double Error(double x)
        {
            Interpolation.SplineInterpolation inter = new Interpolation.SplineInterpolation(n, 3, a, b);
            inter.CalcSpline3();
            return Math.Abs(Interpolation.SplineInterpolation.F1(x) - inter.Spline(x)); }
        private void setValue()
        {//зчитує проміжок і кількість вузлів з текстбоксів
            a = double.Parse(textBox1.Text);
            b = double.Parse(textBox2.Text);
            n = int.Parse(textBox3.Text);
            h = (b - a) / n;
        }

        private void drawBasis(BaseSpline base1)
        {
             GraphPane pane = zedGraphControl1.GraphPane;
            pane.YAxis.Min = -1;
            pane.YAxis.Max = 1;
            pane.XAxis.Min = a ;
            pane.XAxis.Max = b;
         
            string name = "Spline ";
            pane.CurveList.Clear();
          
            for (int k = 0; k < n + 1; k++)
            {
                PointPairList list = new PointPairList();
                for (double x = a; x < b + eps; x += eps)
                {
                    list.Add(x, base1(x - a - h * k));
                }
                KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                KnownColor randomColorName = names[rnd.Next(names.Length)];
                Color randomColor = Color.FromKnownColor(randomColorName);
                
                pane.AddCurve((name + k.ToString()), list,randomColor, SymbolType.None);
              
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        private void drawFunc(Func<double, double> func,Color color, string name)
        {
            GraphPane pane = zedGraphControl1.GraphPane;

          
                PointPairList list = new PointPairList();
                for (double x = a; x < b + eps; x += eps)
                {
                    list.Add(x, func(x));
                }
            
            pane.AddCurve((name), list, color,SymbolType.None);
            PointPairList coordinates = new PointPairList();
            for (int i = 0; i < n+1; i++)
            {
                coordinates.Add(a+i*h,Interpolation.SplineInterpolation.F1(a + i * h));//від рівновіддалених вузлів
            }
            LineItem Curve = pane.AddCurve("Coordinates",coordinates, Color.Black, SymbolType.Star);
            Curve.Line.IsVisible = false;


    
        zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }


        public string MaxError()
        {
            double temp = 0; double point=0;
            for (double i = a; i < b + eps; i += eps)
            {
                if (temp < Error(i))
                {
                    temp = Error(i);
                    point = i;
                }
            }
            return "Max error is " + temp.ToString() + " in point " + point.ToString()+"\n";
        }
            

        private void button3_Click(object sender, EventArgs e)
        {
            setValue();
            Interpolation.SplineInterpolation inter = new Interpolation.SplineInterpolation(n, 3, a, b);
            inter.CalcSpline3();
            // zedGraphControl1.GraphPane.CurveList.Clear();
            drawFunc(new Func<double, double>(inter.F), Color.Coral, "Function");
            drawFunc(new Func<double, double>(inter.Spline), Color.Crimson, "Cubic Spline");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            setValue();
            if (rbLinear.Checked)
                //якщо вибрано лінійну інтерполяцію
                drawBasis(new BaseSpline(Interpolation.SplineInterpolation.LinearSpline));
            else
            {
                if (rbCubic.Checked)
                    drawBasis(new BaseSpline(Interpolation.SplineInterpolation.CubicSpline));
                else
                    drawBasis(new BaseSpline(Interpolation.SplineInterpolation.QuadraticSpline));
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        

        private void button6_Click(object sender, EventArgs e)
        {
            setValue();
            
           // zedGraphControl1.GraphPane.CurveList.Clear();
            GraphPane pane = zedGraphControl1.GraphPane;
            //  pane.CurveList.Clear();
            PointPairList coordinates = new PointPairList();
            for (int i = 0; i < n + 1; i++)
            {
                coordinates.Add(a + i * h, Interpolation.SplineInterpolation.F1(a + i * h));//від рівновіддалених вузлів
            }
            LineItem Curve = pane.AddCurve("Coordinates", coordinates, Color.Cyan, SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Graphic_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
