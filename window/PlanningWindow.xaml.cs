using ListSLG.logic.validationLogic;
using ListSLG.model;
using ListSLG.window.subWindow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// PlanningWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PlanningWindow : Window
    {
        // 選択中のミッションid（いらない気が）
        private int selectedMissionId;

        // 表示中のミッションリスト、初回で取得してあとは使いまわす
        private List<Mission> nowMissionList;
        // 画面に表示中の軍団リスト、確定後にこの内容でCorpPlan更新
        private List<PlannedCorp> displayPlannedCorpList;

        public PlanningWindow()
        {
            InitializeComponent();


            // ミッションリスト取得及び設定
            List<Mission> missionList = MissionDao.getAllMission();
            nowMissionList = missionList;
            MissionListBox.ItemsSource = nowMissionList;

            // 軍団リストの取得及び設定
            diplayCorpMission();


        }

        // 軍団リストの取得及び設定
        public void diplayCorpMission()
        {

            // 将軍含めた軍団リスト取得
            List<GeneralAllDataGridDTO> allyGeneralsAllDataList = GeneralDao.getAllyGeneralAllDataGrid();
            // 戻り値：計画設定対象の軍団リスト
            List<PlannedCorp> plannedCorpList = new List<PlannedCorp>();

            // 無所属を除く自軍限定とする
            for (int i = 1; i < 9; i++)
            {

                // 一行分の計画設定対象の軍団リスト
                PlannedCorp plannedCorp = new PlannedCorp();

                // ループで対照とする軍団とその将軍
                var corpGeneralList = allyGeneralsAllDataList.Where(x => x.corp.corpId == i);

                // 無かったら終了（次以降も無いはずだが、一応continueとする）
                if (!corpGeneralList.Any())
                {
                    continue;
                }

                // 軍団ID
                plannedCorp.corpId = corpGeneralList.First().corp.corpId;
                // 軍団名
                plannedCorp.corpName = corpGeneralList.First().corp.corpName;
                // 軍団長（中央歩兵の将軍）取得。いないとエラーになるため一旦FirstOrDefaultで取る
                GeneralAllDataGridDTO leaderRec = corpGeneralList.Where(x => x.troop.potisioinId == (int)PotisioinDiv.centorInfantry).FirstOrDefault();
                if (leaderRec != null)
                {
                    // いる場合はその人指定
                    plannedCorp.leaderName = leaderRec.general.name;
                }
                // 部隊数
                plannedCorp.troopNum = corpGeneralList.Count();
                // 総兵数
                plannedCorp.sumSoldiernum = (int)corpGeneralList.Select(x => x.troop.soldierNum).Sum();
                // 設定済みの任務
                CorpPlan corpPlan = CorpPlanDao.getCorpPlanByCorp(i);
                // corpPlanがnullの場合はnullのまま、そうでない場合は任務IDを設定
                if (corpPlan != null)
                {
                    plannedCorp.assignMissionId = corpPlan.missionId;
                    plannedCorp.assignMissionName = MissionDao.getMissionByMissionId(corpPlan.missionId).missionName;
                }


                // 計画設定対象の軍団リストに追加
                plannedCorpList.Add(plannedCorp);
            }

            // Datagridに設定
            displayPlannedCorpList = plannedCorpList;
            CorpDataGrid.ItemsSource = displayPlannedCorpList;
            // 一番上の行を選択中にする
            CorpDataGrid.SelectedIndex = 0;
            


        }

        // ミッションリスト選択時の動き
        // 軍団リストにミッションidをあてがい、画面にミッション内容を表示
        private void MissionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 軍団選択時のミッシヨンリスト初期化は無視
            if(MissionListBox.SelectedValue is null)
            {
                return;
            }

            // 選択したミッションのidからMission取得、説明表示
            selectedMissionId = (int)MissionListBox.SelectedValue;
            Mission selectedMission = MissionDao.getMissionByMissionId(selectedMissionId);
            SelectedMissionText.Text = selectedMission.missionText;
            // selectedMission.gainedPrestigeを末尾に追加
            SelectedMissionText.Text += "\n" + "勝利時加算威信" + selectedMission.gainedPrestige;


            // 上記ミッションを別途選択中の軍団に設定
            displayPlannedCorpList[CorpDataGrid.SelectedIndex].assignMissionId = selectedMissionId;
            displayPlannedCorpList[CorpDataGrid.SelectedIndex].assignMissionName = selectedMission.missionName;
            CorpDataGrid.ItemsSource = displayPlannedCorpList;
            CorpDataGrid.Items.Refresh();

        }

        // 軍団選択時
        private void SelectionChanged_MouseDown(object sender, RoutedEventArgs e)
        {
            // ミッションリスト選択解除
            MissionListBox.SelectedIndex = -1;

        }

        // 完了時の処理
        // 戦闘画面へは遷移しなくなったので削除
        /*
        private void Planning_Phase_Complete_Button_Click(object sender, RoutedEventArgs e)
        {

            // 軍団リストに設定されたミッションをCorpPlanに挿入
            foreach (PlannedCorp displayPlannedCorp in displayPlannedCorpList)
            {
                CorpPlanDao.createCorpPlan(displayPlannedCorp.corpId, displayPlannedCorp.assignMissionId);

            }

            var win = new BattleWindow();
            win.Show();
            this.Close();

        }
        */

        private void Planning_Phase_Remove_Button_Click(object sender, RoutedEventArgs e)
        {


            // 既存の計画削除
            CorpPlanDao.deleteAllCorpPlan();

            // 軍団リストに設定されたミッションをCorpPlanに挿入
            foreach (PlannedCorp displayPlannedCorp in displayPlannedCorpList)
            {
                // 未設定の場合はスキップ
                if (displayPlannedCorp.assignMissionId != 0)
                {
                    CorpPlanDao.createCorpPlan(displayPlannedCorp.corpId, displayPlannedCorp.assignMissionId);
                }


            }


            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.mainWindowIsEnabled(true);
                
            }

            this.Close();

        }
    }


    // 計画設定対象の軍団リスト 画面下に表示しているやつの要素クラス
    public class PlannedCorp
    {
        public int corpId { get; set; }
        public String corpName { get; set; }

        public String leaderName { get; set; }

        public int troopNum { get; set; }

        public int sumSoldiernum { get; set; }

        public int assignMissionId { get; set; }
        public String assignMissionName { get; set; }

    }



}
