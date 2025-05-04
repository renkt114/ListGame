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
using System.Windows.Shapes;

namespace ListSLG.window
{
    /// <summary>
    /// TitleWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TitleWindow : Window
    {
        public TitleWindow()
        {
            InitializeComponent();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {

            // メインウィンドウを表示
            //var mainWindow = new MainWindow();
            Application.Current.MainWindow = new MainWindow();

            Application.Current.MainWindow.Show();

            this.Close();

        }
    }
}
