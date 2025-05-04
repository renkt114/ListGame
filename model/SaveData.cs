using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class SaveData
    {
        [Key]
        public int saveDataId { get; set; }
        public string? date { get; set; }
        public string? destination { get; set; }
    }
}
