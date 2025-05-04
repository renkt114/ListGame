using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListSLG.model
{
    public class Troop
    {
        [Key]
        public int troopId { get; set; }
        public int corpId { get; set; }
        public int potisioinId { get; set; }
        public int troopTypeId { get; set; }
        public int orderNum { get; set; }
        public int soldierNum { get; set; }
        public int maxSoldierNum { get; set; }
        public Corp corp { get; set; }
        public General? general { get; set; }

    }
}
