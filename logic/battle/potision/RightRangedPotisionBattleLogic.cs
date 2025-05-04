using ListSLG.bean;
using ListSLG.devtools;
using ListSLG.logic.battle.calc;
using ListSLG.logic.battle.potision;
using ListSLG.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace ListSLG.logic.battle
{
    internal class RightRangedPotisionBattleLogic : PotisionBattleLogicBase
    {

        public RightRangedPotisionBattleLogic(Troop onTroop, CorpIdentDiv corpIdentId) : base(onTroop, corpIdentId)
        {
            this.onPotisioin = PotisioinDiv.rightRanged;
        }


        public override BattleLogicResponse attack(CorpTroopBean onCorpTroopBean, CorpTroopBean opponentCorpTroopBean, CorpIdentDiv corpIdentId)
        {

            PotisioinDiv targetPos = PotisioinDiv.leftRanged;
            double posDamegeRatio = 1.0;

            // 左翼の部隊が全滅している場合は中央歩兵に攻撃
            if ((opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftRanged).soldierNum <= 0
                && opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry).soldierNum <= 0
                && opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum <= 0))
            {
                targetPos = PotisioinDiv.centorInfantry;
                posDamegeRatio = 0.8;

            } else
            {

                bool braleFlg = true;

                // 敵兵のいる部隊にあたるまでループを繰り返す
                while (braleFlg)
                {
                    Random choicerandom = new System.Random();
                    int choiceDice = choicerandom.Next(0, 3);

                    switch (choiceDice)
                    {
                        case 0:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftRanged).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.leftRanged;
                                posDamegeRatio = 1.0;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 1:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.leftCavalry;
                                posDamegeRatio = 1.0;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 2:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.leftInfantry;
                                posDamegeRatio = 0.4;
                                braleFlg = false;
                                break;
                            }
                            break;
                    }

                }

            }

            // ダメージ算出
            int damage = NormalBattleCalc.damegeCalc(onTroop, opponentCorpTroopBean.getTroopByPositionId((int)targetPos), posDamegeRatio);

            // ダメージ減算および結果更新
            BattleLogicResponse battleLogicResponse = damageCalcToResult(onCorpTroopBean, opponentCorpTroopBean, targetPos, damage);

            // ログ追加
            String logTextIdent = "";
            if (corpIdentId == CorpIdentDiv.allyCorp)
            {
                logTextIdent = "味方";
            }
            else
            {
                logTextIdent = "敵";
            }
            battleLogicResponse.logTextList.Add(logTextIdent + "右翼投射 " + onTroop.general.name + "隊の攻撃：" + opponentCorpTroopBean.getTroopByPositionId((int)targetPos).general.name + "隊に：" + damage + "人の損害を与えた");

            return battleLogicResponse;




        }


    }
}
