using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace ListSLG.model
{
    // SaveDataテーブル操作DAO
    // SaveDataはユーザーデータとなるので、UserDataContextを使用する！
    internal class SaveDataDao
    {
        // 全件取得
        public static List<SaveData> getAllSaveData()
        {

            using (var db = new UserDataContext())
            {

                return db.SaveData.ToList(); ;

            }
        }

        // ID999以外取得
        public static List<SaveData> getAllSaveDataNot999()
        {

            using (var db = new UserDataContext())
            {

                return db.SaveData.Where(x => x.saveDataId != 999).ToList(); ;

            }
        }


        // 1件取得
        public static SaveData getSaveData(int saveDataId)
        {

            using (var db = new UserDataContext())
            {

                return db.SaveData.Where(x => x.saveDataId == saveDataId).First();

            }
        }

        // 更新(セーブ)
        public static SaveData updateSaveData(SaveData saveData)
        {

            using (var db = new UserDataContext())
            {
                db.Update(saveData);
                db.SaveChanges();

                return saveData;

            }

        }




    }
}
