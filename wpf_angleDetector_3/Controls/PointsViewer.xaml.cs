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
        }


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
    }
}
