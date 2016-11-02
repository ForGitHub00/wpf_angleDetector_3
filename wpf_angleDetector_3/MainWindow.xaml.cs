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


namespace wpf_angleDetector_3 {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            DataPoints inData = new DataPoints(-40, 40);
            DataPoints outData = new DataPoints(-10, 10);

            inData.RandomData(1280);
            outData.RandomData(320);

            inputData.SetData(inData);
            inputData.DrawData();

            outputData.SetData(inData);
            outputData.DrawDataSimple();
        }
    }
}
