using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using wpf_angleDetector_3.Controls;

namespace wpf_angleDetector_3.Classes {
    class DataPoints{

        public double[] DataX;
        public double[] DataZ;

        public int DataSize;
        public double MaxX { get; set; }
        public double MinX { get; set; }
        public double MaxzZ{ get; set; }
        public double MinZ { get; set; }

        public PointsViewer PV;


        public DataPoints() { }
        public DataPoints(double[] X, double[] Z) {
            DataX = X;
            DataZ = Z;
        }
        public DataPoints(double minX, double maxX) {
            MaxX = maxX;
            MinX = minX;
        }

        public void RandomData(int size, double noizeZ, double angle, PointsViewer p) {
            PV = p;
            DataSize = size;
            DataX = new double[size];
            DataZ = new double[size];
            Random rnd = new Random();
            double stepX = Math.Abs(MinX) + Math.Abs(MaxX);
            stepX /= size;

            for (int i = 0; i < size; i++) {
                DataX[i] = MinX + i * stepX;
                DataZ[i] = rnd.Next(250, 252);
            }


        }

        public void Draw() {
            PV.cnv.Width = 8000;
            PV.cnv.Height = 8000;
            Random rnd = new Random();
            for (int i = 0; i < DataSize; i++) {
                Rectangle rec = new Rectangle() {
                    Height = 0.5,
                    Width = 0.5,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                    Name = "C_" + i.ToString()
                };
                rec.MouseEnter += Rec_MouseEnter;
                Canvas.SetLeft(rec, DataX[i]);
                Canvas.SetTop(rec, DataZ[i]);
                PV.cnv.Children.Add(rec);
            }
        }

        private void Rec_MouseEnter(object sender, MouseEventArgs e) {
            Rectangle rec = sender as Rectangle;
            int index = Convert.ToInt32(rec.Name.Substring(2));
            TextBlock t = new TextBlock();
            MessageBox.Show($"Name {rec.Name} \nZ = {DataZ[index]} \nX = {DataX[index]}");
            /*
            lb_name.Content = rec.Name;
            lb_z.Content = Data[index].Z.ToString();
            lb_x.Content = Data[index].X.ToString();
            */
        }

        private double GradToRad(double grad) {
            return grad * Math.PI / 180;
        }
    }
}
