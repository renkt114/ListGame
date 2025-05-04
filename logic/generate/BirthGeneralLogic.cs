using ListSLG.model;
using ListSLG.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.generate
{
    class BirthGeneralLogic
    {

        // 将軍生成
        public General BirthGeneral(General mother, General father)
        {
            // 父親のabilityTypeで得意技能決定

            int atcGrowth = -1;
            int defGrowth = -1;
            int manGrowth = -1;

            switch (father.abilityType)
            {
                case 1:
                    atcGrowth = 2;
                    break;
                case 2:
                    defGrowth = 2;
                    break;
                case 3:
                    manGrowth = 2;
                    break;
                default:
                    break;
            }


            // CSVから将軍名リスト取得（ファイル操作の回数は押さえたいため、まずこれだけ変数にいれる）
            List<string> nameList = getNameCsv();

            General newGeneral = new General();

            newGeneral.attackAbility = genAbilityCalc(mother.attackAbility, father.attackAbility);
            //newGeneral.commandAbility = genAbilityCalc(mother.commandAbility, father.commandAbility);
            newGeneral.defenseAbility = genAbilityCalc(mother.defenseAbility, father.defenseAbility);
            //newGeneral.managementAbility = genAbilityCalc(mother.managementAbility, father.managementAbility);
            newGeneral.maneuverAbility = genAbilityCalc(mother.maneuverAbility, father.maneuverAbility);
            //newGeneral.physicalAbility = genAbilityCalc(mother.physicalAbility, father.physicalAbility);
            //newGeneral.reconAbility = genAbilityCalc(mother.reconAbility, father.reconAbility);
            //newGeneral.strategyAbility = genAbilityCalc(mother.strategyAbility, father.strategyAbility);

            newGeneral.attackGrowth = genGrowthCalc(mother.attackGrowth, atcGrowth);
            //newGeneral.commandGrowth = genGrowthCalc(mother.commandGrowth, father.commandGrowth);
            newGeneral.defenseGrowth = genGrowthCalc(mother.defenseGrowth, defGrowth);
            //newGeneral.managementGrowth = genGrowthCalc(mother.managementGrowth, father.managementGrowth);
            newGeneral.maneuverGrowth = genGrowthCalc(mother.maneuverGrowth, manGrowth);
            //newGeneral.physicalGrowth = genGrowthCalc(mother.physicalGrowth, father.physicalGrowth);
            //newGeneral.reconGrowth = genGrowthCalc(mother.reconGrowth, father.reconGrowth);
            //newGeneral.strategyGrowth = genGrowthCalc(mother.strategyGrowth, father.strategyGrowth);

            newGeneral.rank = 0;
            newGeneral.years = 0;
            newGeneral.injured = 0;
            newGeneral.condition = 0;

            // TODO 適正値の遺伝
            newGeneral.infantryAdapt = 5;
            newGeneral.cavalryAdapt = 5;
            newGeneral.rangedAdapt = 5;


            newGeneral.name = generateName(nameList);


            // アドバイス追加
            AdviseGenerateLogic.birthAdvice(newGeneral);

            return newGeneral;


        }

        // 能力値遺伝
        public int genAbilityCalc(int motherAbility, int fatherAbility)
        {

            // 親の能力値の平均値を取り、それを1/5にしたもの(都合合計値の1/10)を子供の能力値とする
            int param = (motherAbility + fatherAbility) / 10;

            return param;

        }

        // 成長率遺伝
        public int genGrowthCalc(int motherGrowth, int growth)
        {
            // motherGrowthの値+0～-1の範囲でランダムに変動
            Random random = new Random();
            int param = random.Next(motherGrowth - 1, motherGrowth + 1) + growth;

            // 0以下の場合は1にする。9以上の場合は9にする
            if (param <= 0)
            {
                param = 1;
            }
            else if (param >= 9)
            {
                param = 9;
            }

            return param;

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
