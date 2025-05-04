using ListSLG.dao;
using ListSLG.logic.generate;
using ListSLG.logic.growth;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListSLG.model.SeedingJoinDao;

namespace ListSLG.logic.turn
{
    class TurnEndLogic
    {

        public static void turnEnd()
        {

            //節変更時の処理
            GameMaster gameMaster = GameMasterDao.addTurn();

            // 味方将軍毎の処理
            GeneralDao.getAllyGeneralAllDataGrid().ForEach(x =>
            {
                // 調子変化
                TurnConditionLogic.turnGeneralConditionChange(x.general);
                // 負傷状態変化
                TurnInjuredLogic.turnGeneralInjuredChange(x.general);

            });

            //期変更時の処理
            if (gameMaster.sectionNum == 1)
            {
                // アドバイス削除
                AdviceDao.deleteAllAdvice();

                termChangeProcess();
            }

            //時代変更時の処理
            if (gameMaster.termNum == 1)
            {
                eraChangeProcess();
            }

            //期末時の処理
            if (gameMaster.sectionNum == 5)
            {
                // アドバイス削除
                AdviceDao.deleteAllAdvice();
                //アドバイス追加
                AdviseGenerateLogic.termAdvice();
            }


        }

        //期変更時の処理
        public static void termChangeProcess() 
        {



            // 昇進処理
            RenewalGeneralDao.getPromotionGeneralList().ForEach(general =>
            {

                Troop troop = TroopDao.getTroopByTroopId(general.troopId);

                general.rank++;
                troop.soldierNum += 500;
                troop.maxSoldierNum += 500;
                troop.general = general;

                TroopDao.updateTroop(troop);


            });


            // 成長および加齢
            GeneralDao.getAllyGeneralAllDataGrid().ForEach(x =>
            {
                GrowthGeneral.turnGrowthGeneral(x.general);
            });


            //出産処理 子供が加齢成長しないようにするのは面倒なのでとりあえず成長および加齢の後に行う
            BirthGeneralLogic birthGeneralLogic = new BirthGeneralLogic();
            List<GenerealPair> generealPair = SeedingJoinDao.getGenerealPairList();

            foreach (var pair in generealPair)
            {
                // 将軍生成
                General newGeneral = birthGeneralLogic.BirthGeneral(pair.breedGeneral, pair.partnerGeneral);

                // 母親のTroopを元に新しいTroopを作成
                Troop breedGeneralTroop = TroopDao.getTroopByTroopId(pair.breedGeneral.troopId);
                Troop newTroop = new Troop()
                {
                    corpId = breedGeneralTroop.corpId,
                    potisioinId = 0,
                    soldierNum = 1000,
                    maxSoldierNum = 1000,
                    troopTypeId = 0,
                    // 生成将軍をセット
                    general = newGeneral
                };

                // 登録
                TroopDao.createTroop(newTroop);

                // Pedigree登録
                PedigreeDao.createPedigree(newGeneral.generalId, pair.breedGeneral.generalId, pair.partnerGeneral.generalId);
            }

            //終わったら全削除
            SeedingJoinDao.deleteAllSeedingJoin();

            // 解任処理
            RenewalGeneralDao.getRetireGeneralList().ForEach(general =>
            {
                // 部隊の削除にとどめる（将軍は何かに使うかもしれないので）
                Troop troop = TroopDao.getTroopByTroopId(general.troopId);
                troop.corpId = 11;
                TroopDao.updateTroop(troop);
                general.years = 10;
                GeneralDao.updateGeneral(general);

                // アドバイス追加
                AdviseGenerateLogic.retireAdvice(general);

            });

            // 引退処理
            GeneralDao.getAllyGeneralAllDataGrid().Where(x => x.general.years >= 10).ToList().ForEach(x =>
            {
                Troop troop = TroopDao.getTroopByTroopId(x.general.troopId);
                TroopDao.deleteTroop(troop);

                // アドバイス追加
                AdviseGenerateLogic.retireAdvice(x.general);

            });

            // CorpDao.getCorpTroopNumの結果が0のCorpを削除
            CorpDao.getAllAllyCorpDataNotZero().ForEach(corp =>
            {
                if (CorpDao.getCorpTroopNum(corp.corpId) == 0)
                {
                    CorpDao.deleteCorp(corp);
                }
            });


            //終わったら全削除
            RenewalGeneralDao.deleteAllRenewalGeneral();

            // 威信0
            GameMasterDao.delPrestige();


        }

        //時代変更時の処理
        public static void eraChangeProcess()
        {



        }




    }
}
