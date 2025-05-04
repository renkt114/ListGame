using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class SeedingJoin
    {
        [Key]
        public int seedingJoinId { get; set; }
        public int breedGeneralId { get; set; }
        public int partnerGeneralId { get; set; }
        public int costPrestige { get; set; }
    }
}
