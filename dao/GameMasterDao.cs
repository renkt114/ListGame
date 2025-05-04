using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace ListSLG.model
{
    internal class GameMasterDao
    {
        // 取得
        public static GameMaster getGameMaster()
        {

            using (var db = new GameDataContext())
            {

                return db.GameMaster.First();

            }
        }


        //sectionNumを加算し、sectionNumが6（期末処理後）になった場合、termNum加算してsectionNumを0に戻す。termNumが5になった場合、eraNum加算してtermNumとsectionNumを0に戻す。
        public static GameMaster addTurn()
        {
            using (var db = new GameDataContext())
            {
                var gameMaster = getGameMaster();
                gameMaster.sectionNum++;
                if (gameMaster.sectionNum == 6)
                {
                    gameMaster.sectionNum = 1;
                    gameMaster.termNum++;
                    if (gameMaster.termNum == 5)
                    {
                        gameMaster.termNum = 1;
                        gameMaster.eraNum++;
                        // eraNumの値がenum型のEraDivの要素数を超えた場合、0に戻す
                        if (gameMaster.eraNum == System.Enum.GetNames(typeof(EraDiv)).Length)
                        {
                            gameMaster.eraNum = 0;
                        }

                    }
                }
                db.GameMaster.Update(gameMaster);
                db.SaveChanges();
                return gameMaster;
            }
        }

        // 威信加算
        public static GameMaster addPrestige(int prestige)
        {
            using (var db = new GameDataContext())
            {
                var gameMaster = getGameMaster();
                gameMaster.prestige += prestige;
                db.GameMaster.Update(gameMaster);
                db.SaveChanges();
                return gameMaster;
            }
        }

        // 威信初期化
        public static GameMaster delPrestige()
        {
            using (var db = new GameDataContext())
            {
                var gameMaster = getGameMaster();
                gameMaster.prestige = 0;
                db.GameMaster.Update(gameMaster);
                db.SaveChanges();
                return gameMaster;
            }
        }




    }
}
