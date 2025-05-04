using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    internal class CorpPlan
    {
        [Key]
        public int corpPlanId { get; set; }
        public int corpId { get; set; }
        public int missionId { get; set; }

    }
}
