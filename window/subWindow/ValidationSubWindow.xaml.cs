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
    /// ValidationSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ValidationSubWindow : Window
    {
        public ValidationSubWindow()
        {
            InitializeComponent();

        }

        public void showText(List<string> validationErrorList)
        {
            string validationErrortext = string.Join("\n", validationErrorList);

            ValidationLogRichTextBox.Document.Blocks.Clear();
            ValidationLogRichTextBox.AppendText(validationErrortext);
        }

        public void close()
        {
            this.Close();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
