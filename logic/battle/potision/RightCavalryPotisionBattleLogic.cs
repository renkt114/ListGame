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
    internal class RightCavalryPotisionBattleLogic : PotisionBattleLogicBase
    {

        public RightCavalryPotisionBattleLogic(Troop onTroop, CorpIdentDiv corpIdentId) : base(onTroop, corpIdentId)
        {
            this.onPotisioin = PotisioinDiv.rightCavalry;
        }

        public override BattleLogicResponse attack(CorpTroopBean onCorpTroopBean, CorpTroopBean opponentCorpTroopBean, CorpIdentDiv corpIdentId)
        {

            // 兵数0の場合は行動なし
            if (0 >= onCorpTroopBean.getTroopByPositionId((int)this.onPotisioin).soldierNum)
            {
                return new BattleLogicResponse()
                {
                    corpIdentId = this.corpIdentId,
                    onCorpTroopBean = onCorpTroopBean,
                    OpponentCorpTroopBean = opponentCorpTroopBean,
                    logTextList = new List<String>()

                };
            }


            PotisioinDiv targetPos = PotisioinDiv.leftInfantry;
            double posDamegeRatio = 1.0;

            // 左翼歩兵ブロック判定
            if (blockCalc(onTroop, opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry), 30))
            {
                // 左翼歩兵にアタック
                targetPos = PotisioinDiv.leftInfantry;
                posDamegeRatio = 0.5;
                // ブロック成功を結果に加算
                opponentCorpTroopBean = updateToBlock(opponentCorpTroopBean, targetPos);

                // 左翼騎兵ブロック判定
            }
            else if (blockCalc(onTroop, opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry), 20))
            {
                // 左翼歩兵にアタック
                targetPos = PotisioinDiv.leftCavalry;
                posDamegeRatio = 1.0;
                // ブロック成功を結果に加算
                opponentCorpTroopBean = updateToBlock(opponentCorpTroopBean, targetPos);

            }
            // ブロックを回避した場合
            else
            {
                // ここから左翼歩兵（いなければ中央）、右翼騎兵、右翼弓兵のどれを攻撃するか決める。
                // 可能なら作戦設定で優先順位を決めたいが、とりあえずランダム。

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
                                posDamegeRatio = 1.6;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 1:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftCavalry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.leftCavalry;
                                posDamegeRatio = 1.2;
                                braleFlg = false;
                                break;
                            }
                            break;
                        case 2:
                            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum > 0)
                            {
                                targetPos = PotisioinDiv.leftInfantry;
                                posDamegeRatio = 1.2;
                                braleFlg = false;
                                break;
                            }
                            else
                            {

                                targetPos = PotisioinDiv.centorInfantry;
                                posDamegeRatio = 1.2;
                                braleFlg = false;
                                break;

                            }
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
            battleLogicResponse.logTextList.Add(logTextIdent + "右翼騎兵 " + onTroop.general.name + "隊の攻撃：" + opponentCorpTroopBean.getTroopByPositionId((int)targetPos).general.name + "隊に：" + damage + "人の損害を与えた");

            return battleLogicResponse;


        }

    }
}
