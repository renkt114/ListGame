using ListSLG.bean;
using ListSLG.logic.battle;
using ListSLG.logic.battle.calc;
using ListSLG.logic.generate;
using ListSLG.logic.resource;
using ListSLG.model;
using ListSLG.window.subWindow;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static ListSLG.model.CorpPlanDao;

namespace ListSLG.window
{
    /// <summary>
    /// BattleWindow.xaml の相互作用ロジック
    /// </summary>
    /// 



    public partial class BattleWindow : Window
    {

        // 現戦闘中味方軍団
        internal CorpTroopBean allyCorp = new CorpTroopBean();
        // 現戦闘中敵軍団
        internal CorpTroopBean enemyCorp = new CorpTroopBean();
        // ログウィンドウインスタンス
        public BattleLogWindow battleLogWindow = new BattleLogWindow();

        internal WindowTroopNumBean windowTroopNumBean = new WindowTroopNumBean();

        // 実行中の計画No
        internal int planCnt = 0;
        // 登録された全計画および任務情報
        internal List<CorpPlanMissoinDTO> allCorpPlanMissoin;

        public BattleWindow()
        {
            InitializeComponent();

            //  登録された全計画および任務情報取得
            allCorpPlanMissoin = CorpPlanDao.getAllCorpPlanMissoin();

            // 画面描画後にもろもろの処理を行う
            ContentRendered += windowLoaded;

        }

        // 画面描画後の処理（ログウィンドウを表示するため描画後に動かす）
        private void windowLoaded(object sender, EventArgs e)
        {
            //戦闘画面の右にログ表示
            battleLogWindow.Left = this.Left + this.Width;
            battleLogWindow.Top = this.Top;
            battleLogWindow.Show();

            // もろもろ設定
            battleSetting();


        }

        // 計画毎の軍団セッティングおよび開始メッセージ
        public void battleSetting()
        {

            battleLogWindow.clearLine();

            // allCorpPlanMissoin[planCnt]がnullの場合は戦闘終了
            if (planCnt >= allCorpPlanMissoin.Count )
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.returnBattle();
                }
                battleLogWindow.close();
                this.Close();

                return;
            }

            // 味方軍団　corpPlan.corpIdから取得
            allyCorp.initCorpTroopData(allCorpPlanMissoin[planCnt].corpPlan.corpId);
            // 敵軍団　corpPlanに紐づくmission.enemyCorpIdから取得
            //enemyCorp.initCorpTroopData(allCorpPlanMissoin[planCnt].mission.enemyCorpId);
            CorpGeneralLogic corpGeneralLogic = new CorpGeneralLogic(allCorpPlanMissoin[planCnt].mission.generalNum, allCorpPlanMissoin[planCnt].mission.level);
            enemyCorp.initCorpTroopData(corpGeneralLogic.EnemyCorpCreate());
            // 全部隊設定
            setAllTroop();
            // 開始メッセージ表示
            MessageBox.Show(this, allCorpPlanMissoin[planCnt].mission.missionName, "タイトル", MessageBoxButton.OK, MessageBoxImage.Information);

