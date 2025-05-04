using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ListSLG.model
{
    internal class RenewalGeneralDao
    {

        // promotionFlgが1のGeneralのリストを取得
        public static List<General> getPromotionGeneralList()
        {
            using (var db = new GameDataContext())
            {
                    var promotionGeneralList = db.RenewalGeneral.Where(x => x.promotionFlg == 1)
                    .Join(db.General, renewalGeneral => renewalGeneral.generalId, general => general.generalId, (renewalGeneral, general) => general).ToList();
                    return promotionGeneralList;
    
                }

        }

        // retireFlgが1のGeneralのリストを取得
        public static List<General> getRetireGeneralList()
        {
            using (var db = new GameDataContext())
            {
                var promotionGeneralList = db.RenewalGeneral.Where(x => x.retireFlg == 1)
                .Join(db.General, renewalGeneral => renewalGeneral.generalId, general => general.generalId, (renewalGeneral, general) => general).ToList();
                return promotionGeneralList;

            }

        }


        // RenewalGeneralで登録
        public static RenewalGeneral createRenewalGeneral(RenewalGeneral renewalGeneral)
        {

            using (var db = new GameDataContext())
            {
                // 作成
                db.RenewalGeneral.Add(renewalGeneral);

                db.SaveChanges();

            }

            return renewalGeneral;

        }

        //  RenewalGeneralで更新
        public static RenewalGeneral updateGRenewalGeneral(RenewalGeneral renewalGeneral)
        {

            using (var db = new GameDataContext())
            {
                db.Update(renewalGeneral);
                db.SaveChanges();

                return renewalGeneral;

            }

        }


        //  RenewalGeneralがあればpromotionFlgが1なら0に、0なら1に更新。なければpromotionFlg1で作成。
        public static RenewalGeneral updateRpromotionFlg(General promotionGeneral)
        {

            using (var db = new GameDataContext())
            {
                var renewalGeneral = db.RenewalGeneral.Where(x => x.generalId == promotionGeneral.generalId).FirstOrDefault();

                if (renewalGeneral == null)
                {

                    RenewalGeneral createdRenewalGeneral = new RenewalGeneral
                    {
                        generalId = promotionGeneral.generalId,
                        promotionFlg = 1
                    };

                    // 作成
                    createRenewalGeneral(createdRenewalGeneral);

                    return createdRenewalGeneral;

                }
                else
                {
                    if (renewalGeneral.promotionFlg == 1)
                    {
                        renewalGeneral.promotionFlg = 0;
                    }
                    else
                    {
                        renewalGeneral.promotionFlg = 1;
                    }

                    updateGRenewalGeneral(renewalGeneral);

                }

                return renewalGeneral;


            }

        }

        //  RenewalGeneralがあればretireFlgが1なら0に、0なら1に更新。なければretireFlg1で作成。
        public static RenewalGeneral updateRretireFlg(General retireGeneral)
        {

            using (var db = new GameDataContext())
            {
                var renewalGeneral = db.RenewalGeneral.Where(x => x.generalId == retireGeneral.generalId).FirstOrDefault();

                if (renewalGeneral == null)
                {

                    RenewalGeneral createdRenewalGeneral = new RenewalGeneral
                    {
                        generalId = retireGeneral.generalId,
                        retireFlg = 1,
                        promotionFlg = 0

                    };

                    // 作成
                    createRenewalGeneral(createdRenewalGeneral);

                    // 婚姻をリセット
                    SeedingJoinDao.deleteSeedingJoinById(retireGeneral);

                    return createdRenewalGeneral;

                }
                else
                {
                    if (renewalGeneral.retireFlg == 1)
                    {
                        renewalGeneral.retireFlg = 0;
                    }
                    else
                    {
                        renewalGeneral.retireFlg = 1;
                        // 昇進、婚姻をリセット
                        renewalGeneral.promotionFlg = 0;
                        SeedingJoinDao.deleteSeedingJoinById(retireGeneral);
                    }

                    updateGRenewalGeneral(renewalGeneral);

                }

                return renewalGeneral;
            }
        }

        // 全レコード削除
        public static void deleteAllRenewalGeneral()
        {

            using (var db = new GameDataContext())
            {
                db.RenewalGeneral.RemoveRange(db.RenewalGeneral);
                db.SaveChanges();

            }

        }





    }
}
