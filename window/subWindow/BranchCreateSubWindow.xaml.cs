using ListSLG.dao;
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

namespace ListSLG.window.subWindow
{
    /// <summary>
    /// BranchCreateSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BranchCreateSubWindow : Window
    {

        GeneralsDetailDataGridDTO selectedGeneral;

        List<General> familyGeneralListAll = new List<General>();

        public BranchCreateSubWindow(GeneralsDetailDataGridDTO selectedGeneral)
        {
            InitializeComponent();

            this.selectedGeneral = selectedGeneral;


            // 1親等
            List<General> familyGeneralList = PedigreeDao.getChildGeneral(this.selectedGeneral.general.generalId);

            // 2親等
            List<General> familyGeneralList2 = new List<General>();
            foreach (General general in familyGeneralList)
            {
                familyGeneralList2.AddRange(PedigreeDao.getChildGeneral(general.generalId));
            }

            // 3親等
            List<General> familyGeneralList3 = new List<General>();
            foreach (General general in familyGeneralList2)
            {
                familyGeneralList3.AddRange(PedigreeDao.getChildGeneral(general.generalId));
            }

            // 1親等から3親等までのリストを作成
            this.familyGeneralListAll.AddRange(familyGeneralList);
            this.familyGeneralListAll.AddRange(familyGeneralList2);
            this.familyGeneralListAll.AddRange(familyGeneralList3);


            LeaderLabel.Content = selectedGeneral.general.name;
            FamilyDataGrid.ItemsSource = familyGeneralListAll;

        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {

            Corp newCorp = CorpDao.createCorp(selectedGeneral.general.name + "家");
            TroopDao.updateCorpId(selectedGeneral.troop.troopId, newCorp.corpId);

            //familyGeneralListAllのGeneralを新しいCorpに所属させる
            foreach (General general in familyGeneralListAll)
            {
                TroopDao.updateCorpId(general.troopId, newCorp.corpId);
            }

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.reflashAllDataGrid();
            }

            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
