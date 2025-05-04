using ListSLG.bean;
using ListSLG.devtools;
using ListSLG.logic.battle.calc;
using ListSLG.logic.battle.potision;
using ListSLG.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ListSLG.logic.battle
{
    internal class LeftRangedPotisionBattleLogic : PotisionBattleLogicBase
    {

        public LeftRangedPotisionBattleLogic(Troop onTroop, CorpIdentDiv corpIdentId) : base(onTroop, corpIdentId)
        {
            this.onPotisioin = PotisioinDiv.leftRanged;
        }


        public override BattleLogicResponse attack(CorpTroopBean onCorpTroopBean, CorpTroopBean opponentCorpTroopBean, CorpIdentDiv corpIdentId)
        {

            PotisioinDiv targetPos = PotisioinDiv.rightRanged;
            double posDamegeRatio = 1.0;


            // 右翼の部隊が全滅している場合は中央歩兵に攻撃
            if ((opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightRanged).soldierNum <= 0
                && opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry).soldierNum <= 0
                && opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry).soldierNum <= 0))
            {
                targetPos = PotisioinDiv.centorInfantry;
                posDamegeRatio = 0.8;
            }
            else
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
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightRanged).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.rightRanged;
                                posDamegeRatio = 1.0;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 1:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightCavalry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.rightCavalry;
                                posDamegeRatio = 1.0;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 2:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.rightInfantry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.rightInfantry;
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
            battleLogicResponse.logTextList.Add(logTextIdent + "左翼投射 " + onTroop.general.name + "隊の攻撃：" + opponentCorpTroopBean.getTroopByPositionId((int)targetPos).general.name + "隊に：" + damage + "人の損害を与えた");

            return battleLogicResponse;




        }


    }
}
