using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ListSLG.model
{
    internal class CorpDao
    {
        // 1件の軍団取得（troop込み）
        public static Corp getCorpData(int corpId)
        {

            using (var db = new GameDataContext())
            {
                Corp corp = db.Corp.Include(a => a.troop).ThenInclude(troop => troop.general).Where(x => x.corpId == corpId).First();
                return corp;

            }
        }

        // 自軍の全軍団取得
        public static List<Corp> getAllAllyCorpData()
        {

            using (var db = new GameDataContext())
            {
                var corpList = db.Corp.Include(a => a.troop).ThenInclude(troop => troop.general).Where(x => x.corpId <= 9).ToList();
                return corpList;

            }

        }
        // 自軍の全軍団取得(無所属除く)
        public static List<Corp> getAllAllyCorpDataNotZero()
        {

            using (var db = new GameDataContext())
            {
                var corpList = db.Corp.Include(a => a.troop).ThenInclude(troop => troop.general).Where(x => x.corpId <= 9).Where(x => x.corpId >= 1).ToList();
                return corpList;

            }

        }

        // 軍団新規作成（最後尾ID追加）
        public static Corp createCorp(string corpName)
        {

            using (var db = new GameDataContext())
            {
                // 自軍ID最大値取得
                // corpIdの9以下で空いている最小の数値を取得

                int nextId = 1;
                for (int i = 1; i <= 9; i++)
                {
                    if (db.Corp.Where(x => x.corpId == i).Count() == 0)
                    {
                        nextId = i;
                        break;
                    }
                }

                // 作成
                db.Corp.Add(new Corp
                {
                    corpId = nextId,
                    corpName = corpName
                });

                db.SaveChanges();

                return db.Corp.Where(x => x.corpId == nextId).First();

            }

        }

        // 軍団数
        public static int getCorpNum()
        {

            using (var db = new GameDataContext())
            {

                int corpNum = db.Corp.Where(x => (9 >= x.corpId)).Count();

                return corpNum;

            }

        }

        public static List<CorpSumNumDataDTO> getAllAllyCorpSumNumData()
        {

            List<CorpSumNumDataDTO> corpSumNumDataDTOList = new List<CorpSumNumDataDTO>();

            using (var db = new GameDataContext())
            {

                var corpList = db.Corp
                    .Where(x => (1 <= x.corpId) && (9 >= x.corpId))
                    .Include(p => p.troop) 
                    .ToList();

                
                foreach (var corpData in corpList)
                {

                    CorpSumNumDataDTO corpSumNumDataDTO = new CorpSumNumDataDTO();
                    corpSumNumDataDTO.corpId = corpData.corpId;
                    corpSumNumDataDTO.corpName = corpData.corpName;
                    corpSumNumDataDTO.sumGeneralNum = 0;
                    corpSumNumDataDTO.sumMaxSoldierNum = 0;

                    foreach (var troopData in corpData.troop)
                    {
                        corpSumNumDataDTO.sumGeneralNum++;
                        corpSumNumDataDTO.sumMaxSoldierNum += troopData.maxSoldierNum;


                    }

                    corpSumNumDataDTOList.Add(corpSumNumDataDTO);


                }
            }

            return corpSumNumDataDTOList;

        }

        // getCorpDataの結果からtroop.potisioinIdが1以上の件数を取得（バリデーション用）
        public static int getPositiondCropTroopNum(int corpId)
        {

            using (var db = new GameDataContext())
            {
                Corp corp = db.Corp.Include(a => a.troop).ThenInclude(troop => troop.general).Where(x => x.corpId == corpId).First();
                int corpNum = corp.troop.Where(x => x.potisioinId >= 1).Count();
                return corpNum;

            }

        }

        // Corpに属するTroopの数を取得
        public static int getCorpTroopNum(int corpId)
        {

            using (var db = new GameDataContext())
            {
                Corp corp = db.Corp.Include(a => a.troop).ThenInclude(troop => troop.general).Where(x => x.corpId == corpId).First();
                int corpNum = corp.troop.Count();
                return corpNum;

            }

        }

        // 軍団削除
        public static void deleteCorp(Corp corp)
        {

            using (var db = new GameDataContext())
            {
                db.Corp.Remove(corp);
                db.SaveChanges();

            }

        }



        public class CorpSumNumDataDTO
        {
            public int corpId { get; set; }
            public String corpName { get; set; }
            public int sumGeneralNum { get; set; }
            public int sumMaxSoldierNum { get; set; }
        }
    }
}
