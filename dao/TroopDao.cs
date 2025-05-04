using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Collections;
using System.Dynamic;
using System;

namespace ListSLG.model
{
    internal class TroopDao
    {

        // generalIdからtroopを取得
        public static Troop getTroopByGeneralId(int generalId)
        {
            using (var db = new GameDataContext())
            {
    
                    var general = db.General.Where(x => x.generalId == generalId).First();
                    var troop = db.Troop.Where(x => x.troopId == general.troopId).First();
    
                    return troop;
    
            }
        }

        // troopIdからtroopを取得
        public static Troop getTroopByTroopId(int troopId)
        {
            using (var db = new GameDataContext())
            {
    
                    var troop = db.Troop.Where(x => x.troopId == troopId).First();
    
                    return troop;
    
            }
        }



        public static Troop updateCorpId(int troopId,int corpId)
        {

            using (var db = new GameDataContext())
            {
                var troop = db.Troop.Where(x => x.troopId == troopId).First();
                troop.corpId = corpId;
                db.SaveChanges();

                return troop;

            }

        }

        public static Troop updatePotisionId(int troopId, int postisionId)
        {

            using (var db = new GameDataContext())
            {
                var troop = db.Troop.Where(x => x.troopId == troopId).First();
                troop.potisioinId = postisionId;
                db.SaveChanges();

                return troop;

            }

        }

        public static Troop updateOrderNum(int troopId, int orderNum)
        {

            using (var db = new GameDataContext())
            {
                var troop = db.Troop.Where(x => x.troopId == troopId).First();
                troop.orderNum = orderNum;
                db.SaveChanges();

                return troop;

            }

        }


        public static Troop createTroop(Troop troop)
        {

            using (var db = new GameDataContext())
            {

                // 作成
                db.Troop.Add(troop);

                db.SaveChanges();

                return troop;

            }

        }

        // troopのみの削除。将軍解雇など
        public static void deleteTroop(Troop troop)
        {

            using (var db = new GameDataContext())
            {

                // 作成
                db.Troop.Remove(troop);

                db.SaveChanges();

            }

        }

        // troop更新。結合する将軍情報も同時に更新したりする
        public static void updateTroop(Troop troop)
        {

            using (var db = new GameDataContext())
            {

                db.Update(troop);
                db.SaveChanges();

            }

        }

        // 総将軍数
        public static int getAllSumTroopNum()
        {

            using (var db = new GameDataContext())
            {

                int troopNum = db.Troop.Where(x => (9 >= x.corpId)).Count();

                return troopNum;

            }

        }

        // 総兵数数
        public static int getAllSumTroopMaxSoldierNum()
        {

            using (var db = new GameDataContext())
            {

                int sumTroopMaxSoldierNum = db.Troop.Where(x => (9 >= x.corpId)).Sum(x => (x.maxSoldierNum));

                return sumTroopMaxSoldierNum;

            }

        }


    }

}
