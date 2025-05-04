using ListSLG.bean;
using ListSLG.model;
using ListSLG.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListSLG.model.SeedingJoinDao;

namespace ListSLG.logic.resource
{
    public static class PrestigeLogic
    {

        // 戦闘終了後の威信増加計算
        public static int battleEndPrestige(int gainedPrestige, CorpTroopBean battledEnemyCorpBean)
        {
        
            if (battledEnemyCorpBean.getTroopByPositionId((int)PotisioinDiv.centorInfantry).soldierNum <= 0)
            {
                // 勝利の場合、全額ゲット
                GameMasterDao.addPrestige(gainedPrestige);

                return gainedPrestige;

            }
            else
            {
                // 敗北時は半額*残兵士/総兵士
                double sumSoldierNum = battledEnemyCorpBean.getSumSoldierNum();
                double sumMaxSoldierNum = battledEnemyCorpBean.getSumMaxSoldierNum();
                double killRatio = 1.0 - (sumSoldierNum/sumMaxSoldierNum);

                int resultPrestage = MathUtil.RoundUpToDigit((int)((gainedPrestige / 2) * killRatio), 2);

                // 技術11が有効の場合は1.5倍
                if (App.techBean.getTechEnableBool(11))
                {
                     resultPrestage = MathUtil.RoundUpToDigit((int)(resultPrestage * 1.5), 2);
                }


                GameMasterDao.addPrestige(resultPrestage);

                return resultPrestage;

            }



        }

        // 当期使用予定の威信コスト計算
        public static int calcCostPrestige()
        {

            int costPrestige = 0;

            // 将軍3人まで無料
            int freeGenCnt = 3;


            // 技術10が有効の場合は5人まで無料
            if (App.techBean.getTechEnableBool(10))
            {
                freeGenCnt = 5;
            }

            // 

            // 全将軍のrank1につき500を加算、ただしgeneralはnull許容
            // 解任予定の将軍も支払うこととする。当期に対して払っているので。
            GeneralDao.getAllyGeneralAllDataGrid().ForEach(generalAllDataGrid =>
            {

                // 無料将軍カウント
                if(freeGenCnt > 0)
                {
                    costPrestige += -500;
                    freeGenCnt --;
                }

                // yearsが2以下の場合は無料
                if (generalAllDataGrid.general.years >= 2)
                {
                    costPrestige += (generalAllDataGrid.general.rank ?? 0) * 500;
                }

                

            });

            // 軍団数につき加算
            switch (CorpDao.getAllAllyCorpDataNotZero().Count)
            {
                case 1:
                    break;
                case 2:
                    costPrestige += 500;
                    break;
                case 3:
                    costPrestige += 1500;
                    break;
                case 4:
                    costPrestige += 3000;
                    break;
                default:
                    break;
            }


            // 出産予定
            SeedingJoinDao.getSeedingJoinList().ForEach(seedingJoin =>
            {
                costPrestige += seedingJoin.costPrestige;

            });

            // 昇進予定
            RenewalGeneralDao.getPromotionGeneralList().ForEach(genereal =>
            {
                // TODO とりあえず一人500固定
                costPrestige += 500;

            });

            // 解任予定
            RenewalGeneralDao.getRetireGeneralList().ForEach(genereal =>
            {
                // ランク1あたり1000コスト減算
                costPrestige -= (genereal.rank ?? 0) * 1000;

            });

            // マイナスの場合は0にする
            if (costPrestige < 0)
            {
                costPrestige = 0;
            }

            return costPrestige;


        }
    }
}
