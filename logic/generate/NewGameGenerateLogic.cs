using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListSLG.dao;
using ListSLG.logic.growth;
using ListSLG.model;
using ListSLG.util;

namespace ListSLG.logic.generate
{
    internal class NewGameGenerateLogic
    {

        // 新規ゲーム開始時の処理
        public static void newGameGenerate()
        {

            // CSVから将軍名リスト取得（ファイル操作の回数は押さえたいため、まずこれだけ変数にいれる）
            List<string> nameList = getNameCsv();
            String corpName = generateName(nameList);

            Corp corp = CorpDao.createCorp(corpName + "家");

            createStartGeneral(corp, corpName, 2,1500, 1, 1, 1001, 10);
            createStartGeneral(corp, generateName(nameList), 1,1000, 2, 2, 1002, 0);
            createStartGeneral(corp, generateName(nameList), 1,1000, 3, 3, 1003 , 0);

        }

        // CSVから将軍名リスト取得
        private static List<string> getNameCsv()
        {

            return ResourceFileUtil.GetGeneralNameByCsvFile();


        }

        // 将軍名リストから名前取得
        //TODO　名前の重複回避
        private static string generateName(List<string> generalNameList)
        {
            Random random = new Random();
            // 0～リスト件数の乱数を取得し、その番号の名前を返す
            return generalNameList[random.Next(0, generalNameList.Count)];

        }


        private static void createStartGeneral(Corp corp, String name, int rank, int solnum, int potisioinId, int orderNum, int id, int buf)
        {

            Troop troop = new Troop();
            Troop newTroop = new Troop()
            {
                corpId = corp.corpId,
                potisioinId = potisioinId,
                soldierNum = solnum,
                maxSoldierNum = solnum,
                troopTypeId = 0,
                orderNum = orderNum
            };

            TroopDao.createTroop(newTroop);


            General g1 = new General();
            g1.generalId = id;
            g1.name = name;

            g1.troopId = newTroop.troopId;
            g1.rank = rank;
            g1.years = 2;
            g1.injured = 0;
            g1.condition = 3;
            g1.tier = 0;
            g1.abilityType = 0;

            g1.infantryAdapt = 5;
            g1.cavalryAdapt = 5;
            g1.rangedAdapt = 5;

            g1.attackGrowth = 5;
            g1.defenseGrowth = 5;
            g1.maneuverGrowth = 5;

            int atk = buf;
            int def = buf;
            int man = buf;
            for (int i = 0; i < 10; i++)
            {
                atk += GrowthGeneral.grouthCalc(g1.attackGrowth);
                def += GrowthGeneral.grouthCalc(g1.defenseGrowth);
                man += GrowthGeneral.grouthCalc(g1.maneuverGrowth);
            }

            g1.attackAbility = atk;
            g1.defenseAbility = def;
            g1.maneuverAbility = man;


            GeneralDao.createGeneral(g1);
        }
    }
}
