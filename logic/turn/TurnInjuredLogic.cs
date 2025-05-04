using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.turn
{
    internal static class TurnInjuredLogic
    {

        internal static void turnGeneralInjuredChange(General general)
        {
            // general.injuredが0より大きい場合、general.injuredを1減少
            if (general.injured > 0)
            {
                general.injured -= 1;
            }

            GeneralDao.updateGeneral(general);
        }   

    }
}
