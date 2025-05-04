using ListSLG.logic.generate;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ListSLG.logic.growth
{

    // 成長ロジック
    public static class GrowthGeneral
    {

        public static General turnGrowthGeneral(General general , bool debug = false)
        {

            // 成長処理
            int attack = getGrowthPoint(general.attackGrowth, general.years);
            general.attackAbility += attack;
            general.attackFromNow = attack;

            int defense = getGrowthPoint(general.defenseGrowth, general.years);
            general.defenseAbility += defense;    
            general.defenseFromNow = defense;

            int maneuver = getGrowthPoint(general.maneuverGrowth, general.years);
            general.maneuverAbility += maneuver;
            general.maneuverFromNow = maneuver;


            // 以下は今のところ使っていないので省略
            //general.commandAbility += getGrowthPoint(general.commandGrowth, general.years);
            //general.managementAbility += getGrowthPoint(general.managementGrowth, general.years);
            //general.strategyAbility += getGrowthPoint(general.strategyGrowth, general.years);
            //general.reconAbility += getGrowthPoint(general.reconGrowth, general.years);
            //general.physicalAbility += getGrowthPoint(general.physicalGrowth, general.years);

            // 年数加算
            general.years++;

            // 2年目以降はランク1になる
            if (general.years == 2)
            {
                general.rank = 1;
                // アドバイス追加
                AdviseGenerateLogic.growthAdvice(general);
            }

            // デバッグモードでなければ、DBに保存
            if (!debug) {
                GeneralDao.updateGeneral(general);
            }

            return general;

        }


        private static int getGrowthPoint(int growthParam, int years)
        {

            // 能力値平均9以上が指定されたら9にする
            if (growthParam > 9)
            {
                growthParam = 9;
            }


            if (years == 0)
            {
                // 1年目と２年目で７割成長
                return grouthCalc(growthParam) * 4;

            }
            if (years == 1)
            {

                return grouthCalc(growthParam) * 5;

            }
            else if(years <= 7) 
            {
                // 2年目以降7年目までで残り3割成長
                // 0.5*6=3の計算
                return grouthCalc(growthParam) / 2;

            }else
            {
                // 目下、8年目以降は成長しない
                return 0;
            }


        }

        // 成長計算
        public static int grouthCalc(int growthParam)
        {

            Random random = new Random();
            int[] paramArry = new int[9];

            // 1～9の乱数を5個作る
            for (int i = 0; i < 9; i++)
            {
                paramArry[i] = random.Next(1, 10);
            }

            // 昇順（低い順）に並べる
            paramArry = paramArry.OrderBy(x => x).ToArray();

            // 下からx +1 番目（引数の0～9）を返す。配列が0からなので -1 する。
            return paramArry[growthParam - 1];

        }



        public static void testMain()
        {
            turnGrowthGeneral(GeneralDao.getGeneralByGeneralId(1004));

        }



    }
}
