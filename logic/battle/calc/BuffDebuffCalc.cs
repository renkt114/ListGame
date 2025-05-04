using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.battle.calc
{
    internal static class BuffDebuffCalc
    {

        // リーダーのバフ
        internal static General leaderBuff(General general,General leaderGeneral)
        {
            // leaderGeneralの各Abilityの20%をgeneralの各Abilityに加算する。
            general.commandAbility += (int)(leaderGeneral.commandAbility * 0.2);
            general.attackAbility += (int)(leaderGeneral.attackAbility * 0.2);
            general.defenseAbility += (int)(leaderGeneral.defenseAbility * 0.2);
            general.managementAbility += (int)(leaderGeneral.managementAbility * 0.2);
            general.strategyAbility += (int)(leaderGeneral.strategyAbility * 0.2);
            general.maneuverAbility += (int)(leaderGeneral.maneuverAbility * 0.2);
            general.reconAbility += (int)(leaderGeneral.reconAbility * 0.2);
            general.physicalAbility += (int)(leaderGeneral.physicalAbility * 0.2);


            return general;

        }

        // 怪我のデバフ
        internal static General injuredDeBuff(General general)
        {

            // injuredが1以上なら各Abilityを3割減
            if (general.injured >= 1)
            {
                general.commandAbility = (int)(general.commandAbility * 0.7);
                general.attackAbility = (int)(general.attackAbility * 0.7);
                general.defenseAbility = (int)(general.defenseAbility * 0.7);
                general.managementAbility = (int)(general.managementAbility * 0.7);
                general.strategyAbility = (int)(general.strategyAbility * 0.7);
                general.maneuverAbility = (int)(general.maneuverAbility * 0.7);
                general.reconAbility = (int)(general.reconAbility * 0.7);
                general.physicalAbility = (int)(general.physicalAbility * 0.7);
            }

            return general;

        }

        // 調子のデバフ
        internal static General conditionBuff(General general)
        {

            // 3を基準に計算
            double cond = 3 - general.condition;
            cond = 1.0 + (cond * -0.1);

            general.commandAbility = (int)(general.commandAbility * cond);
            general.attackAbility = (int)(general.attackAbility * cond);
            general.defenseAbility = (int)(general.defenseAbility * cond);
            general.managementAbility = (int)(general.managementAbility * cond);
            general.strategyAbility = (int)(general.strategyAbility * cond);
            general.maneuverAbility = (int)(general.maneuverAbility * cond);
            general.reconAbility = (int)(general.reconAbility * cond);
            general.physicalAbility = (int)(general.physicalAbility * cond);

            return general;

        }

        // 歩兵技術バフ
        internal static General infTechBuff(General general)
        {

            general.defenseAbility = (int)(general.defenseAbility + 10);

            return general;

        }

        // 騎兵技術バフ
        internal static General cavTechBuff(General general)
        {

            general.maneuverAbility = (int)(general.maneuverAbility + 10);

            return general;

        }

        // 投射技術バフ
        internal static General rangeTechBuff(General general)
        {

            general.attackAbility = (int)(general.attackAbility + 10);

            return general;

        }


    }
}
