using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class Corp
    {
        [Key]
        public int corpId { get; set; }
        public string? corpName { get; set; }
        public List<Troop>? troop { get; set; }
    }
}
