using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using wpf_angleDetector_3.Classes;

namespace wpf_angleDetector_3.Controls {
    /// <summary>
    /// Логика взаимодействия для PointsViewer.xaml
    /// </summary>
    public partial class PointsViewer : UserControl {
        public PointsViewer() {
            InitializeComponent();
            data = new DataPoints(-60, 60);
            //data.RandomData(1024, 15, 0, this);
            //data.Draw();
            //DrawData();
        }
        DataPoints data;
        #region Data
        // Used when manually scrolling.
        //private Point scrollTarget;
        private Point scrollStartPoint;
        private Point scrollStartOffset;
        //private Point previousPoint;
        //private Vector velocity;
        //private double friction;
        private DispatcherTimer animationTimer = new DispatcherTimer();
        #endregion
        
        public void SetData(DataPoints dat) {
            data = dat;
        }

        public void DrawDataSimple() {
            cnv.Children.Clear();
            cnv.Width = data.LenX * 10;
            cnv.Height = 3800;
            Random rnd = new Random();


            int tempValueX = (int)data.DataX[0];
           // int tempValueZ = (int)data.DataZ[0];
            Rectangle rec1 = new Rectangle() {
                Height = 1,
                Width = 1,
                Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                Name = "C_I_0"
            };
            rec1.MouseEnter += data.Rec_MouseEnter;
            Canvas.SetLeft(rec1, (data.DataX[0] + Math.Abs(data.MinX)) * 10);
            Canvas.SetTop(rec1, data.DataZ[0]);
            cnv.Children.Add(rec1);


            for (int i = 1; i < data.DataSize; i++) {              
                if (tempValueX != (int)data.DataX[i]) {
              //if (tempValueX != (int)data.DataX[i] && tempValueZ != (int)data.DataZ[i]) {
                    tempValueX = (int)data.DataX[i];
                    //tempValueZ = (int)data.DataZ[i];
                    Rectangle rec = new Rectangle() {
                        Height = 1,
                        Width = 1,
                        Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                        Name = "C_I_" + i.ToString()
                    };
                    rec.MouseEnter += data.Rec_MouseEnter;                   
                    Canvas.SetLeft(rec, (data.DataX[i] + Math.Abs(data.MinX)) * 10);
                    Canvas.SetTop(rec, data.DataZ[i]);
                    cnv.Children.Add(rec);

                }
            }
        }

        public void DrawData() {
            cnv.Children.Clear();
            cnv.Width = data.LenX * 10;
            cnv.Height = 3800;
            Random rnd = new Random();

            for (int i = 0; i < data.DataSize; i++) {
                Rectangle rec = new Rectangle() {
                    Height = 1,
                    Width = 1,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                    Name = "C_I_" + i.ToString()
                };
                rec.MouseEnter += data.Rec_MouseEnter;
                Canvas.SetLeft(rec, (data.DataX[i] + Math.Abs(data.MinX)) * 10);
                Canvas.SetTop(rec, data.DataZ[i]);
                cnv.Children.Add(rec);
            }
        }

        private void cnv_MouseWheel(object sender, MouseWheelEventArgs e) {
            //var element = sender as UIElement;
            var element = grid as UIElement;
            var position = e.GetPosition(element);
            var transform = element.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1); // choose appropriate scaling factor
            //cnv.Width = cnv.Width * scale;
            //cnv.Width = (double)SizeToContent.Width;
            //cnv.Width = Width * scale;
            //cnv.Width = cnv.ActualWidth;
            //ScrollViewer.Width = cnv.Children.Count * scale;
            //grid.Width = grid.ActualWidth;


            Console.WriteLine($"{ScrollViewer.ViewportWidth}");

            Console.WriteLine($"{cnv.ActualWidth}   --   {cnv.Width}   --   {grid.ActualWidth}   --   {grid.Width}");
          
            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            transform.Matrix = matrix;
        }

        private void MouseWheelZoom(object sender, MouseWheelEventArgs e) {
            if (cnv.IsMouseOver) {

                Point mouseAtImage = e.GetPosition(cnv); // ScrollViewer_CanvasMain.TranslatePoint(middleOfScrollViewer, Canvas_Main);
                Point mouseAtScrollViewer = e.GetPosition(ScrollViewer);

                ScaleTransform st = grid.LayoutTransform as ScaleTransform;
                if (st == null) {
                    st = new ScaleTransform();
                    grid.LayoutTransform = st;
                }

                if (e.Delta > 0) {
                    st.ScaleX = st.ScaleY = st.ScaleX * 1.25;
                    if (st.ScaleX > 64)
                        st.ScaleX = st.ScaleY = 64;
                } else {
                    st.ScaleX = st.ScaleY = st.ScaleX / 1.25;
                    if (st.ScaleX < 1)
                        st.ScaleX = st.ScaleY = 1;
                }
                #region [this step is critical for offset]
                ScrollViewer.ScrollToHorizontalOffset(0);
                ScrollViewer.ScrollToVerticalOffset(0);
                this.UpdateLayout();
                #endregion

                Vector offset = cnv.TranslatePoint(mouseAtImage, ScrollViewer) - mouseAtScrollViewer; // (Vector)middleOfScrollViewer;
                ScrollViewer.ScrollToHorizontalOffset(offset.X);
                ScrollViewer.ScrollToVerticalOffset(offset.Y);
                this.UpdateLayout();

                e.Handled = true;
            }


        }
        #region Mouse Events
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
            if (ScrollViewer.IsMouseOver) {
                // Save starting point, used later when determining how much to scroll.
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = ScrollViewer.HorizontalOffset;
                scrollStartOffset.Y = ScrollViewer.VerticalOffset;

                // Update the cursor if can scroll or not.
                this.Cursor = (ScrollViewer.ExtentWidth > ScrollViewer.ViewportWidth) ||
                    (ScrollViewer.ExtentHeight > ScrollViewer.ViewportHeight) ?
                    Cursors.ScrollAll : Cursors.Arrow;

                this.CaptureMouse();
            }

            base.OnPreviewMouseDown(e);
        }


        protected override void OnPreviewMouseMove(MouseEventArgs e) {
            if (this.IsMouseCaptured) {
                // Get the new scroll position.
                Point point = e.GetPosition(this);

                // Determine the new amount to scroll.
                Point delta = new Point(
                    (point.X > this.scrollStartPoint.X) ?
                        -(point.X - this.scrollStartPoint.X) :
                        (this.scrollStartPoint.X - point.X),

                    (point.Y > this.scrollStartPoint.Y) ?
                        -(point.Y - this.scrollStartPoint.Y) :
                        (this.scrollStartPoint.Y - point.Y));

                // Scroll to the new position.
                ScrollViewer.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                ScrollViewer.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
            }

            base.OnPreviewMouseMove(e);
        }



        protected override void OnPreviewMouseUp(MouseButtonEventArgs e) {
            if (this.IsMouseCaptured) {
                this.Cursor = Cursors.Arrow;
                this.ReleaseMouseCapture();
            }

            base.OnPreviewMouseUp(e);
        }
        #endregion

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e) {
           // ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset);
            //e.Handled = true;
        }
    }
}

