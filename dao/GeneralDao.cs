using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Collections;
using System.Dynamic;
using System;
using System.Text.RegularExpressions;
using ListSLG.Migrations;

namespace ListSLG.model
{
    internal class GeneralDao
    {

        // generalIDからGeneral取得
        public static General getGeneralByGeneralId(int generalId)
        {

            using (var db = new GameDataContext())
            {
                General general = db.General.Where(x => x.generalId == generalId).First();
                return general;

            }
        }

        // 全将軍取得
        public static List<GeneralAllDataGridDTO> getGeneralAllDataGrid()
        {

            DataGrid dataGrid = new DataGrid();

            using (var db = new GameDataContext())
            {

                var general = db.General;
                var troop = db.Troop;
                var corp = db.Corp;

                var allGeneralAllDataList = corp.Join(
                    troop, x => x.corpId, y => y.corpId,
                                    (cor, tor) => new { corp = cor, troop = tor })

                    .Join(general, xy => xy.troop.troopId, z => z.troopId,
                    (xy, z) => new { xy.corp, xy.troop, general = z })
                    .Select(joined => new GeneralAllDataGridDTO
                    {
                        corp = joined.corp,
                        troop = joined.troop,
                        general = joined.general
                    })
                .ToList();

                return allGeneralAllDataList;


                //return db.General.FromSqlRaw("SELECT * FROM General INNER JOIN Troop on General.troopId = Troop.troopId INNER JOIN Corp on Troop.corpId = Corp.corpId").ToList<dynamic>();


            }

        }

        // 味方全将軍取得
        public static List<GeneralAllDataGridDTO> getAllyGeneralAllDataGrid()
        {

            DataGrid dataGrid = new DataGrid();

            using (var db = new GameDataContext())
            {

                var general = db.General;
                var troop = db.Troop;
                // corpIdが9以下のものを取得
                var corp = db.Corp.Where(x => x.corpId <= 9);

                var allGeneralAllDataList = corp.Join(
                    troop, x => x.corpId, y => y.corpId,
                                    (cor, tor) => new { corp = cor, troop = tor })

                    .Join(general, xy => xy.troop.troopId, z => z.troopId,
                    (xy, z) => new { xy.corp, xy.troop, general = z })
                    .Select(joined => new GeneralAllDataGridDTO
                    {
                        corp = joined.corp,
                        troop = joined.troop,
                        general = joined.general
                    })
                .ToList();

                return allGeneralAllDataList;


                //return db.General.FromSqlRaw("SELECT * FROM General INNER JOIN Troop on General.troopId = Troop.troopId INNER JOIN Corp on Troop.corpId = Corp.corpId").ToList<dynamic>();


            }

        }

        // 引数の将軍リストにpartnerGeneralを追加
        public static List<GeneralsDetailDataGridDTO> getAllGeneralsDetailDataGrid(List<GeneralAllDataGridDTO> generalDataList)
        {

            List<GeneralsDetailDataGridDTO> generalsDetailDataGridList = new List<GeneralsDetailDataGridDTO>();

            using (var db = new GameDataContext())
            {

                // seedJoinとGeneralの結合
                var seedJoinAndPartnerList = db.SeedingJoin
                    .Join(db.General, sj => sj.partnerGeneralId, gen => gen.generalId, (sj, gen) => new { sj, gen })
                    .ToList();

                // renewalGeneralを取得（promotionFlgが1のものを限定していたが、retireFlgも増えたし大した量もないだろうから全量）
                var renewalGeneralList = db.RenewalGeneral
                    .ToList();

                // EFCoreもliteSQLも外部結合に弱いため、手動で結合
                foreach (var generalData in generalDataList)
                {
                    GeneralsDetailDataGridDTO generalsDetailData = new GeneralsDetailDataGridDTO();

                    generalsDetailData.general = generalData.general;
                    generalsDetailData.corp = generalData.corp;
                    generalsDetailData.troop = generalData.troop;
                    generalsDetailData.partnerGeneral = seedJoinAndPartnerList.FirstOrDefault(x => x.sj.breedGeneralId == generalData.general.generalId)?.gen;
                    generalsDetailData.renewalGeneral = renewalGeneralList.FirstOrDefault(x => x.generalId == generalData.general.generalId);
                    if (generalsDetailData.renewalGeneral != null && generalsDetailData.renewalGeneral.promotionFlg == 1)
                    {
                        generalsDetailData.promotionFlg = "昇進予定";
                    }
                    else
                    {
                        generalsDetailData.promotionFlg = "";
                    }

                    if (generalsDetailData.renewalGeneral != null && generalsDetailData.renewalGeneral.retireFlg == 1)
                    {
                        generalsDetailData.retireFlg = "解任予定";
                    }
                    else
                    {
                        generalsDetailData.retireFlg = "";
                    }



                    generalsDetailDataGridList.Add(generalsDetailData);


                }


            }

            return generalsDetailDataGridList;

        }

