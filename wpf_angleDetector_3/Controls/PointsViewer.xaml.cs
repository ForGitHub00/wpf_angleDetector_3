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
            DataPoints data = new DataPoints(-400, 400);
            data.RandomData(1024, 15, 0, this);
            data.Draw();
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
                       animationTimer.Tick += new EventHandler(HandleWorldTimerTick);
                        animationTimer.Start();
        }

        #region Data
        // Used when manually scrolling.
        private Point scrollTarget;
        private Point scrollStartPoint;
        private Point scrollStartOffset;
        private Point previousPoint;
        private Vector velocity;
        private double friction;
        private DispatcherTimer animationTimer = new DispatcherTimer();
        #endregion


        const double ScaleRate = 1.1;
        private void cnv_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                st.ScaleX *= ScaleRate;
                st.ScaleY *= ScaleRate;
            } else {
                st.ScaleX /= ScaleRate;
                st.ScaleY /= ScaleRate;
            }
            e.Handled = true;
        }

        private void HandleWorldTimerTick(object sender, EventArgs e) {

        }
        private void ScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
        }
        private void cnv_MouseUp(object sender, MouseButtonEventArgs e) {
        }
        private void cnv_MouseMove(object sender, MouseEventArgs e) {
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

    }
}

