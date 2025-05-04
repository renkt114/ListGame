using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class FamilyLine
    {
        [Key]
        public int familyLineId { get; set; }
        public int childGeneralId { get; set; }
        public int motherGeneralId { get; set; }
        public int fatherGeneralId { get; set; }
    }
}
