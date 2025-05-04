using ListSLG.logic.generate;
using ListSLG.model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// HireWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HireWindow : Window
    {

        // 画面に表示中の雇用候補将軍リスト、確定後にこの内容でGeneral、troop更新
        private List<HireListItem> displayHireList = new List<HireListItem>();

        public HireWindow()
        {
            InitializeComponent();

            NewGeneralGenerate newGeneralGenerate = new NewGeneralGenerate();
            List<General> generatedGeneralList = newGeneralGenerate.generateNewGeneralList(10, 7);

            int index = 0;
            foreach (General general in generatedGeneralList)
            {
                HireListItem hireListItem = new HireListItem();
                hireListItem.no = index;
                hireListItem.IsSelected = false;
                hireListItem.generatedGeneral = general;
                displayHireList.Add(hireListItem);
                index++;
            }


            HireGeneralDataGrid.ItemsSource = displayHireList;

        }

        private void Hire_Phase_Complete_Button_Click(object sender, RoutedEventArgs e)
        {

            displayHireList.Where(x => x.IsSelected == true).ToList().ForEach(x =>
            {

                Troop troop = new Troop();
                troop.corpId = 0;
                troop.potisioinId = 0;
                troop.troopTypeId = 0;
                troop.soldierNum = 1000;
                troop.maxSoldierNum = 1000;
                troop = TroopDao.createTroop(troop);

                x.generatedGeneral.troopId = troop.troopId;
                GeneralDao.createGeneral(x.generatedGeneral);

            });

        }


        private void Hire_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;


            // チェックボックスのDataContextからデータアイテムを取得
            HireListItem dataItem = checkBox.DataContext as HireListItem;

            // チェックが入ったときの処理
            if (dataItem != null)
            {
                displayHireList[dataItem.no].IsSelected = true;
            }
        }

        private void Hire_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;


            // チェックボックスのDataContextからデータアイテムを取得
            HireListItem dataItem = checkBox.DataContext as HireListItem;

            // チェックが入ったときの処理
            if (dataItem != null)
            {
                displayHireList[dataItem.no].IsSelected = false;
            }
        }

    }

    // 雇用対象リスト 画面に表示しているdatagridの要素クラス
    public class HireListItem
    {

        public int no { get; set; }
        public bool IsSelected { get; set; }
        public General generatedGeneral { get; set; }

    }
}