        //特定のcorpの全将軍取得
        public static List<GeneralAllDataGridDTO> getGeneralDataGridByCorpId(int corpId)
        {

            DataGrid dataGrid = new DataGrid();

            using (var db = new GameDataContext())
            {



                var general = db.General;
                var troop = db.Troop;
                var corp = db.Corp;

                var allGeneralAllDataList = corp.Join(
                    troop, x => x.corpId, y => y.corpId,
                                    (cor, tor) => new { corp = cor, troop = tor })

                    .Join(general, xy => xy.troop.troopId, z => z.troopId,
                    (xy, z) => new { xy.corp, xy.troop, general = z })
                    .Where(z => z.corp.corpId == corpId)
                    .Select(joined => new GeneralAllDataGridDTO
                    {
                        corp = joined.corp,
                        troop = joined.troop,
                        general = joined.general
                    })
                .ToList();

                return allGeneralAllDataList;


                //return db.General.FromSqlRaw("SELECT * FROM General INNER JOIN Troop on General.troopId = Troop.troopId INNER JOIN Corp on Troop.corpId = Corp.corpId").ToList<dynamic>();


            }

        }

        // 現在のera、指定したtierの敵将軍を取得
        public static List<General> getGeneralByEraTier(int tier)
        {

            using (var db = new GameDataContext())
            {

                var eraNum = db.GameMaster.FirstOrDefault().eraNum;

                var generalList = db.General
                    .Where(x => x.entryEra == eraNum && x.tier == tier && x.generalId <= 999)
                    .ToList();

                return generalList;

            }

        }

        // 自動生成した将軍の雇用による追加
        public static General createGeneral(General general)
        {

            using (var db = new GameDataContext())
            {

                // 作成
                db.General.Add(general);

                db.SaveChanges();

                return general;

            }

        }

        // 更新、主に成長時
        public static General updateGeneral(General general)
        {

            using (var db = new GameDataContext())
            {
                db.Update(general);
                db.SaveChanges();

                return general;

            }

        }


        // 種付け相手の将軍取得
        public static List<General> getSeedingPartnerGeneral()
        {

            using (var db = new GameDataContext())
            {

                var eraNum = db.GameMaster.FirstOrDefault().eraNum;

                // 時代かつtierが2以上のもの、技術が有効の場合自軍も含める
                var seedingPartnerGeneralList = db.General
                    .Where(x => (x.entryEra == eraNum && x.tier >= 2) || (x.generalId >= 1000 && App.techBean.getTechEnableBool(6)))
                    .ToList();

                return seedingPartnerGeneralList;



            }

        }

    }

    public class GeneralAllDataGridDTO
    {
        public Corp corp { get; set; }
        public Troop troop { get; set; }
        public General general { get; set; }
    }

    // 将軍一覧グリッド用データクラス
    public class GeneralsDetailDataGridDTO
    {
        public General general { get; set; }

        public Troop troop { get; set; }

        public Corp corp { get; set; }

        public General? partnerGeneral { get; set; }

        public RenewalGeneral? renewalGeneral { get; set; }

        public String promotionFlg  { get; set; }

        public String retireFlg { get; set; }
    }

}
