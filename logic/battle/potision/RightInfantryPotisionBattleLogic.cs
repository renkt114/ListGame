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
    class RightInfantryPotisionBattleLogic : PotisionBattleLogicBase
    {

        public RightInfantryPotisionBattleLogic(Troop onTroop, CorpIdentDiv corpIdentId) : base(onTroop, corpIdentId)
        {
            this.onPotisioin = PotisioinDiv.rightInfantry;
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

            // 敵leftが存在する場合はleft
            if (opponentCorpTroopBean.getTroopByPositionId((int)PotisioinDiv.leftInfantry).soldierNum > 0)
            {
                targetPos = PotisioinDiv.leftInfantry;
                posDamegeRatio = 0.7;
            }
            // 存在しないならcentor
            else 
            {
                targetPos = PotisioinDiv.centorInfantry;
                posDamegeRatio = 0.8;
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
            battleLogicResponse.logTextList.Add(logTextIdent + "右翼歩兵 " + onTroop.general.name + "隊の攻撃：" + opponentCorpTroopBean.getTroopByPositionId((int)targetPos).general.name + "隊に：" + damage + "人の損害を与えた");

            return battleLogicResponse;


        }

    }
}
