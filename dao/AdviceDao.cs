using ListSLG.devtools;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.dao
{
    internal class AdviceDao
    {

        // 全アドバイスをpriorityが高い順に取得
        public static List<Advice> getAllAdviceSortedPriority()
        {

            using (var db = new GameDataContext())
            {

                var adviceList = db.Advice.OrderByDescending(x => x.priority).ToList();
                return adviceList;

            }

        }

        // 全アドバイス削除
        public static void deleteAllAdvice()
        {

            using (var db = new GameDataContext())
            {

                db.Advice.RemoveRange(db.Advice);
                db.SaveChanges();

            }

        }

        // アドバイス新規作成
        public static void createAdvice(int priority, string adviseText)
        {

            using (var db = new GameDataContext())
            {

                db.Advice.Add(new Advice
                {
                    priority = priority,
                    adviseText = adviseText
                });
                db.SaveChanges();

            }

        }




    }
}
