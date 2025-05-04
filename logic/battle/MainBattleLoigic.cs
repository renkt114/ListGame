using ListSLG.bean;
using ListSLG.devtools;
using ListSLG.logic.battle.calc;
using ListSLG.logic.battle.potision;
using ListSLG.model;
using ListSLG.window;
using ListSLG.window.subWindow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ListSLG.logic.battle
{
    class MainBattleLoigic
    {
        // 戦闘画面インスタンス
        BattleWindow battleWindow = new BattleWindow();
        // 戦闘ログ画面インスタンス
        BattleLogWindow battleLogWindow = new BattleLogWindow();

        // 部隊別のBattleLogicリスト
        Dictionary<int, PotisionBattleLogicBase> battleLogicDic = new Dictionary<int, PotisionBattleLogicBase>();

        // 味方軍団
        CorpTroopBean allyCorpBean = new CorpTroopBean();
        // 敵軍団
        CorpTroopBean enemyCorpBean = new CorpTroopBean();

        int delayTime = 500;

        Boolean continueFlg = true;

        // メイン処理
        public async void start(int delayTime = 500)
        {

            // ウェイト時間設定
            this.delayTime = delayTime;

            // 各画面インスタンス取得
            battleWindow = Application.Current.Windows.OfType<BattleWindow>().SingleOrDefault(x => x.IsActive);
            //battleLogWindow = Application.Current.Windows.OfType<BattleLogWindow>().SingleOrDefault(x => x.IsActive);
            // 何故か取れないのでこちら
            battleLogWindow = battleWindow.battleLogWindow;

            // 味方corp取得
            allyCorpBean = battleWindow.allyCorp;

            // 敵corp取得
            enemyCorpBean = battleWindow.enemyCorp;

            // バフデバフ
            buffDebuff(allyCorpBean);
            buffDebuff(enemyCorpBean);


            // 各ポジションのBattleLogicクラスの宣言
            // 兵科や陣形のカスタマイズがあれば、ここはfacrotyパターンになるだろう

            // 味方
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry).soldierNum > 0)
            {
                CenterInfantryPotisionBattleLogic allyCIBL = new CenterInfantryPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyCIBL.onTroop.troopId, allyCIBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry).soldierNum > 0)
            {
                RightInfantryPotisionBattleLogic allyRIBL = new RightInfantryPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyRIBL.onTroop.troopId, allyRIBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum > 0)
            {
                LeftInfantryPotisionBattleLogic allyLIBL = new LeftInfantryPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyLIBL.onTroop.troopId, allyLIBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry).soldierNum > 0)
            {
                RightCavalryPotisionBattleLogic allyRCBL = new RightCavalryPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyRCBL.onTroop.troopId, allyRCBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry).soldierNum > 0)
            {
                LeftCavalryPotisionBattleLogic allyLCBL = new LeftCavalryPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyLCBL.onTroop.troopId, allyLCBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightRanged).soldierNum > 0)
            {
                RightRangedPotisionBattleLogic allyRRBL = new RightRangedPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightRanged), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyRRBL.onTroop.troopId, allyRRBL);

            }
            if (allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftRanged).soldierNum > 0)
            {
                LeftRangedPotisionBattleLogic allyLRBL = new LeftRangedPotisionBattleLogic(allyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftRanged), CorpIdentDiv.allyCorp);
                this.battleLogicDic.Add(allyLRBL.onTroop.troopId, allyLRBL);

            }

            // 敵
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry).soldierNum > 0)
            {
                CenterInfantryPotisionBattleLogic enemyCIBL = new CenterInfantryPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyCIBL.onTroop.troopId, enemyCIBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry).soldierNum > 0)
            {
                RightInfantryPotisionBattleLogic enemyRIBL = new RightInfantryPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyRIBL.onTroop.troopId, enemyRIBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum > 0)
            {
                LeftInfantryPotisionBattleLogic enemyLIBL = new LeftInfantryPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyLIBL.onTroop.troopId, enemyLIBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry).soldierNum > 0)
            {
                RightCavalryPotisionBattleLogic enemyRCBL = new RightCavalryPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyRCBL.onTroop.troopId, enemyRCBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry).soldierNum > 0)
            {
                LeftCavalryPotisionBattleLogic enemyLCBL = new LeftCavalryPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyLCBL.onTroop.troopId, enemyLCBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightRanged).soldierNum > 0)
            {
                RightRangedPotisionBattleLogic enemyRRBL = new RightRangedPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.rightRanged), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyRRBL.onTroop.troopId, enemyRRBL);

            }
            if (enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftRanged).soldierNum > 0)
            {
                LeftRangedPotisionBattleLogic enemyLRBL = new LeftRangedPotisionBattleLogic(enemyCorpBean.getTroopByPositionId((int)PotisioinDiv.leftRanged), CorpIdentDiv.enemyCorp);
                this.battleLogicDic.Add(enemyLRBL.onTroop.troopId, enemyLRBL);

            }


            // 戦闘終了まで繰り返す。分岐は後で考える
            while (continueFlg)
            {

                // ターン毎の順番発番。とりあえずmaneuverAbility順
                // Dictionary<int, PotisionBattleLogicBase> orderBattleLogicDic = this.battleLogicDic.OrderByDescending(x => x.Value.onTroop.general.maneuverAbility).ToDictionary(x => x.Key, x => x.Value); ;

                // ターン毎の順番発番。とりあえずorderNum順
                Dictionary<int, PotisionBattleLogicBase> orderBattleLogicDic = this.battleLogicDic.OrderBy(x => x.Value.onTroop.orderNum).ToDictionary(x => x.Key, x => x.Value); ;


                foreach (PotisionBattleLogicBase battleLogic in orderBattleLogicDic.Values)
                {
                    // 順番が回ってきた部隊のBattleLogic実行
                    executeBattleLogic(battleLogic);
                    // 画面描画のための待機
                    await DelayedProcessingAsync();

                    // どちらかの中央歩兵部隊が全滅したら終了
                    if((0 >= allyCorpBean.troop1.soldierNum) || (0 >= enemyCorpBean.troop1.soldierNum))
                    {
                        continueFlg = false;
                        break;
                    }

                }

            }

            battleWindow.BattleEnd();


        }

        // BattleLogic実行
        private void executeBattleLogic(PotisionBattleLogicBase battleLogic)
        {

            // battleLogic取得
            //PotisionBattleLogicBase battleLogic = battleLogicDic[troopId];

            // レスポンス宣言
            BattleLogicResponse battleLogicResponse = new BattleLogicResponse();


            // ロジックが持つ敵味方IDを元に敵のCorpTroopBeanを引数に入れ、結果を反映（もう少しスマートにできそうだが・・・）
            if (battleLogic.corpIdentId == CorpIdentDiv.allyCorp)
            {
                battleLogicResponse = battleLogic.attack(allyCorpBean, enemyCorpBean, CorpIdentDiv.allyCorp);
                allyCorpBean = battleLogicResponse.onCorpTroopBean;
                enemyCorpBean = battleLogicResponse.OpponentCorpTroopBean;
            } else
            {
                battleLogicResponse = battleLogic.attack(enemyCorpBean, allyCorpBean, CorpIdentDiv.enemyCorp);
                enemyCorpBean = battleLogicResponse.onCorpTroopBean;
                allyCorpBean = battleLogicResponse.OpponentCorpTroopBean;
            }

            // レスポンスのログまとめて出力
            foreach(String logText in battleLogicResponse.logTextList)
            {
                battleLogWindow.addLine(logText);
            }

            Debug.Print("result:" + allyCorpBean.result1.atkTimes + "," + allyCorpBean.result1.atkSumNum);

            // 画面更新
            Application.Current.Dispatcher.Invoke(() =>
            {
                battleWindow.allyCorp = allyCorpBean;
                battleWindow.enemyCorp = enemyCorpBean;
                battleWindow.setAllTroop();
                battleWindow.animationAttack(battleLogic.corpIdentId, battleLogic.onPotisioin);
            });


        }


        //ウェイト処理
        private async Task DelayedProcessingAsync()
        {
            await Task.Delay(this.delayTime);
        }

        // ログ出力
        private void addBattleLog(Troop attackTroop, Troop targetTroop, int damage)
        {

            string log = attackTroop.general.name + "隊の攻撃：" + targetTroop.general.name + "隊に：" + damage + "人の損害を与えた";
            battleLogWindow.addLine(log);

        }

        // バフ・デバフ処理
        private void buffDebuff(CorpTroopBean corpTroopBean)
        {

            General leader = corpTroopBean.troop1.general;

            foreach (PotisioinDiv position in Enum.GetValues(typeof(PotisioinDiv)))
            {

                // ややこしいが、corpTroopBeanからtroopを取得し、そのtroopのgeneralにリーダーバフをかけて、再度corpTroopBeanにセットする
                // 参照型なのでこうしなくてもいいがわかりやすさ優先
                Troop troop = corpTroopBean.getTroopByPositionId((int)position);

                // リーダーにはリーダーバフをかけない
                if (position != PotisioinDiv.centorInfantry)
                {
                    troop.general = BuffDebuffCalc.leaderBuff(troop.general, leader);
                }

                troop.general = BuffDebuffCalc.injuredDeBuff(troop.general);
                troop.general = BuffDebuffCalc.conditionBuff(troop.general);

                // 歩兵強化技術有効の場合
                if(App.techBean.getTechEnableBool(7) && ( position == PotisioinDiv.centorInfantry || position == PotisioinDiv.rightInfantry || position == PotisioinDiv.rightInfantry))
                {
                    troop.general = BuffDebuffCalc.infTechBuff(troop.general);
                }

                // 騎兵強化技術有効の場合
                if (App.techBean.getTechEnableBool(8) && (position == PotisioinDiv.rightCavalry || position == PotisioinDiv.leftCavalry))
                {
                    troop.general = BuffDebuffCalc.cavTechBuff(troop.general);
                }

                // 投射強化技術有効の場合
                if (App.techBean.getTechEnableBool(9) && (position == PotisioinDiv.rightRanged || position == PotisioinDiv.leftRanged))
                {
                    troop.general = BuffDebuffCalc.rangeTechBuff(troop.general);
                }


                corpTroopBean.setTroopByPositionId((int)position, troop);


            }
        }


    }
}
