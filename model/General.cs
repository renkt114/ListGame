using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class General
    {
        [Key]
        public int generalId { get; set; }
        public int? assignCorpId { get; set; }
        public int troopId { get; set; }
        public string? name { get; set; }
        public int years { get; set; }
        public int? rank { get; set; }
        public int commandAbility { get; set; }
        public int attackAbility { get; set; }
        public int defenseAbility { get; set; }
        public int managementAbility { get; set; }
        public int strategyAbility { get; set; }
        public int maneuverAbility { get; set; }
        public int reconAbility { get; set; }
        public int physicalAbility { get; set; }
        public int growthTypeId { get; set; }
        public int commandGrowth { get; set; }
        public int attackGrowth { get; set; }
        public int defenseGrowth { get; set; }
        public int managementGrowth { get; set; }
        public int strategyGrowth { get; set; }
        public int maneuverGrowth { get; set; }
        public int reconGrowth { get; set; }
        public int physicalGrowth { get; set; }
        public int commandFromNow { get; set; }
        public int attackFromNow { get; set; }
        public int defenseFromNow { get; set; }
        public int managementFromNow { get; set; }
        public int strategyFromNow { get; set; }
        public int maneuverFromNow { get; set; }
        public int reconFromNow { get; set; }
        public int physicalFromNow { get; set; }
        public int infantryAdapt { get; set; }
        public int cavalryAdapt { get; set; }
        public int rangedAdapt { get; set; }
        public int injured { get; set; }
        public int condition { get; set; }
        public int tier { get; set; }
        public int entryEra { get; set; }
        public int abilityType { get; set; }
    }
}
