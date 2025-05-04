using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using ListSLG.bean;
using ListSLG.window;
using static ListSLG.model.CorpDao;
using ListSLG.window.subWindow;
using ListSLG.logic.turn;
using ListSLG.logic.savedata;
using ListSLG.logic.validationLogic;
using ListSLG.logic.resource;
using ListSLG.logic.growth;
using System.Globalization;
using ListSLG.window.debug;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ListSLG.logic.generate;
using ListSLG.Migrations;

namespace ListSLG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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



        private List<DataGrid> corpDatagridList = new List<DataGrid>() { null, null, null, null, null, null, null, null, null };
        private List<TabItem> corpTabItemList = new List<TabItem>() { null, null, null, null, null, null, null, null, null };

        public ObservableCollection<GeneralsDetailDataGridDTO> allGeneralsCollection { get; set; } = new ObservableCollection<GeneralsDetailDataGridDTO>();

        public ObservableCollection<GeneralTabItem> generalTabItems { get; set; } = new ObservableCollection<GeneralTabItem>();

        // バリデーションウィンドウインスタンス
        public ValidationSubWindow validationSubWindow;

        // アドバイスウィンドウインスタンス
        public AdvisorSubWindow advisorSubWindow;

        // 初期表示
        public MainWindow()
        {
            
            InitializeComponent();

            // セーブデータ初期化
            saveDataLogic.DBDataFileInitialization();

            // 初期データ作成
            NewGameGenerateLogic.newGameGenerate();

            // 表示パラメータ初期化
            this.reflashAllDisplayParam();

            // 各一覧初期化
            this.reflashAllDataGrid();


            // 画面描画後にもろもろの処理を行う
            ContentRendered += windowLoaded;


        }

        // 初回画面描画後の処理
        private void windowLoaded(object sender, EventArgs e)
        {
            // メインメニュー追加
            MainMenuUpdate();
            // コンテキストメニュー追加
            ContexitMenuUpdate();
            // AdvisorSubWindow表示
            advisorSubWindow = new AdvisorSubWindow();
            advisorSubWindow.Left = this.Left + this.Width + 10;
            advisorSubWindow.Top = this.Top;
            advisorSubWindow.Show();
            advisorSubWindow.showAdvice();

        }

        private void Create_Corp_Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindowIsEnabled(false);
            var win = new CreateCorpSubWindow();
            win.Show();


        }

        // セーブ
        private void Save_Menu_Item_Click(object sender, RoutedEventArgs e)
        {

            MenuItem clickedMenuItem = sender as MenuItem;
            if (clickedMenuItem != null)
            {
                int saveDataId = (int)clickedMenuItem.Tag;
                if (saveDataId != null)
                {
                    saveDataLogic.saveGameData(SaveDataDao.getSaveData(saveDataId));
                }
            }

            // セーブデータ欄更新
            this.MainMenuUpdate();

        }

        // ロード
        private void Load_Menu_Item_Click(object sender, RoutedEventArgs e)
        {

            MenuItem clickedMenuItem = sender as MenuItem;
            if (clickedMenuItem != null)
            {
                int saveDataId = (int)clickedMenuItem.Tag;
                if (saveDataId != null)
                {
                    saveDataLogic.loadGameData(SaveDataDao.getSaveData(saveDataId));
                }
            }

            // ロード処理（すなわち変更したデータで再描画）
            reflashAllDataGrid();

            // ロードしましたのポップアップ表示
            MessageBox.Show("ロードしました。", "ロード完了", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        // ゲーム終了
        private void SystemGameExit(object sender, RoutedEventArgs e)
        {
            // ゲーム終了
            Application.Current.Shutdown();

        }

        // 将軍詳細
        private void cmGeneraldetail(object sender, RoutedEventArgs e)
        {
            if (allGenerals.SelectedCells.Count == 0)
            {
                return;
            }

            // 選択した将軍情報取得
            GeneralsDetailDataGridDTO selectedGeneral = (GeneralsDetailDataGridDTO)allGenerals.SelectedCells[0].Item;

            // 将軍詳細画面表示
            var win = new GeneralSubWindow(selectedGeneral.general);
            win.ShowDialog();

        }


        // 軍団変更
        private void cmAssignCorp(object sender, RoutedEventArgs e)
        {
            // 選択した将軍情報取得
            GeneralAllDataGridDTO selectedGeneral = (GeneralAllDataGridDTO)allGenerals.SelectedCells[0].Item;
            // コンテキストで選択したitem
            MenuItem menuitem = (MenuItem)sender;
            // タグ番号（軍団IDと同値）
            string? tag = menuitem.Tag.ToString();

            // troop更新
            TroopDao.updateCorpId(selectedGeneral.troop.troopId, Int32.Parse(tag));

            // 各一覧再表示
            this.reflashAllDataGrid();

        }

        //分家作成
        private void createBranch(object sender, RoutedEventArgs e)
        {

            if (allGenerals.SelectedCells.Count == 0)
            {
                return;
            }

            // Corpの数を判定（未所属があるため5まで）
            if (CorpDao.getAllAllyCorpData().Count >= 5)
            {
                // アラートを出して終了
                MessageBox.Show("軍団数が上限に達しています。", "分家作成失敗", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }


            // 選択した将軍情報取得
            GeneralsDetailDataGridDTO selectedGeneral = (GeneralsDetailDataGridDTO)allGenerals.SelectedCells[0].Item;

            // 分家作成画面表示
            var win = new BranchCreateSubWindow(selectedGeneral);
            win.ShowDialog();

        }

        // 種付け
        private void cmAssignSeeding(object sender, RoutedEventArgs e)
        {
            if (allGenerals.SelectedCells.Count == 0)
            {
                return;
            }

            // 選択した将軍情報取得
            GeneralsDetailDataGridDTO selectedGeneral = (GeneralsDetailDataGridDTO)allGenerals.SelectedCells[0].Item;

            // 種付け予定済みか判定
            if(selectedGeneral.partnerGeneral == null)
            {
                // 種付け画面表示
                var win = new AssignSeedingSubWindow(selectedGeneral.general);
                win.ShowDialog();
            }else{
                // 種付け解除
                SeedingJoinDao.deleteSeedingJoinById(selectedGeneral.general);
                // 各一覧再表示
                this.reflashAllDataGrid();
                return;
            }

        }

        // 昇格予約
        private void cmAssignPromotion(object sender, RoutedEventArgs e)
        {

            if (allGenerals.SelectedCells.Count == 0)
            {
                return;
            }

            // 選択した将軍情報取得
            GeneralsDetailDataGridDTO selectedGeneral = (GeneralsDetailDataGridDTO)allGenerals.SelectedCells[0].Item;

            // 昇格予約あるいは取り消し
            RenewalGeneralDao.updateRpromotionFlg(selectedGeneral.general);

            // 各一覧再表示
            this.reflashAllDataGrid();


        }


        // 解任予約
        private void cmAssignRetire(object sender, RoutedEventArgs e)
        {

            if (allGenerals.SelectedCells.Count == 0)
            {
                return;
            }

            // 選択した将軍情報取得
            GeneralsDetailDataGridDTO selectedGeneral = (GeneralsDetailDataGridDTO)allGenerals.SelectedCells[0].Item;

            // 解任予約あるいは取り消し
            RenewalGeneralDao.updateRretireFlg(selectedGeneral.general);

            // 各一覧再表示
            this.reflashAllDataGrid();

        }


        // 計画ボタン（計画画面へ）
        private void Planning_Phase_Button_Click(object sender, RoutedEventArgs e)
        {

            mainWindowIsEnabled(false);
            var win = new PlanningWindow();
            win.Show();


        }

        // 技術ボタン（技術画面へ）
        private void Tech_Window_Button_Click(object sender, RoutedEventArgs e)
        {

            mainWindowIsEnabled(false);
            var win = new TechSubWindow();
            win.Show();

        }


        // 次へボタン（戦闘画面へ）
        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {

            if (validationSubWindow != null)
            {
                validationSubWindow.close();
            }

            // バリデーションチェック
            List<string> validationErrorList = MainWindowValidation.turnEndValidation();
            if (validationErrorList.Count > 0)
            {

                validationSubWindow = new ValidationSubWindow();
                // TODO エラー表示
                validationSubWindow.showText(validationErrorList);
                validationSubWindow.Show();
                return;
            }

            if (GameMasterDao.getGameMaster().sectionNum <= 4)
            {

                mainWindowIsEnabled(false);
                var win = new BattleWindow();
                win.Show();

            }
            else
            {
                // 期末処理のみで計画面行かない
                this.returnBattle();
            }

        }

        // 表示パラメータ再表示
        public void reflashAllDisplayParam()
        {
            // ターン表示
            var gamemaster = GameMasterDao.getGameMaster();
            EraLabel.Content = (EraDiv)gamemaster.eraNum;
            TermLabel.Content = gamemaster.termNum + "期";
            if(gamemaster.sectionNum <= 4)
            {
                SectionLabel.Content = gamemaster.sectionNum + "節";
                
            } else
            {
                SectionLabel.Content = "期末";
            }
            PrestageNumLabel.Content = gamemaster.prestige;
            PrestageCostNumLabel.Content = PrestigeLogic.calcCostPrestige();

        }


        // 各一覧再表示
        // TODO: 全部張り替えなおすのは重そうだなあ・・・
        public void reflashAllDataGrid(bool reflashTab = false)
        {

            MainMenuUpdate();

            //技術最新化
            App.techBean.renewTechEnable();

            List<GeneralAllDataGridDTO> allyGeneralsAllDataList = GeneralDao.getAllyGeneralAllDataGrid();

            if (reflashTab)
            {
                reflashCorps(allyGeneralsAllDataList);
            } else
            {
                createCorpsTab(allyGeneralsAllDataList);
            }

            List<GeneralsDetailDataGridDTO> allGeneralsDetailDataGrid = GeneralDao.getAllGeneralsDetailDataGrid(allyGeneralsAllDataList);
            reflashAllGenerals(allGeneralsDetailDataGrid);

            // 合計値再表示
            reflashSumData();

            // 表示パラメータ再表示
            reflashAllDisplayParam();



            this.IsEnabled = true;


        }

        // 合計値再表示
        public void reflashSumData()
        {
            // 総合計取得および設定
            SumCorpNumTextBlock.Text = (CorpDao.getCorpNum() - 1).ToString();
            SumGeneralNumTextBlock.Text = TroopDao.getAllSumTroopNum().ToString();
            SumSoldierNumTextBlock.Text = TroopDao.getAllSumTroopMaxSoldierNum().ToString();

            // 軍団ごとの合計値取得
            var AllCorpSumNumDataList = CorpDao.getAllAllyCorpSumNumData();

            // あってもなくても4軍団繰り返す
            for (int corpCnt = 1; corpCnt <= 4; corpCnt = corpCnt + 1)
            {

                // UI名を取得するためのプレフィクス生成
                String corpPre = "Corp" + corpCnt.ToString();

                // ループの軍団IDのUI取得
                var corpGrid = this.FindName(corpPre + "SumGrid") as Grid;
                var nameLabel = this.FindName(corpPre + "NameLabel") as Label;
                var generalNumTextBlock = this.FindName(corpPre + "GeneralNumTextBlock") as TextBlock;
                var soldierNumTextBlock = this.FindName(corpPre + "SoldierNumTextBlock") as TextBlock;

                CorpSumNumDataDTO corpData = new CorpSumNumDataDTO();

                // ループの軍団IDがあるか判定
                if (AllCorpSumNumDataList.Any(x =>(x.corpId == corpCnt)))
                {

                    // あった場合は設定
                    corpData = AllCorpSumNumDataList.Where(x => (x.corpId == corpCnt)).First();
                    // 軍団追加時を想定しgridの高さ戻す。
                    // TODO 戻すときの高さ直書きはどうだろうか・・・ 
                    corpGrid.Height = 33;
                    nameLabel.Content = corpData.corpName;
                    generalNumTextBlock.Text = corpData.sumGeneralNum.ToString();
                    soldierNumTextBlock.Text = corpData.sumMaxSoldierNum.ToString();

                } else
                {
                    // 無い場合はgridの高さを0に
                    corpGrid.Height = 0;
                    nameLabel.Content = "";
                    generalNumTextBlock.Text = 0.ToString();
                    soldierNumTextBlock.Text = 0.ToString();
                }

            }

        }


        // 軍団変更コンテキスト再作成
        /*
        private void reflashCorpArrangeContext()
        {
            // いったん全削除
            CorpArrangeMenu.Items.Clear();
            // コンテキストメニュー軍団配属選択肢リスト作成
            List<Corp> corpList = CorpDao.getAllAllyCorpData();
            // タグ番号（変更時の軍団ID更新値に利用する）
            int tagNum = 0;
            foreach (Corp corp in corpList)
            {
                MenuItem subCorpArrangeMenuItem = new MenuItem { Tag = tagNum, Header = corp.corpName };
                subCorpArrangeMenuItem.Click += cmAssignCorp;
                CorpArrangeMenu.Items.Add(subCorpArrangeMenuItem);
                tagNum += 1;
            }

        }
        */

        // 将軍一覧再表示
        private void reflashAllGenerals(List<GeneralsDetailDataGridDTO> allyGeneralsAllDataList)
        {
            // データセット
            allGenerals.ItemsSource = allyGeneralsAllDataList;
            // コンテキストメニュー最新化（初回表示時はここと同じことをloadedイベントでも行う。処理が二回走るので微妙だが・・・）
            ContexitMenuUpdate();
        }


        // メインメニュー最新化
        private void MainMenuUpdate()
        {
            // データ欄クリア
            SaveMenuItem.Items.Clear();
            LoadMenuItem.Items.Clear();

            List<SaveData> saveDataList = SaveDataDao.getAllSaveDataNot999();
            int saveNum = 1;

            foreach (SaveData saveData in saveDataList)
            {
                MenuItem childSaveMenuItem = new MenuItem();
                childSaveMenuItem.Header = "データ" + saveNum + ":  " + saveData.date;
                childSaveMenuItem.Click += Save_Menu_Item_Click;
                childSaveMenuItem.Tag = saveData.saveDataId;
                SaveMenuItem.Items.Add(childSaveMenuItem);

                MenuItem childLoadMenuItem = new MenuItem();
                childLoadMenuItem.Header = "データ" + saveNum + ":  " + saveData.date;
                childLoadMenuItem.Click += Load_Menu_Item_Click;
                childLoadMenuItem.Tag = saveData.saveDataId;
                LoadMenuItem.Items.Add(childLoadMenuItem);

                saveNum++;

            }

            SaveData autoSaveData = SaveDataDao.getSaveData(999);

            if (autoSaveData.date != null)
            {
                MenuItem childLoadMenuItem = new MenuItem();
                childLoadMenuItem.Header = "オートセーブ" + ":  " + autoSaveData.date;
                childLoadMenuItem.Click += Load_Menu_Item_Click;
                childLoadMenuItem.Tag = autoSaveData.saveDataId;
                LoadMenuItem.Items.Add(childLoadMenuItem);

            }


        }




        // コンテキストメニュー最新化
        private void ContexitMenuUpdate()
        {
            // 起動直後に呼ばれるためdatagrid最新化
            allGenerals.UpdateLayout();

            foreach (GeneralsDetailDataGridDTO item in allGenerals.ItemsSource)
            {

                var contextMenu = new ContextMenu();

                // 将軍詳細
                var generaldetailMenuItem = new MenuItem();
                generaldetailMenuItem.Click += cmGeneraldetail;
                generaldetailMenuItem.Header = "将軍詳細情報";
                contextMenu.Items.Add(generaldetailMenuItem);

                // 解任予約していない場合のみ各種操作可能
                if (item.renewalGeneral == null || item.renewalGeneral.retireFlg == 0)
                {
                    // 種付けメニュー
                    var partnerMenuItem = new MenuItem();
                    partnerMenuItem.Click += cmAssignSeeding;
                    if (item.partnerGeneral == null)
                    {
                        partnerMenuItem.Header = "配偶予約";
                    }
                    else
                    {
                        partnerMenuItem.Header = "配偶予約解除";
                    }
                    contextMenu.Items.Add(partnerMenuItem);

                    // 昇格予約メニュー(要技術)
                    if (App.techBean.getTechEnableBool(4))
                    {
                        
                        var promotionMenuItem = new MenuItem();
                        promotionMenuItem.Click += cmAssignPromotion;
                        if (item.renewalGeneral == null || item.renewalGeneral.promotionFlg == 0)
                        {
                            promotionMenuItem.Header = "昇格予約";
                        }
                        else
                        {
                            promotionMenuItem.Header = "昇格予約解除";
                        }
                        contextMenu.Items.Add(promotionMenuItem);
                    }

                    // 分家作成メニュー(要技術)
                    if (App.techBean.getTechEnableBool(5))
                    {
                        var branchMenuItem = new MenuItem();
                        branchMenuItem.Click += createBranch;
                        branchMenuItem.Header = "分家作成";
                        contextMenu.Items.Add(branchMenuItem);
                    }

                }

                // 解任予約メニュー
                var retireMenuItem = new MenuItem();
                retireMenuItem.Click += cmAssignRetire;
                if (item.renewalGeneral == null || item.renewalGeneral.retireFlg == 0)
                {
                    retireMenuItem.Header = "解任予約";
                }
                else
                {
                    retireMenuItem.Header = "解任予約解除";
                }
                contextMenu.Items.Add(retireMenuItem);

                // 行ごとにコンテキストメニューを設定する
                DataGridRow row = (DataGridRow)allGenerals.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    row.ContextMenu = contextMenu;
                }

            }
        }

        // 軍団一覧タブ再描画
        private void reflashCorps(List<GeneralAllDataGridDTO> allyGeneralsAllDataList)
        {

            // 無所属を除く自軍限定とする
            for (int i = 1; i < 9; i++)
            {
                var corpGeneralList = allyGeneralsAllDataList.Where(x => x.corp.corpId == i);

                // 無かったら終了（次以降も無いはずだが、一応continueとする）
                if (!corpGeneralList.Any())
                {
                    continue;
                }
                // 保存していたdatagridに登録
                corpDatagridList[i].ItemsSource = corpGeneralList;

            }
        }

        // 軍団タブ作成
        private void createCorpsTab(List<GeneralAllDataGridDTO> allyGeneralsAllDataList)
        {
            foreach (TabItem tabItem in corpTabItemList)
            {
                // 既にタブがあったら削除
                if (tabItem != null)
                {
                    mainTab.Items.Remove(tabItem);
                }
            }

            // 無所属を除く自軍限定とする
            for (int i = 1; i < 9; i++)
            {
                // ループで対象とするcorpIdを取得
                var corpGeneralList = allyGeneralsAllDataList.Where(x => x.corp.corpId == i);

                // 無かったら終了（次以降も無いはずだが、一応continueとする）
                if (!corpGeneralList.Any())
                {
                    continue;
                }

                //データグリッド作成
                var dataGrid = new DataGrid();
                dataGrid.Name = "corpDataGrid" + i;
                dataGrid.AutoGenerateColumns = false;

                //カラム作成
                DataGridTextColumn nameColumn = new DataGridTextColumn();
                nameColumn.Header = "名前";
                nameColumn.Binding = new System.Windows.Data.Binding("general.name");
                nameColumn.IsReadOnly = true;
                dataGrid.Columns.Add(nameColumn);

                DataGridTextColumn rankColumn = new DataGridTextColumn();
                rankColumn.Header = "階級";
                rankColumn.Binding = new System.Windows.Data.Binding("general.rank");
                rankColumn.IsReadOnly = true;
                dataGrid.Columns.Add(rankColumn);

                DataGridTextColumn yearsColumn = new DataGridTextColumn();
                yearsColumn.Header = "年数";
                yearsColumn.Binding = new System.Windows.Data.Binding("general.years");
                yearsColumn.IsReadOnly = true;
                dataGrid.Columns.Add(yearsColumn);

                DataGridTextColumn soldierNumColumn = new DataGridTextColumn();
                soldierNumColumn.Header = "兵数";
                soldierNumColumn.Binding = new System.Windows.Data.Binding("troop.soldierNum");
                soldierNumColumn.IsReadOnly = true;
                dataGrid.Columns.Add(soldierNumColumn);

                DataGridTextColumn conditionColumn = new DataGridTextColumn();
                conditionColumn.Header = "調子";
                conditionColumn.Binding = new System.Windows.Data.Binding("general.condition ");
                conditionColumn.IsReadOnly = true;
                dataGrid.Columns.Add(conditionColumn);

                DataGridTextColumn injuredColumn = new DataGridTextColumn();
                injuredColumn.Header = "全治";
                injuredColumn.Binding = new System.Windows.Data.Binding("general.injured ");
                injuredColumn.IsReadOnly = true;
                dataGrid.Columns.Add(injuredColumn);

                DataGridTextColumn attackAbilityColumn = new DataGridTextColumn();
                attackAbilityColumn.Header = "攻撃";
                attackAbilityColumn.Binding = new System.Windows.Data.Binding("general.attackAbility ");
                attackAbilityColumn.IsReadOnly = true;
                dataGrid.Columns.Add(attackAbilityColumn);

                DataGridTextColumn defenseAbilityColumn = new DataGridTextColumn();
                defenseAbilityColumn.Header = "防御";
                defenseAbilityColumn.Binding = new System.Windows.Data.Binding("general.defenseAbility ");
                defenseAbilityColumn.IsReadOnly = true;
                dataGrid.Columns.Add(defenseAbilityColumn);

                DataGridTextColumn maneuverAbilityColumn = new DataGridTextColumn();
                maneuverAbilityColumn.Header = "機動";
                maneuverAbilityColumn.Binding = new System.Windows.Data.Binding("general.maneuverAbility ");
                maneuverAbilityColumn.IsReadOnly = true;
                dataGrid.Columns.Add(maneuverAbilityColumn);

                // 行スタイル設定
                Style rowStyle = new Style(typeof(DataGridRow));
                rowStyle.Setters.Add(new Setter(DataGridRow.IsEnabledProperty, new Binding("general.years") { Converter = new YearsToEnabledConverter() }));
                dataGrid.RowStyle = rowStyle;

                // ポジションコンボボックス要素（別定義したい）
                ObservableCollection<PotisionComboBox> potisionComboBoxCollection = new ObservableCollection<PotisionComboBox>
                {
                    new PotisionComboBox { potisioinId = 0, potisioinName = "未配属" },
                    new PotisionComboBox { potisioinId = 1, potisioinName = "中央歩兵" },
                    new PotisionComboBox { potisioinId = 2, potisioinName = "右翼歩兵" },
                    new PotisionComboBox { potisioinId = 3, potisioinName = "左翼歩兵" },
                    //new PotisionComboBox { potisioinId = 4, potisioinName = "右翼騎兵" },
                    //new PotisionComboBox { potisioinId = 5, potisioinName = "左翼騎兵" },
                    //new PotisionComboBox { potisioinId = 6, potisioinName = "右翼投射" },
                    //new PotisionComboBox { potisioinId = 7, potisioinName = "左翼投射" },
                };

                if (App.techBean.getTechEnableBool(2))
                {
                    potisionComboBoxCollection.Add(new PotisionComboBox { potisioinId = 4, potisioinName = "右翼騎兵" });
                    potisionComboBoxCollection.Add(new PotisionComboBox { potisioinId = 5, potisioinName = "左翼騎兵" });
                }

                if (App.techBean.getTechEnableBool(3))
                {
                    potisionComboBoxCollection.Add(new PotisionComboBox { potisioinId = 6, potisioinName = "右翼投射" });
                    potisionComboBoxCollection.Add(new PotisionComboBox { potisioinId = 7, potisioinName = "左翼投射" });
                }

                // コンボボックスカラム作成
                // 諸々の理由でコンボボックスはDataGridTemplateColumnの中に入れる
                DataGridTemplateColumn countryColumn = new DataGridTemplateColumn();
                countryColumn.Header = "ポジション";

                FrameworkElementFactory comboBoxFactory = new FrameworkElementFactory(typeof(ComboBox));
                comboBoxFactory.SetValue(ComboBox.ItemsSourceProperty, potisionComboBoxCollection);
                // 利用する値
                comboBoxFactory.SetValue(ComboBox.SelectedValuePathProperty, "potisioinId");
                // 表示する名称
                comboBoxFactory.SetValue(ComboBox.DisplayMemberPathProperty, "potisioinName");
                // 初期表示値
                comboBoxFactory.SetValue(ComboBox.SelectedValueProperty, new Binding("troop.potisioinId"));
                // 変更時イベント
                comboBoxFactory.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(potisionComboBoxSelectionChanged));

                DataTemplate cellTemplate = new DataTemplate { VisualTree = comboBoxFactory };
                countryColumn.CellTemplate = cellTemplate;
                dataGrid.Columns.Add(countryColumn);

                // 攻撃順コンボボックス要素（別定義したい）
                ObservableCollection<OrderComboBox> orderComboBoxCollection = new ObservableCollection<OrderComboBox>
                {
                    new OrderComboBox { orderNum = 0, orderNumName = "" },
                    new OrderComboBox { orderNum = 1, orderNumName = "1" },
                    new OrderComboBox { orderNum = 2, orderNumName = "2" },
                    new OrderComboBox { orderNum = 3, orderNumName = "3" },
                    new OrderComboBox { orderNum = 4, orderNumName = "4" },
                    new OrderComboBox { orderNum = 5, orderNumName = "5" },
                    new OrderComboBox { orderNum = 6, orderNumName = "6" },
                    new OrderComboBox { orderNum = 7, orderNumName = "7" },
                };


                DataGridTemplateColumn orderNumCountryColumn = new DataGridTemplateColumn();
                orderNumCountryColumn.Header = "攻撃順";

                FrameworkElementFactory orderNumComboBoxFactory = new FrameworkElementFactory(typeof(ComboBox));
                orderNumComboBoxFactory.SetValue(ComboBox.ItemsSourceProperty, orderComboBoxCollection);
                orderNumComboBoxFactory.SetValue(ComboBox.DisplayMemberPathProperty, "orderNumName");
                orderNumComboBoxFactory.SetValue(ComboBox.SelectedValuePathProperty, "orderNum");
                orderNumComboBoxFactory.SetValue(ComboBox.SelectedValueProperty, new Binding("troop.orderNum"));
                orderNumComboBoxFactory.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(orderNumComboBoxSelectionChanged));

                DataTemplate orderNumCellTemplate = new DataTemplate { VisualTree = orderNumComboBoxFactory };
                orderNumCountryColumn.CellTemplate = orderNumCellTemplate;
                dataGrid.Columns.Add(orderNumCountryColumn);



                // グリッドにデータグリッドを入れ、グリッドをタブに入れる
                Grid grid1 = new Grid();
                grid1.Children.Add(dataGrid);

                var tabItem = new TabItem();
                tabItem.Header = corpGeneralList.First().corp.corpName;
                tabItem.Content = grid1;
                mainTab.Items.Add(tabItem);

                // 新規タブ作成
                dataGrid.ItemsSource = corpGeneralList;

                // dataGrid保存
                corpDatagridList.Insert(i, dataGrid);
                corpTabItemList.Insert(i, tabItem);
            }
        }

        public class YearsToEnabledConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int years = (int)value;
                return years > 1;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        // ポジションコンボボックス変更時イベント
        private void potisionComboBoxSelectionChanged(object sender, EventArgs e)
        {

            // コンボボックスか判定
            if (sender is ComboBox comboBox)
            {
                // 変更されたコンボボックスの一行のデータ取得
                GeneralAllDataGridDTO selectedGeneral = (GeneralAllDataGridDTO)comboBox.DataContext;

                // 変更後と変更前を比較し、変更していれば更新（タブ遷移した際も走ってしまうため）
                if (comboBox.SelectedItem is PotisionComboBox selectedCountry && selectedCountry.potisioinId != selectedGeneral.troop.potisioinId)
                {

                    TroopDao.updatePotisionId(selectedGeneral.troop.troopId, selectedCountry.potisioinId);

                    // 未配属の場合はorderNumも0に
                    if (selectedCountry.potisioinId == 0)
                    {
                        TroopDao.updateOrderNum(selectedGeneral.troop.troopId, 0);
                    }

                    // 一応画面再描画
                    reflashAllDataGrid(true);
                }
            }


        }

        // 隊列コンボボックス変更時イベント
        private void orderNumComboBoxSelectionChanged(object sender, EventArgs e)
        {

            // コンボボックスか判定
            if (sender is ComboBox comboBox)
            {
                // 変更されたコンボボックスの一行のデータ取得
                GeneralAllDataGridDTO selectedGeneral = (GeneralAllDataGridDTO)comboBox.DataContext;

                // 変更後と変更前を比較し、変更していれば更新（タブ遷移した際も走ってしまうため）
                if (comboBox.SelectedItem is OrderComboBox selectedCountry && selectedCountry.orderNum != selectedGeneral.troop.orderNum)
                {

                    TroopDao.updateOrderNum(selectedGeneral.troop.troopId, selectedCountry.orderNum);

                    // 一応画面再描画
                    reflashAllDataGrid(true);
                }
            }
        }

        private void Debug_Button_Click(object sender, RoutedEventArgs e)
        {

            GrowthGeneral.testMain();
            reflashAllDataGrid();

        }

        private void Debug_Button_Click2(object sender, RoutedEventArgs e)
        {

            var win = new ResignWindow();
            win.Show();

        }

        // 画面の活性/非活性
        public void mainWindowIsEnabled(bool b)
        {
            this.IsEnabled = b;
        }

        // 戦闘終了およびターン終了処理
        public void returnBattle()
        {

            // ターン終了処理
            TurnEndLogic.turnEnd();

            // 表示パラメータ初期化
            this.reflashAllDisplayParam();

            // 各一覧初期化
            this.reflashAllDataGrid();

            // 画面活性化
            this.mainWindowIsEnabled(true);

            // アドバイス表示
            advisorSubWindow.showAdvice();

            // オートセーブ
            saveDataLogic.saveGameData(SaveDataDao.getSaveData(999));

        }

        private void BattleDebugWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            // BattleDebugWindow表示
            var win = new BattleDebugWindow();
            win.Show();

        }

        private void GenerateDebugWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            // GenerateDebugWindow表示
            var win = new GenerateDebugWindow();
            win.Show();

        }

    }

    // ポジション指定コンボボックス要素定義クラス
    public class PotisionComboBox
    {
        public int potisioinId { get; set; }
        public string potisioinName { get; set; }
    }

    // 攻撃順指定コンボボックス要素定義クラス
    public class OrderComboBox
    {
        public int orderNum { get; set; }
        public string orderNumName { get; set; }
    }


}



