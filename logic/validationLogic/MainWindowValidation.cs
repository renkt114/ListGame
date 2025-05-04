using ListSLG.logic.resource;
using ListSLG.Migrations;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ListSLG.logic.validationLogic
{
    internal static class MainWindowValidation
    {

        public static List<string> turnEndValidation()
        {
            List<string> validationErrorList = new List<string>();

            GameMaster gameMaster = GameMasterDao.getGameMaster();

            if (gameMaster.sectionNum <= 4)
            {
                validationErrorList.AddRange(planValidation());
            }else
            {
                validationErrorList.AddRange(termValidation());
            }

            return validationErrorList;


        }

        // 期末以外のターンエンド時のバリデーション
        private static List<string> planValidation()
        {

            List<string> validationErrorList = new List<string>();


            List<Corp> corpList = CorpDao.getAllAllyCorpDataNotZero();
            foreach (Corp corp in corpList)
            {
                // 中央歩兵ありチェック
                if (corp.troop.Where(x => x.potisioinId == 1).Count() == 0)
                {
                    validationErrorList.Add(corp.corpName + "に中央歩兵がいません。ポジションを変更してください");
                }

                // 同ポジションチェック（potisioinId=0は除外）

                // TODO 表記がずれている？未配属に変更時に更新されていない？順序をクリアできない

                var potisioinCntList = corp.troop.Where(x => x.potisioinId != 0).GroupBy(x => x.potisioinId).Select(y => new { pos = y.Key, count = y.Count() }).Where(z => z.count >= 2);
                foreach (var potisioinCnt in potisioinCntList)
                {

                    validationErrorList.Add(corp.corpName + "に" + Enum.GetName(typeof(PotisioinDivName), potisioinCnt.pos - 1) + "が2部隊以上います。ポジションを変更してください");
                }

                // 同orderNumチェック
                var orderNumCntList = corp.troop.GroupBy(x => x.orderNum).Select(y => new { orderNum = y.Key, count = y.Count() }).Where(z => z.orderNum >= 1).Where(z => z.count >= 2);
                if (orderNumCntList.Count() > 0)
                {
                    validationErrorList.Add(corp.corpName + "に" + orderNumCntList.First().orderNum + "番の部隊が2部隊以上います。攻撃順を変更してください");
                }

                // orderNum未入力チェック
                var orderNumNotList = corp.troop.Where(z => z.orderNum == 0).Where(x => x.potisioinId != 0);
                if (orderNumNotList.Count() > 0)
                {
                    validationErrorList.Add(corp.corpName + "に攻撃順未指定の部隊があります。攻撃順を指定してください");
                }

                // 作戦チェック
                if (CorpPlanDao.getCorpPlanByCorp(corp.corpId) == null)
                {
                    validationErrorList.Add(corp.corpName + "の計画が未設定です。計画を設定してください。");
                }else if (CorpDao.getPositiondCropTroopNum(corp.corpId)  > CorpPlanDao.getCorpPlanMissoinByCorp(corp).mission.generalNum)
                {
                    validationErrorList.Add(corp.corpName + "の部隊数が作戦の上限を超えています。ポジションが未配属以外の将軍を" + CorpPlanDao.getCorpPlanMissoinByCorp(corp).mission.generalNum + "以下にしてください。");
                }

            }



            return validationErrorList;
        }

        // 期末時のバリデーション
        private static List<string> termValidation()
        {
            List<string> validationErrorList = new List<string>();

            if (GameMasterDao.getGameMaster().prestige < PrestigeLogic.calcCostPrestige())
            {
                validationErrorList.Add("威信消費量が所有威信を超過しています。婚姻または昇格予定を変更してください。");

            }

            return validationErrorList;
        }





    }
}
