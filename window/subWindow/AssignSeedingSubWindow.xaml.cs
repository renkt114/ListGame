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
    /// AssignSeedingSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AssignSeedingSubWindow : Window
    {

        General breedGeneral;

        public AssignSeedingSubWindow(General breedGeneral)
        {
            InitializeComponent();

            this.breedGeneral = breedGeneral;

            List<SeedingPartnerItemsSource> seedingPartnerItemsSources = GeneralDao.getSeedingPartnerGeneral()
                .Select(x =>
                {
                    String typ = "";
                    switch (x.abilityType)
                    {
                        case 1:
                            typ = "攻撃";
                            break;
                        case 2:
                            typ = "防御";
                            break;
                        case 3:
                            typ = "機動";
                            break;
                        default:
                            break;
                    }

                return new SeedingPartnerItemsSource
                {
                    general = x,
                    typ = typ
                };}).ToList();


            SeedingPartnerDataGrid.ItemsSource = seedingPartnerItemsSources;

        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {

            SeedingPartnerItemsSource seedingPartnerItemsSource = (SeedingPartnerItemsSource)SeedingPartnerDataGrid.SelectedCells[0].Item;
            General selectedPartnerGeneral = seedingPartnerItemsSource.general;

            SeedingJoin seedingJoin = new SeedingJoin();
            seedingJoin.breedGeneralId = breedGeneral.generalId;
            seedingJoin.partnerGeneralId = selectedPartnerGeneral.generalId;
            // SeedingPrestageCostNumLabelの値をcostPrestigeに代入
            seedingJoin.costPrestige = int.Parse(SeedingPrestageCostNumLabel.Content.ToString());
            SeedingJoinDao.createSeedingJoin(seedingJoin);

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


        // 選択時イベント
        private void seedingPartnerSelectionChanged(object sender, EventArgs e)
        {
            // 選択された行のtier×500をSeedingPrestageCostNumLabelに表示
            SeedingPartnerItemsSource seedingPartnerItemsSource = (SeedingPartnerItemsSource)SeedingPartnerDataGrid.SelectedCells[0].Item;
            General selectedPartnerGeneral = seedingPartnerItemsSource.general;
            SeedingPrestageCostNumLabel.Content = selectedPartnerGeneral.tier * 500;

        }


        private class SeedingPartnerItemsSource
        {
            public General general { get; set; }
            public String typ { get; set; }
        }

    }
}
