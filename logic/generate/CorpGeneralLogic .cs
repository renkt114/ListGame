using ListSLG.model;
using ListSLG.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.generate
{
    class CorpGeneralLogic
    {


        Random random;
        int corpId;
        int level;
        string name;
        int generalNum;



        public CorpGeneralLogic(int generalNum,int level, string name = "敵軍団")
        {
            random = new System.Random();
            corpId = 11;
            this.level = level;
            this.name = name;
            this.generalNum = generalNum;
        }


        // 敵corp生成
        public Corp EnemyCorpCreate()
        {

            Random random = new System.Random();

            Corp corp = new Corp();
            corp.corpId = corpId;
            corp.corpName = name;

            //generalNumによってtroopListを変更
            List<Troop> troopList = new List<Troop>();
            if (generalNum == 3)
            {
                troopList = create3TroopList();
            }
            else if (generalNum == 5)
            {
                troopList = create5TroopList();
            }
            else if (generalNum == 7)
            {
                troopList = create7TroopList();
            }

            corp.troop = troopList;

            return corp;

        }


        private List<Troop> create3TroopList()
        {
            List<General> tierGeneralList = getTierGeneralList(this.level);
            List<General> upperTierGeneralList = getTierGeneralList(this.level + 1 );


            List<Troop> troopList = new List<Troop>();

            troopList.Add(createTroop(1, upperTierGeneralList[0], 1, 1, (this.level + 2 ) * 500));
            troopList.Add(createTroop(2, tierGeneralList[0], 2, 2, (this.level + 1) * 500));
            troopList.Add(createTroop(3, tierGeneralList[1], 3, 3, (this.level + 1) * 500));

            return troopList;

        }

        private List<Troop> create5TroopList()
        {
            List<General> tierGeneralList = getTierGeneralList(this.level);
            List<General> upperTierGeneralList = getTierGeneralList(this.level + 1);


            List<Troop> troopList = new List<Troop>();

            troopList.Add(createTroop(1, upperTierGeneralList[0], 1, 1, (this.level + 2) * 500));
            troopList.Add(createTroop(2, upperTierGeneralList[1], 2, 2, (this.level + 1) * 500));
            troopList.Add(createTroop(3, upperTierGeneralList[2], 3, 3, (this.level + 1) * 500));
            troopList.Add(createTroop(4, tierGeneralList[1], 4, 4, (this.level + 1) * 500));
            troopList.Add(createTroop(5, tierGeneralList[2], 5, 5, (this.level + 1) * 500));

            return troopList;

        }

        private List<Troop> create7TroopList()
        {
            List<General> tierGeneralList = getTierGeneralList(this.level);
            List<General> upperTierGeneralList = getTierGeneralList(this.level + 1);

            List<Troop> troopList = new List<Troop>();

            troopList.Add(createTroop(1, upperTierGeneralList[0], 1, 1, (this.level + 2) * 500));
            troopList.Add(createTroop(2, upperTierGeneralList[1], 2, 2, (this.level + 2) * 500));
            troopList.Add(createTroop(3, upperTierGeneralList[2], 3, 3, (this.level + 2) * 500));
            troopList.Add(createTroop(4, upperTierGeneralList[3], 4, 4, (this.level + 1) * 500));
            troopList.Add(createTroop(5, upperTierGeneralList[4], 5, 5, (this.level + 1) * 500));
            troopList.Add(createTroop(6, tierGeneralList[1], 6, 6, (this.level + 1) * 500));
            troopList.Add(createTroop(7, tierGeneralList[2], 7, 7, (this.level + 1) * 500));

            return troopList;

        }



        private List<General> getTierGeneralList(int tier)
        {

            List<General> tier1GeneralList = GeneralDao.getGeneralByEraTier(tier);

            //tier1GeneralListをランダムに7個選択、重複無し
            List<General> selectedGeneralList = new List<General>();
            for (int i = 0; i < 7; i++)
            {
                int index = random.Next(tier1GeneralList.Count - 1);
                selectedGeneralList.Add(tier1GeneralList[index]);
                tier1GeneralList.RemoveAt(index);
            }

            return selectedGeneralList;

        }



        private Troop createTroop(int troopId, General general, int potisioinId, int orderNum, int soldierNum)
        {

            Troop troop = new Troop();
            troop.troopId = troopId;
            troop.corpId = corpId;
            troop.potisioinId = potisioinId;
            troop.orderNum = orderNum;
            troop.general = general;
            troop.soldierNum = soldierNum;
            troop.maxSoldierNum = soldierNum;

            return troop;

        }


    }
}
