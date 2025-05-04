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
    /// BattleLogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleLogWindow : Window
    {
        public BattleLogWindow()
        {
            InitializeComponent();
        }

        public void addLine(String logText)
        {

            // 新しい行を作成
            Paragraph paragraph = new Paragraph(new Run(logText));

            // RichTextBox に新しい行を追加
            BattleLogRichTextBox.Document.Blocks.Add(paragraph);
        }

        public void clearLine()
        {

            // 全行削除
            BattleLogRichTextBox.Document.Blocks.Clear();
        }

        // ウィンドウを閉じる
        public void close()
        {
            this.Close();
        }

    }
}
