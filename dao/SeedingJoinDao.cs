using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ListSLG.model
{
    internal class SeedingJoinDao
    {

        // 全件取得
        public static List<SeedingJoin> getSeedingJoinList()
        {
            using (var db = new GameDataContext())
            {
                return db.SeedingJoin.ToList();
            }
        }

        // IDで登録追加
        public static void createSeedingJoinById(General breedGeneral, General partnerGeneral)
        {

            using (var db = new GameDataContext())
            {
                // 作成
                db.SeedingJoin.Add(new SeedingJoin
                {
                    breedGeneralId = breedGeneral.generalId,
                    partnerGeneralId = partnerGeneral.generalId
                }); ;

                db.SaveChanges();

            }

        }

        // SeedingJoinで登録
        public static SeedingJoin createSeedingJoin(SeedingJoin seedingJoin)
        {

            using (var db = new GameDataContext())
            {
                // 作成
                db.SeedingJoin.Add(seedingJoin);

                db.SaveChanges();

            }

            return seedingJoin;

        }

        // breedGeneralId、partnerGeneralIdのそれぞれのGeneralをGenerealPairで取得
        public static List<GenerealPair> getGenerealPairList()
        {
            
            using (var db = new GameDataContext())
            {
                var seedingJoinList = db.SeedingJoin.ToList();
                List<GenerealPair> generealPairList = new List<GenerealPair>();

                foreach (var seedingJoin in seedingJoinList)
                {
                    GenerealPair generealPair = new GenerealPair();
                    generealPair.breedGeneral = db.General.Where(x => x.generalId == seedingJoin.breedGeneralId).First();
                    generealPair.partnerGeneral = db.General.Where(x => x.generalId == seedingJoin.partnerGeneralId).First();
                    generealPairList.Add(generealPair);
                }

                return generealPairList;

            }

        }

        // IDで登録削除（1件）
        public static void deleteSeedingJoinById(General breedGeneral)
        {
            using (var db = new GameDataContext())
            {
                var seedingJoin = db.SeedingJoin.FirstOrDefault(x => x.breedGeneralId == breedGeneral.generalId);
                if (seedingJoin == null)
                {
                    // 対象なしとして処理を終了する
                    return;
                }
                    db.SeedingJoin.Remove(seedingJoin);
                    db.SaveChanges();
    
            }


        }

        // 全SeedingJoin削除
        public static void deleteAllSeedingJoin()
        {

            using (var db = new GameDataContext())
            {
                var seedingJoinList = db.SeedingJoin.ToList();
                db.SeedingJoin.RemoveRange(seedingJoinList);
                db.SaveChanges();

            }

        }



        public class GenerealPair
        {
            public General breedGeneral { get; set; }
            public General partnerGeneral { get; set; }
        }


    }
}
