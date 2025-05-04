using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class GameMaster
    {
        [Key]
        public int GameMasterId { get; set; }
        public int eraNum { get; set; }
        public int termNum { get; set; }
        public int sectionNum { get; set; }
        public int prestige { get; set; }
    }
}
