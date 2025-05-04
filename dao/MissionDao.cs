using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace ListSLG.model
{
    internal class MissionDao
    {
        // 全任務取得
        public static List<Mission> getAllMission()
        {

            using (var db = new GameDataContext())
            {

                return  db.Mission.ToList();

            }
        }

        // IDで任務検索
        public static Mission getMissionByMissionId(int missionId)
        {

            using (var db = new GameDataContext())
            {


                return db.Mission.Where(x => x.missionId == missionId).FirstOrDefault();

            }
        }


    }
}
