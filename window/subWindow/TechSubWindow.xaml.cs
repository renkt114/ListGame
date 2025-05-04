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

namespace ListSLG.window.subWindow
{
    /// <summary>
    /// TechSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TechSubWindow : Window
    {
        public TechSubWindow()
        {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.mainWindowIsEnabled(true);
                mainWindow.reflashAllDataGrid();

            }

            this.Close();
        }

        private void Tech_Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && clickedButton.Tag != null)
            {
                // ボタンのタグのtechIdでTechEnableSubWindowを開く
                this.IsEnabled = false;
                if (int.TryParse(clickedButton.Tag.ToString(), out int techId))
                {
                    var win = new TechEnableSubWindow(techId);
                    win.ShowDialog();
                }
                else
                {
                    // Handle the case where the Tag is not a valid integer
                    MessageBox.Show("Invalid techId");
                }

                this.IsEnabled = true;
            }
        }
    }
}
