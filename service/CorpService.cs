using ListSLG.devtools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ListSLG.model
{
    internal class CorpService
    {
        public static Troop? serchPotisionTroop(List<Troop> troopList,int potisionId)
        {
            foreach (var troop in troopList)
            {
                if (troop.potisioinId == potisionId)
                {
                    return troop;
                }

            }

            return null;

        }

    }
}
