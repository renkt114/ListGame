using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// ResignWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ResignWindow : Window
    {
        // 表示中のデータリスト
        List<ResignItem> displayResignItemdataList = new List<ResignItem>();

        public ResignWindow()
        {
            InitializeComponent();

            List<GeneralAllDataGridDTO> generalDTOList = GeneralDao.getGeneralAllDataGrid();

            int index = 0;
            foreach (GeneralAllDataGridDTO generalDTO in generalDTOList)
            {
                ResignItem resignItem = new ResignItem();
                resignItem.no = index;
                // コンボボックスのID、初期値は未選択のID
                // TODO IDを区分値化する
                resignItem.resignComboBoxId = 1;
                resignItem.generalDTO = generalDTO;
                index++;

                displayResignItemdataList.Add(resignItem);
            }
            ResignDataGrid.ItemsSource = displayResignItemdataList;
        }

        // Resignコンボボックスの変更時のイベント
        // displayResignItemdataListを変更内容で更新する
        private void ResignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                ResignItem resignItem = comboBox.DataContext as ResignItem;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem != null)
                {
                    // Tagプロパティの値を取得
                    string tagValue = selectedItem.Tag.ToString();
                    // コンボボックスのタグでID指定
                    displayResignItemdataList[resignItem.no].resignComboBoxId = Int32.Parse(tagValue);
                }
            }
        }


        // 解雇対象リスト 画面に表示しているdatagridの要素クラス
        public class ResignItem
        {

            public int no { get; set; }
            public int resignComboBoxId { get; set; }

            public GeneralAllDataGridDTO generalDTO { get; set; }
        }

        // 完了ボタン
        private void Resign_Complete_Button_Click(object sender, RoutedEventArgs e)
        {

            displayResignItemdataList.Where(x => x.resignComboBoxId == 2).ToList().ForEach(x =>
            {
                // troopのみ削除することでプレイヤーの軍団から削除とする。CorpとGeneralは残す。
                TroopDao.deleteTroop(x.generalDTO.troop);

            });
        }
    }
}
