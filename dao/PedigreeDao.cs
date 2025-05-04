using ListSLG.devtools;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ListSLG.dao
{
    internal class PedigreeDao
    {

        // Pedigree登録
        public static void createPedigree(int childId, int breedGeneraId, int partnerGeneralId)
        {

            using (var db = new GameDataContext())
            {

                db.Pedigree.Add(new Pedigree
                {
                    childId = childId,
                    breedGeneraId = breedGeneraId,
                    partnerGeneralId = partnerGeneralId
                });
                db.SaveChanges();

            }

        }


        // 血統図取得用
        public static List<General> getPedigreeChart(int childId)
        {

            using (var db = new GameDataContext())
            {

                List<int?> pedGenIdList = new List<int?>();

                Pedigree ped1 = null;
                Pedigree ped2b = null;
                Pedigree ped2p = null;
                Pedigree ped3bb = null;
                Pedigree ped3bp = null;
                Pedigree ped3pb = null;
                Pedigree ped3pp = null;


                ped1 = db.Pedigree.Where(x => x.childId == childId).FirstOrDefault();


                if(ped1 != null)
                {
                    ped2b = db.Pedigree.Where(x => x.childId == ped1.breedGeneraId).FirstOrDefault();
                    ped2p = db.Pedigree.Where(x => x.childId == ped1.partnerGeneralId).FirstOrDefault();
                }

                if (ped2b != null)
                {
                    ped3bb = db.Pedigree.Where(x => x.childId == ped2b.breedGeneraId).FirstOrDefault();
                    ped3bp = db.Pedigree.Where(x => x.childId == ped2b.partnerGeneralId).FirstOrDefault();

                }

                if (ped2p != null)
                {
                    ped3pb = db.Pedigree.Where(x => x.childId == ped2p.breedGeneraId).FirstOrDefault();
                    ped3pp = db.Pedigree.Where(x => x.childId == ped2p.partnerGeneralId).FirstOrDefault();
                }




                // ped1がnullの場合はnullを追加
                if (ped1 != null)
                {
                    pedGenIdList.Add(ped1.breedGeneraId);
                    pedGenIdList.Add(ped1.partnerGeneralId);

                }else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);

                }
                if (ped2b != null)
                {
                    pedGenIdList.Add(ped2b.breedGeneraId);
                    pedGenIdList.Add(ped2b.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }
                if (ped2p != null)
                {
                    pedGenIdList.Add(ped2p.breedGeneraId);
                    pedGenIdList.Add(ped2p.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }
                if (ped3bb != null)
                {
                    pedGenIdList.Add(ped3bb.breedGeneraId);
                    pedGenIdList.Add(ped3bb.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }
                if (ped3bp != null)
                {
                    pedGenIdList.Add(ped3bp.breedGeneraId);
                    pedGenIdList.Add(ped3bp.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }
                if (ped3pb != null)
                {
                    pedGenIdList.Add(ped3pb.breedGeneraId);
                    pedGenIdList.Add(ped3pb.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }
                if (ped3pp != null)
                {
                    pedGenIdList.Add(ped3pp.breedGeneraId);
                    pedGenIdList.Add(ped3pp.partnerGeneralId);
                } else
                {
                    pedGenIdList.Add(null);
                    pedGenIdList.Add(null);
                }


                // pedListでループしてGeneralを取得
                List<General> generalList = new List<General>();
                foreach (int? generalId in pedGenIdList)
                {
                    if(generalId != null)
                    {
                        General general = db.General.Where(x => x.generalId == generalId).First();
                        generalList.Add(general);
                    } else
                    {
                        generalList.Add(null);
                    }

                }

                return generalList;

                /*


                var sql = @"
                    select 
                    genp1b.name AS 'genp1bname',
                    genp1p.name AS 'genp1pname',
                    genp2bb.name AS 'genp2bbname',
                    genp2bp.name AS 'genp2bpname',
                    genp2pb.name AS 'genp2pbname',
                    genp2pp.name AS 'genp2ppname',
                    genp3bbb.name AS 'genp3bbbname',
                    genp3bbp.name AS 'genp3bbpname',
                    genp3bpb.name AS 'genp3bpbname',
                    genp3bpp.name AS 'genp3bppname',
                    genp3pbb.name AS 'genp3pbbname',
                    genp3pbp.name AS 'genp3pbpname',
                    genp3ppb.name AS 'genp3ppbname',
                    genp3ppp.name AS 'genp3pppname'
                    from Pedigree ped1
                    LEFT OUTER JOIN Pedigree ped2b on ped1.breedGeneraId = ped2b.childId
                    LEFT OUTER JOIN Pedigree ped3bb on ped2b.breedGeneraId = ped3bb.childId
                    LEFT OUTER JOIN Pedigree ped3bp on ped2b.partnerGeneralId = ped3bp.childId
                    LEFT OUTER JOIN Pedigree ped2p on ped1.partnerGeneralId = ped2p.childId
                    LEFT OUTER JOIN Pedigree ped3pb on ped2p.breedGeneraId = ped3pb.childId
                    LEFT OUTER JOIN Pedigree ped3pp on ped2p.partnerGeneralId = ped3pp.childId
                    INNER JOIN General genp1b on ped1.breedGeneraId = genp1b.generalId
                    INNER JOIN General genp1p on ped1.partnerGeneralId = genp1p.generalId
                    INNER JOIN General genp2bb ON ped2b.breedGeneraId = genp2bb.generalId
                    INNER JOIN General genp2bp ON ped2b.partnerGeneralId = genp2bp.generalId
                    INNER JOIN General genp2pb ON ped2p.breedGeneraId = genp2pb.generalId
                    INNER JOIN General genp2pp ON ped2p.partnerGeneralId = genp2pp.generalId
                    INNER JOIN General genp3bbb ON ped3bb.breedGeneraId = genp3bbb.generalId
                    INNER JOIN General genp3bbp ON ped3bb.partnerGeneralId = genp3bbp.generalId
                    INNER JOIN General genp3bpb ON ped3bp.breedGeneraId = genp3bpb.generalId
                    INNER JOIN General genp3bpp ON ped3bp.partnerGeneralId = genp3bpp.generalId
                    INNER JOIN General genp3pbb ON ped3pb.breedGeneraId = genp3pbb.generalId
                    INNER JOIN General genp3pbp ON ped3pb.partnerGeneralId = genp3pbp.generalId
                    INNER JOIN General genp3ppb ON ped3pp.breedGeneraId = genp3ppb.generalId
                    INNER JOIN General genp3ppp ON ped3pp.partnerGeneralId = genp3ppp.generalId
                    where ped1.childId = 1001;";
                */

            }

        }


        public static List<General> getChildGeneral(int breedGeneraId)
        {
            using (var db = new GameDataContext())
            {

                // PedigreeのchildIdとGeneralのgeneralIdを結合
                var generalList = db.Pedigree
                    .Join(db.General,
                          ped => ped.childId,
                          gen => gen.generalId,
                          (ped, gen) => new { ped, gen }
                          )
                    .Where(x => x.ped.breedGeneraId == breedGeneraId)
                    .Where(x => x.gen.years <= 10)
                    .Select(x => x.gen)
                    .ToList();


                return generalList;
            }



        }



    }
}
