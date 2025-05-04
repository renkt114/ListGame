using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class Advice
    {

        [Key]
        public int adviseId { get; set; }
        public int priority { get; set; }
        public String adviseText { get; set; }

    }
}
