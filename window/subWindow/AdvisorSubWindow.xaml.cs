using ListSLG.dao;
using ListSLG.model;
using ListSLG.resources.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ListSLG.window.subWindow
{
    /// <summary>
    /// AdvisorSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AdvisorSubWindow : Window
    {

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_STYLE = -16;
        const int WS_SYSMENU = 0x80000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            int style = GetWindowLong(handle, GWL_STYLE);
            style = style & (~WS_SYSMENU);
            SetWindowLong(handle, GWL_STYLE, style);
        }

        public AdvisorSubWindow()
        {
            InitializeComponent();
        }

        // AdviceDaoを使用しテーブルからAdviceTextを取得し、AdviceRichTextBoxに表示する
        public void showAdvice()
        {
            // AdviceDaoを使用しテーブルからAdviceTextを取得
            // TODO 全件取得し流せるようにする
            List<Advice> advice = AdviceDao.getAllAdviceSortedPriority();

            // AdviceRichTextBoxにAdviceTextを表示
            AdviceRichTextBox.Document.Blocks.Clear();
            foreach (Advice a in advice)
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run(a.adviseText));
                AdviceRichTextBox.Document.Blocks.Add(p);
            }

        }


    }
}
