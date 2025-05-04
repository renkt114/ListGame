
using ListSLG.util;
using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.generate
{

    // 将軍生成ロジック　今は使っていない
    internal class NewGeneralGenerate
    {


        public List<General> generateNewGeneralList(int generateNum, int avgRank)
        {

            // CSVから将軍名リスト取得（ファイル操作の回数は押さえたいため、まずこれだけ変数にいれる）
            List<string> nameList = getNameCsv();

            List <General> generaterdGeneralList = new List <General>();
            for (int i = 0; i < generateNum; i++)
            {
                General general = new General();
                // 名前生成
                general.name = generateName(nameList);
                // TODO 初期ランクどうするか
                general.rank = 0;
                // TODO　初期年数どうするか
                general.years = 0;
                // TODO パラメータの平均値どうするか
                general.commandAbility = generateParameter(avgRank);
                general.attackAbility = generateParameter(avgRank);
                general.defenseAbility = generateParameter(avgRank);
                general.managementAbility = generateParameter(avgRank);
                general.strategyAbility = generateParameter(avgRank);
                general.maneuverAbility = generateParameter(avgRank);
                general.reconAbility = generateParameter(avgRank);
                general.physicalAbility = generateParameter(avgRank);

                general.commandGrowth = generategrowth(avgRank);
                general.attackGrowth = generategrowth(avgRank);
                general.defenseGrowth = generategrowth(avgRank);
                general.managementGrowth = generategrowth(avgRank);
                general.strategyGrowth = generategrowth(avgRank);
                general.maneuverGrowth = generategrowth(avgRank);
                general.reconGrowth = generategrowth(avgRank);
                general.physicalGrowth = generategrowth(avgRank);

                generaterdGeneralList.Add(general);

            }
            return generaterdGeneralList;
        }

        // 各種能力値（1～99）生成処理
        private int generateParameter(int paramRank)
        {
            // 能力値平均9以上が指定されたら9にする
            if (paramRank > 9)
            {
                paramRank = 9;
            }


            Random random = new Random();
            int[] paramArry = new int[9];

            // 1～99の乱数を10個作る
            for (int i = 0; i < 9; i++)
            {
                paramArry[i] = random.Next(0, 99);
            }

            // 昇順（低い順）に並べる
            paramArry = paramArry.OrderBy(x => x).ToArray();
            
            // 下からx番目（引数の0～9）を返す。これで例えば6ならだいたい70前後になる想定
            return paramArry[paramRank];


        }

        private int generategrowth(int paramRank)
        {
            // 能力値平均9以上が指定されたら9にする
            if (paramRank > 9)
            {
                paramRank = 9;
            }


            Random random = new Random();
            int[] paramArry = new int[9];

            // 1～9の乱数を10個作る
            for (int i = 0; i < 9; i++)
            {
                paramArry[i] = random.Next(0, 9);
            }

            // 昇順（低い順）に並べる
            paramArry = paramArry.OrderBy(x => x).ToArray();

            // 下からx番目（引数の0～9）を返す。これで例えば6ならだいたい70前後になる想定
            return paramArry[paramRank];


        }


        // CSVから将軍名リスト取得
        private List<string> getNameCsv()
        {

            return ResourceFileUtil.GetGeneralNameByCsvFile();


        }

        // 将軍名リストから名前取得
        //TODO　名前の重複回避
        private string generateName(List<string> generalNameList)
        {
            Random random = new Random();
            // 0～リスト件数の乱数を取得し、その番号の名前を返す
            return generalNameList[random.Next(0, generalNameList.Count)];

        }



    }
}
