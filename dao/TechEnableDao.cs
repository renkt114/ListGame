using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace ListSLG.model
{
    internal class TechEnableDao
    {
        // 取得
        public static List<TechEnable> geTechEnableList()
        {
            using (var db = new GameDataContext())
            {
                return db.TecEnable.ToList();
            }

        }

        // 更新
        public static void updateTechEnable(int techId,bool enable)
        {
            using (var db = new GameDataContext())
            {
                var target = db.TecEnable.Where(x => x.techId == techId).FirstOrDefault();
                if (target != null)
                {
                    target.Enable = enable;
                    db.SaveChanges();
                }
            }
        }
    }
}
