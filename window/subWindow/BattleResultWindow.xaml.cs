using ListSLG.bean;
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

namespace ListSLG.window.subWindow
{
    /// <summary>
    /// BattleResultWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleResultWindow : Window
    {

        BattleWindow battleWindow = new BattleWindow();
        public BattleResultWindow(CorpTroopBean corpTroopBean)
        {
            InitializeComponent();

            battleWindow = Application.Current.Windows.OfType<BattleWindow>().SingleOrDefault(x => x.IsActive);

            if (corpTroopBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry).soldierNum > 0)
            {
                ResultText.Text = "Victory";
            }
            else
            {
                ResultText.Text = "Defeat";
            }

            var dataGridSetList = new ObservableCollection<DataGridSet>();

            for (int i = 1; i <= 7; i++)
            {
                DataGridSet dataGridSet = new DataGridSet();
                var troop = typeof(CorpTroopBean).GetProperty("troop" + i).GetValue(corpTroopBean) as Troop;
                var result = typeof(CorpTroopBean).GetProperty("result" + i).GetValue(corpTroopBean) as ResultBean;
                dataGridSet.name = troop.general.name;
                dataGridSet.atkTimes = result.atkTimes;
                dataGridSet.atkSumNum = result.atkSumNum;
                dataGridSet.defTimes = result.defTimes;
                dataGridSet.defSumNum = result.defSumNum;
                dataGridSet.blockTimes = result.blockTimes;
                dataGridSetList.Add(dataGridSet);

            }

            AllyResultGrid.ItemsSource = dataGridSetList;

        }



        class DataGridSet
        {
            public string name { get; set; }
            public int atkSumNum { get; set; }
            public int atkTimes { get; set; }
            public int defTimes { get; set; }
            public int defSumNum { get; set; }

            public int blockTimes { get; set; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            battleWindow.battleSetting();
            this.Close();

        }
    }
}