            //ボタンを再表示
            BattleStartShortButton.Visibility = Visibility.Visible;
            BattleStartButton.Visibility = Visibility.Visible;

        }

        // 全部隊設定　兵数やアニメーションの反映も行うため、ダメージ計算が発生するたびに呼び出される
        public void setAllTroop()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                AllyCISolNumText.Text = allyCorp.troop1.soldierNum + "/" + allyCorp.troop1.maxSoldierNum;
                AllyCISolNumBar.Value = calcsoldierRatio(allyCorp.troop1.soldierNum, allyCorp.troop1.maxSoldierNum);
                AllyCISolName.Text = allyCorp.troop1.general.name;

                // 兵数ダメージ表示
                if(windowTroopNumBean.AllyCINum > allyCorp.troop1.soldierNum)
                {
                    ShowTextRelativeTo(AllyCIImage, (allyCorp.troop1.soldierNum - windowTroopNumBean.AllyCINum).ToString());
                }
                windowTroopNumBean.AllyCINum = allyCorp.troop1.soldierNum;

                // 全滅判定
                if (allyCorp.troop1.soldierNum <= 0)
                {
                    AllyCIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyCIImage.Visibility = Visibility.Visible;
                }

                AllyRISolNumText.Text = allyCorp.troop2.soldierNum + "/" + allyCorp.troop2.maxSoldierNum;
                AllyRISolNumBar.Value = calcsoldierRatio(allyCorp.troop2.soldierNum, allyCorp.troop2.maxSoldierNum);
                AllyRISolName.Text = allyCorp.troop2.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyRINum > allyCorp.troop2.soldierNum)
                {
                    ShowTextRelativeTo(AllyRIImage, (allyCorp.troop2.soldierNum - windowTroopNumBean.AllyRINum).ToString());
                }
                windowTroopNumBean.AllyRINum = allyCorp.troop2.soldierNum;

                if (allyCorp.troop2.soldierNum <= 0)
                {
                    AllyRIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyRIImage.Visibility = Visibility.Visible;
                }

                AllyLISolNumText.Text = allyCorp.troop3.soldierNum + "/" + allyCorp.troop3.maxSoldierNum;
                AllyLISolNumBar.Value = calcsoldierRatio(allyCorp.troop3.soldierNum, allyCorp.troop3.maxSoldierNum);
                AllyLISolName.Text = allyCorp.troop3.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyLINum > allyCorp.troop3.soldierNum)
                {
                    ShowTextRelativeTo(AllyLIImage, (allyCorp.troop3.soldierNum - windowTroopNumBean.AllyLINum).ToString());
                }
                windowTroopNumBean.AllyLINum = allyCorp.troop3.soldierNum;
                
                if (allyCorp.troop3.soldierNum <= 0)
                {
                    AllyLIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyLIImage.Visibility = Visibility.Visible;
                }

                AllyRCSolNumText.Text = allyCorp.troop4.soldierNum + "/" + allyCorp.troop4.maxSoldierNum;
                AllyRCSolNumBar.Value = calcsoldierRatio(allyCorp.troop4.soldierNum, allyCorp.troop4.maxSoldierNum);
                AllyRCSolName.Text = allyCorp.troop4.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyRCNum > allyCorp.troop4.soldierNum)
                {
                    ShowTextRelativeTo(AllyRCImage, (allyCorp.troop4.soldierNum - windowTroopNumBean.AllyRCNum).ToString());
                }
                windowTroopNumBean.AllyRCNum = allyCorp.troop4.soldierNum;

                if (allyCorp.troop4.soldierNum <= 0)
                {
                    AllyRCImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyRCImage.Visibility = Visibility.Visible;
                }

                AllyLCSolNumText.Text = allyCorp.troop5.soldierNum + "/" + allyCorp.troop5.maxSoldierNum;
                AllyLCSolNumBar.Value = calcsoldierRatio(allyCorp.troop5.soldierNum, allyCorp.troop5.maxSoldierNum);
                AllyLCSolName.Text = allyCorp.troop5.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyLCNum > allyCorp.troop5.soldierNum)
                {
                    ShowTextRelativeTo(AllyLCImage, (allyCorp.troop5.soldierNum - windowTroopNumBean.AllyLCNum).ToString());
                }
                windowTroopNumBean.AllyLCNum = allyCorp.troop5.soldierNum;

                if (allyCorp.troop5.soldierNum <= 0)
                {
                    AllyLCImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyLCImage.Visibility = Visibility.Visible;
                }

                AllyRRSolNumText.Text = allyCorp.troop6.soldierNum + "/" + allyCorp.troop6.maxSoldierNum;
                AllyRRSolNumBar.Value = calcsoldierRatio(allyCorp.troop6.soldierNum, allyCorp.troop6.maxSoldierNum);
                AllyRRSolName.Text = allyCorp.troop6.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyRRNum > allyCorp.troop6.soldierNum)
                {
                    ShowTextRelativeTo(AllyRRImage, (allyCorp.troop6.soldierNum - windowTroopNumBean.AllyRRNum).ToString());
                }
                windowTroopNumBean.AllyRRNum = allyCorp.troop6.soldierNum;

                if (allyCorp.troop6.soldierNum <= 0)
                {
                    AllyRRImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyRRImage.Visibility = Visibility.Visible;
                }

                AllyLRSolNumText.Text = allyCorp.troop7.soldierNum + "/" + allyCorp.troop7.maxSoldierNum;
                AllyLRSolNumBar.Value = calcsoldierRatio(allyCorp.troop7.soldierNum, allyCorp.troop7.maxSoldierNum);
                AllyLRSolName.Text = allyCorp.troop7.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.AllyLRNum > allyCorp.troop7.soldierNum)
                {
                    ShowTextRelativeTo(AllyLRImage, (allyCorp.troop7.soldierNum - windowTroopNumBean.AllyLRNum).ToString());
                }
                windowTroopNumBean.AllyLRNum = allyCorp.troop7.soldierNum;

                if (allyCorp.troop7.soldierNum <= 0)
                {
                    AllyLRImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    AllyLRImage.Visibility = Visibility.Visible;
                }


                EnemyCISolNumText.Text = enemyCorp.troop1.soldierNum + "/" + enemyCorp.troop1.maxSoldierNum;
                EnemyCISolNumBar.Value = calcsoldierRatio(enemyCorp.troop1.soldierNum, enemyCorp.troop1.maxSoldierNum);
                EnemyCISolName.Text = enemyCorp.troop1.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyCINum > enemyCorp.troop1.soldierNum)
                {
                    ShowTextRelativeTo(EnemyCIImage, (enemyCorp.troop1.soldierNum - windowTroopNumBean.EnemyCINum).ToString());
                }
                windowTroopNumBean.EnemyCINum = enemyCorp.troop1.soldierNum;

                if (enemyCorp.troop1.soldierNum <= 0)
                {
                    EnemyCIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyCIImage.Visibility = Visibility.Visible;
                }

                EnemyRISolNumText.Text = enemyCorp.troop2.soldierNum + "/" + enemyCorp.troop2.maxSoldierNum;
                EnemyRISolNumBar.Value = calcsoldierRatio(enemyCorp.troop2.soldierNum, enemyCorp.troop2.maxSoldierNum);
                EnemyRISolName.Text = enemyCorp.troop2.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyRINum > enemyCorp.troop2.soldierNum)
                {
                    ShowTextRelativeTo(EnemyRIImage, (enemyCorp.troop2.soldierNum - windowTroopNumBean.EnemyRINum).ToString());
                }
                windowTroopNumBean.EnemyRINum = enemyCorp.troop2.soldierNum;

                if (enemyCorp.troop2.soldierNum <= 0)
                {
                    EnemyRIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyRIImage.Visibility = Visibility.Visible;
                }

                EnemyLISolNumText.Text = enemyCorp.troop3.soldierNum + "/" + enemyCorp.troop3.maxSoldierNum;
                EnemyLISolNumBar.Value = calcsoldierRatio(enemyCorp.troop3.soldierNum, enemyCorp.troop3.maxSoldierNum);
                EnemyLISolName.Text = enemyCorp.troop3.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyLINum > enemyCorp.troop3.soldierNum)
                {
                    ShowTextRelativeTo(EnemyLIImage, (enemyCorp.troop3.soldierNum - windowTroopNumBean.EnemyLINum).ToString());
                }
                windowTroopNumBean.EnemyLINum = enemyCorp.troop3.soldierNum;

                if (enemyCorp.troop3.soldierNum <= 0)
                {
                    EnemyLIImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyLIImage.Visibility = Visibility.Visible;
                }

                EnemyRCSolNumText.Text = enemyCorp.troop4.soldierNum + "/" + enemyCorp.troop4.maxSoldierNum;
                EnemyRCSolNumBar.Value = calcsoldierRatio(enemyCorp.troop4.soldierNum, enemyCorp.troop4.maxSoldierNum);
                EnemyRCSolName.Text = enemyCorp.troop4.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyRCNum > enemyCorp.troop4.soldierNum)
                {
                    ShowTextRelativeTo(EnemyRCImage, (enemyCorp.troop4.soldierNum - windowTroopNumBean.EnemyRCNum).ToString());
                }
                windowTroopNumBean.EnemyRCNum = enemyCorp.troop4.soldierNum;

                if (enemyCorp.troop4.soldierNum <= 0)
                {
                    EnemyRCImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyRCImage.Visibility = Visibility.Visible;
                }

                EnemyLCSolNumText.Text = enemyCorp.troop5.soldierNum + "/" + enemyCorp.troop5.maxSoldierNum;
                EnemyLCSolNumBar.Value = calcsoldierRatio(enemyCorp.troop5.soldierNum, enemyCorp.troop5.maxSoldierNum);
                EnemyLCSolName.Text = enemyCorp.troop5.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyLCNum > enemyCorp.troop5.soldierNum)
                {
                    ShowTextRelativeTo(EnemyLCImage, (enemyCorp.troop5.soldierNum - windowTroopNumBean.EnemyLCNum).ToString());
                }
                windowTroopNumBean.EnemyLCNum = enemyCorp.troop5.soldierNum;

                if (enemyCorp.troop5.soldierNum <= 0)
                {
                    EnemyLCImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyLCImage.Visibility = Visibility.Visible;
                }

                EnemyRRSolNumText.Text = enemyCorp.troop6.soldierNum + "/" + enemyCorp.troop6.maxSoldierNum;
                EnemyRRSolNumBar.Value = calcsoldierRatio(enemyCorp.troop6.soldierNum, enemyCorp.troop6.maxSoldierNum);
                EnemyRRSolName.Text = enemyCorp.troop6.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyRRNum > enemyCorp.troop6.soldierNum)
                {
                    ShowTextRelativeTo(EnemyRRImage, (enemyCorp.troop6.soldierNum - windowTroopNumBean.EnemyRRNum).ToString());
                }
                windowTroopNumBean.EnemyRRNum = enemyCorp.troop6.soldierNum;

                if (enemyCorp.troop6.soldierNum <= 0)
                {
                    EnemyRRImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyRRImage.Visibility = Visibility.Visible;
                }

                EnemyLRSolNumText.Text = enemyCorp.troop7.soldierNum + "/" + enemyCorp.troop7.maxSoldierNum;
                EnemyLRSolNumBar.Value = calcsoldierRatio(enemyCorp.troop7.soldierNum, enemyCorp.troop7.maxSoldierNum);
                EnemyLRSolName.Text = enemyCorp.troop7.general.name;

                // 兵数ダメージ表示
                if (windowTroopNumBean.EnemyLRNum > enemyCorp.troop7.soldierNum)
                {
                    ShowTextRelativeTo(EnemyLRImage, (enemyCorp.troop7.soldierNum - windowTroopNumBean.EnemyLRNum).ToString());
                }
                windowTroopNumBean.EnemyLRNum = enemyCorp.troop7.soldierNum;

                if (enemyCorp.troop7.soldierNum <= 0)
                {
                    EnemyLRImage.Visibility = Visibility.Hidden;
                }
                else
                {
                    EnemyLRImage.Visibility = Visibility.Visible;
                }
            });





        }

        // 兵数バーの計算
        private int calcsoldierRatio(int nowSoldierNum, int maxSoldierNum)
        {
            if (nowSoldierNum > 0 && maxSoldierNum > 0)
            {
                return (int)Math.Round((double)nowSoldierNum / maxSoldierNum * 100);
            } else
            {
                return 0;
            }
            

        }



        private void Battle_Start_Normal(object sender, RoutedEventArgs e)
        {
            //ボタンを非表示
            BattleStartShortButton.Visibility = Visibility.Hidden;
            BattleStartButton.Visibility = Visibility.Hidden;
            MainBattleLoigic mainBattleLoigic = new MainBattleLoigic();
            mainBattleLoigic.start();


        }

        private void Battle_Start_Short(object sender, RoutedEventArgs e)
        {
            //ボタンを非表示
            BattleStartShortButton.Visibility = Visibility.Hidden;
            BattleStartButton.Visibility = Visibility.Hidden;

            MainBattleLoigic mainBattleLoigic = new MainBattleLoigic();
            mainBattleLoigic.start(50);

        }

        // 戦闘終了処理
        public void BattleEnd()
        {
            // 威信加算
            PrestigeLogic.battleEndPrestige(allCorpPlanMissoin[planCnt].mission.gainedPrestige, enemyCorp);

            // 怪我処理
            foreach (PotisioinDiv position in Enum.GetValues(typeof(PotisioinDiv)))
            {

                Troop troop = allyCorp.getTroopByPositionId((int)position);

                // 兵数0は未設定としてスキップ
                if(troop.maxSoldierNum == 0)
                {
                    continue;
                }

                Random random = new Random();
                int injury = random.Next(0, 100);

                // 基本確率 2 + 兵数損害割合 * 3 - physicalAbility / 100 * 5
                double injurline = 2.0 + ((1 - troop.soldierNum / troop.maxSoldierNum) * 3) - ((troop.general.physicalAbility / 100) * 5);

                if (injury < injurline)
                {
                    // 怪我　allyCorpの中身は何を更新しているのかわからないので新しく取りに行く
                    General injurGeneral = GeneralDao.getGeneralByGeneralId(troop.general.generalId);
                    injurGeneral.injured += 2;
                    GeneralDao.updateGeneral(injurGeneral);

                }

            }

            // 結果画面表示
            BattleResultWindow battleResultWindow = new BattleResultWindow(allyCorp);
            battleResultWindow.Show();

            // 計画Noを次に進める
            planCnt += 1;

        }

        public void animationAttack(CorpIdentDiv corpIdentDiv, PotisioinDiv potisioinDiv)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                // 攻撃側のアニメーション
                TranslateTransform translateTransform = new TranslateTransform();
                switch (corpIdentDiv)
                {
                    case CorpIdentDiv.allyCorp:
                        switch (potisioinDiv)
                        {
                            case PotisioinDiv.centorInfantry:
                                AllyCIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightInfantry:
                                AllyRIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftInfantry:
                                AllyLIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightCavalry:
                                AllyRCImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftCavalry:
                                AllyLCImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightRanged:
                                AllyRRImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftRanged:
                                AllyLRImage.RenderTransform = translateTransform;
                                break;
                        }
                        break;
                    case CorpIdentDiv.enemyCorp:
                        switch (potisioinDiv)
                        {
                            case PotisioinDiv.centorInfantry:
                                EnemyCIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightInfantry:
                                EnemyRIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftInfantry:
                                EnemyLIImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightCavalry:
                                EnemyRCImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftCavalry:
                                EnemyLCImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.rightRanged:
                                EnemyRRImage.RenderTransform = translateTransform;
                                break;
                            case PotisioinDiv.leftRanged:
                                EnemyLRImage.RenderTransform = translateTransform;
                                break;
                        }
                        break;
                }

                MoveImage(translateTransform, corpIdentDiv);
            });

        }

        // 攻撃アニメーション
        private void MoveImage(TranslateTransform translateTransform, CorpIdentDiv corpIdentDiv)
        {

            int toMove = 10;
            if (corpIdentDiv == CorpIdentDiv.enemyCorp)
            {
                toMove = -10;
            }

            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = toMove, // 横に10ピクセル移動
                Duration = new Duration(TimeSpan.FromSeconds(0.3)), // 0.3秒で移動
                AutoReverse = true, // 元に戻る
                RepeatBehavior = new RepeatBehavior(1) // 一度だけ再生
            };

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        // ユニット毎テキスト表示（主に兵数消費）
        private void ShowTextRelativeTo(UIElement relativeElement, string message)
        {
            // メッセージを設定
            FadeTextBlock.Text = message;
            FadeTextBlock.Visibility = Visibility.Visible;
            FadeTextBlock.Opacity = 1.0;

            // 基準となる要素の位置を取得
            var relativePosition = relativeElement.TransformToAncestor(this)
                .Transform(new Point(0, 0));

            // TextBlockの位置を、基準要素の相対位置に設定
            FadeTextBlock.Margin = new Thickness(
                relativePosition.X, // 基準要素の右側に少しずらして配置
                relativePosition.Y,
               5, 5);

            // フェードアウトアニメーションを作成
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1),
                BeginTime = TimeSpan.FromSeconds(0) // 1秒後にフェードアウト開始
            };

            // アニメーション完了時に非表示にする
            fadeOutAnimation.Completed += (s, e) =>
            {
                FadeTextBlock.Visibility = Visibility.Collapsed;
            };

            // TextBlockにアニメーションを適用
            FadeTextBlock.BeginAnimation(OpacityProperty, fadeOutAnimation);


            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = -10, // 横に10ピクセル移動
                Duration = new Duration(TimeSpan.FromSeconds(0.3)), // 0.3秒で移動
                RepeatBehavior = new RepeatBehavior(1) // 一度だけ再生
            };

            FadeTextBlock.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

    }
}
