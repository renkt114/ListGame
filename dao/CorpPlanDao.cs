using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace ListSLG.model
{
    internal class CorpPlanDao
    {

        // 1件取得
        public static CorpPlan getCorpPlanByCorp(int corpId)
        {

            using (var db = new GameDataContext())
            {

                return db.CorpPlan.Where(x => x.corpId == corpId).FirstOrDefault();

            }

        }

        // 全計画取得
        public static List<CorpPlan> getAllCorpPlan()
        {

            using (var db = new GameDataContext())
            {

                return db.CorpPlan.ToList();

            }

        }

        // 全計画取得（mission付き）
        public static List<CorpPlanMissoinDTO> getAllCorpPlanMissoin()
        {

            using (var db = new GameDataContext())
            {

                var query = from corpPlan in db.CorpPlan
                            join mission in db.Mission
                            on corpPlan.missionId equals mission.missionId
                            select new CorpPlanMissoinDTO
                            {
                                corpPlan = corpPlan,
                                mission = mission
                            };

                return query.ToList();

            }

        }

        // 1件取得（mission付き）
        public static CorpPlanMissoinDTO getCorpPlanMissoinByCorp(Corp corp)
        {

            using (var db = new GameDataContext())
            {

                var query = from corpPlan in db.CorpPlan
                            join mission in db.Mission
                            on corpPlan.missionId equals mission.missionId
                            where corpPlan.corpId == corp.corpId
                            select new CorpPlanMissoinDTO
                            {
                                corpPlan = corpPlan,
                                mission = mission
                            };

                return query.First();

            }

        }



        // 新規作成（最後尾ID追加）
        public static CorpPlan createCorpPlan(int corpId,int missionId)
        {

            using (var db = new GameDataContext())
            {


                CorpPlan newCorpPlan = new CorpPlan
                {
                    corpId = corpId,
                    missionId = missionId

                };

                // 作成
                db.CorpPlan.Add(newCorpPlan);
                db.SaveChanges();

                return newCorpPlan;

            }

        }


        // 全計画削除
        public static void deleteAllCorpPlan()
        {

            using (var db = new GameDataContext())
            {

                db.RemoveRange(getAllCorpPlan());
                db.SaveChanges();

            }

        }

        public class CorpPlanMissoinDTO
        {
            public CorpPlan corpPlan { get; set; }
            public Mission mission { get; set; }
        }
    }
}
