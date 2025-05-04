using ListSLG.model;
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
    /// CreateCorpSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CreateCorpSubWindow : Window
    {
        public CreateCorpSubWindow()
        {
            InitializeComponent();
        }


        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {

            CorpDao.createCorp(CorpNameTextBox.Text);
            

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.reflashAllDataGrid();
            }

            this.Close();

        }
    }
}
