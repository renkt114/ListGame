using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.turn
{
    internal static class TurnConditionLogic
    {

        internal static void turnGeneralConditionChange(General general)
        {
            // 5割の確率でgeneral.conditionが1増加、あるいは1減少。最小値は1、最大値は5
             Random random = new Random();
            int conditionChange = random.Next(0, 100);
            // TODO 一旦はgeneral.managementAbilityの値で増加確率が最大3割増加。
            conditionChange += (int)((double)general.managementAbility * 0.3);


            int managementAbility = general.managementAbility;
            if (conditionChange < 50)
            {
                general.condition -= 1;
            }
            else
            {
                general.condition += 1;
            }

            if (general.condition < 1)
            {
                general.condition = 1;
            }
            else if (general.condition > 5)
            {
                general.condition = 5;
            }

            GeneralDao.updateGeneral(general);
        }   

    }
}
