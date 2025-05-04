using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class RenewalGeneral
    {
        [Key]
        public int renewalGeneralId { get; set; }

        public int generalId { get; set; }

        public int promotionFlg { get; set; }

        public int retireFlg { get; set; }
    }
}
