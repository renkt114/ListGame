using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.battle.calc
{
    //一般的な攻撃計算を行うクラス。
    internal static class NormalBattleCalc

    {
        // 一般的な攻撃計算
        public static int damegeCalc(Troop onTroop, Troop toTroop, double posDamegeRatio)
        {
            //　兵数による加算 兵数/4000+1
            double solNumDamegeRatio = (double)onTroop.soldierNum / 5000 + 1;

            // 適正による加算 1.0 + (適正値 * 0.05)　最大で1.5
            double adaptRatio = 1.0;
            switch (onTroop.potisioinId)
            {
                case 1:
                case 2:
                case 3:
                    adaptRatio = 1.00 + (0.05 * (double)onTroop.general.infantryAdapt);
                    break;
                case 4:
                case 5:
                    adaptRatio = 1.00 + (0.05 * (double)onTroop.general.cavalryAdapt);
                    break;
                case 6:
                case 7:
                    adaptRatio = 1.00 + (0.05 * (double)onTroop.general.rangedAdapt);
                    break;
                default:
                    adaptRatio = 1.00;
                    break;
            }


            // 乱数による加算
            Random random = new System.Random();
            int atkDice = random.Next(80, 120);

            double DiceDamegeRatio = 1.0;

            if (atkDice <= 85)
            {
                // ファンブル
                DiceDamegeRatio = 0.5;
                Debug.Print("fumble");

            }
            else if (atkDice >= 115)
            {
                // クリティカル
                DiceDamegeRatio = 1.5;
                Debug.Print("critical");
            } else
            {
                // 通常（0.8 ～ 1.2）
                DiceDamegeRatio = (double)atkDice / 100;
            }

            // 相手方の防御係数
            int defDice = random.Next(50, 150);
            double DiceDefenceRatio = (double)defDice / 100;


            // ダメージ計算
            double damege = Math.Round(

                // 攻撃能力 * ポジションダメージ係数 * 兵数ダメージ係数 * 適正ダメージ係数 * 乱数ダメージ係数 * 2
                (((double)onTroop.general.attackAbility / 1.5 + 50) * posDamegeRatio * solNumDamegeRatio * adaptRatio * DiceDamegeRatio * 3) 
                // 上記から敵防御能力 * 乱数防御係数を引く
                    - (((double)toTroop.general.defenseAbility / 1.5 + 50) * DiceDefenceRatio)

                );

            // 防御側が上回った場合はダメージ0
            if (damege < 0)
            {
                damege = 0;
            }


            Debug.Print("posDamegeRatio:" + posDamegeRatio + " solNumDamegeRatio:" + solNumDamegeRatio + " adaptRatio:" + adaptRatio + " DiceDamegeRatio:" + DiceDamegeRatio + " attack:" + ((double)onTroop.general.attackAbility * posDamegeRatio * solNumDamegeRatio * DiceDamegeRatio * 2) + " DiceDefenceRatio: " + DiceDefenceRatio + "  total:" + damege);

            return (int)damege;


        }
    }
}
