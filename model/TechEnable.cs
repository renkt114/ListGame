using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class TechEnable
    {
        [Key]
        public int TechEnableId { get; set; }
        public int techId { get; set; }
        public bool Enable { get; set; }
    }
}
