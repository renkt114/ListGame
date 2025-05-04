using ListSLG.bean;
using ListSLG.model;
using ListSLG.window.subWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.battle.potision
{
    abstract class PotisionBattleLogicBase
    {

        public CorpIdentDiv corpIdentId { get; }
        internal Troop onTroop { get; }

        public PotisioinDiv onPotisioin { get; set; }

        internal PotisionBattleLogicBase(Troop onTroop, CorpIdentDiv corpIdentId)
        {
            this.onTroop = onTroop;
            this.corpIdentId = corpIdentId;
        }

        // abstract 攻撃処理
        public abstract BattleLogicResponse attack(CorpTroopBean onCorpTroopBean, CorpTroopBean opponentCorpTroopBean, CorpIdentDiv corpIdentId);

        // ブロック判定共通処理
        // 自maneuver + 1～100の乱数 - 敵maneuver <= maneuverdiff trueならブロック
        // maneuverdifが大きいほど相手のブロックが成功しやすくなる（防御側有利）
        // 同じ能力なら乱数だけで決まるので50なら2回に1回成功とみてよい、100や0を超えると運だけではどうにもならない
        public bool blockCalc(Troop onTroop,Troop opponentTroop, int maneuverdiff)
        {

            Random random = new System.Random();
            int targetDice = random.Next(0, 99) + onTroop.general.maneuverAbility;

            if (opponentTroop.soldierNum >= 1  && (targetDice - opponentTroop.general.maneuverAbility) <= maneuverdiff)
            {
                return true;
            }

            return false;
           
        }

        // ダメージ計算共通処理
        public BattleLogicResponse damageCalcToResult(CorpTroopBean onCorpTroopBean, CorpTroopBean opponentCorpTroopBean, PotisioinDiv targetPos, int damage)
        {
            // ダメージ減算
            if(damage > opponentCorpTroopBean.getTroopByPositionId((int)targetPos).soldierNum)
            {
                damage = opponentCorpTroopBean.getTroopByPositionId((int)targetPos).soldierNum;
            }

            opponentCorpTroopBean.getTroopByPositionId((int)targetPos).soldierNum -= damage;
            if(0 > opponentCorpTroopBean.getTroopByPositionId((int)targetPos).soldierNum)
            {
                // ありえないが、0以下になったら0
                opponentCorpTroopBean.getTroopByPositionId((int)targetPos).soldierNum = 0;
            }

            // 攻撃側の結果加算
            onCorpTroopBean = updateOnResult(onCorpTroopBean, this.onPotisioin, damage);
            // 防御側の結果加算
            opponentCorpTroopBean = updateToResult(opponentCorpTroopBean, targetPos, damage);

            //　レスポンス作成
            return new BattleLogicResponse()
            {
                corpIdentId = this.corpIdentId,
                onCorpTroopBean = onCorpTroopBean,
                OpponentCorpTroopBean = opponentCorpTroopBean,
                logTextList = new List<String>()

            };
        }

        // 集計共通処理（攻撃側）
        public CorpTroopBean updateOnResult(CorpTroopBean onCorpTroopBean, PotisioinDiv potisioin, int damage)
        {

            onCorpTroopBean.getResultByPositionId((int)potisioin).atkTimes += 1;
            onCorpTroopBean.getResultByPositionId((int)potisioin).atkSumNum += damage;

            return onCorpTroopBean;

        }

        // 集計共通処理（防御側）
        public CorpTroopBean updateToResult(CorpTroopBean opponentCorpTroopBean, PotisioinDiv targetPos, int damage)
        {

            opponentCorpTroopBean.getResultByPositionId((int)targetPos).defTimes += 1;
            opponentCorpTroopBean.getResultByPositionId((int)targetPos).defSumNum += damage;

            return opponentCorpTroopBean;

        }

        // 集計共通処理（防御側ブロック）
        public CorpTroopBean updateToBlock(CorpTroopBean opponentCorpTroopBean, PotisioinDiv blockPos)
        {

            opponentCorpTroopBean.getResultByPositionId((int)blockPos).blockTimes += 1;
            return opponentCorpTroopBean;

        }


    }

    public class BattleLogicResponse
    {
        internal CorpIdentDiv corpIdentId { get; set; }
        internal CorpTroopBean onCorpTroopBean { get; set; }
        internal CorpTroopBean OpponentCorpTroopBean { get; set; }
        internal List<String> logTextList { get; set; }

    }



}
