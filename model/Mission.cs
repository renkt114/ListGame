using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    internal class Mission
    {
        [Key]
        public int missionId { get; set; }
        public string? missionName { get; set; }
        public string? missionText { get; set; }
        public int enemyCorpId { get; set; }
        public int gainedPrestige { get; set; }
        public int level { get; set; }
        public int generalNum { get; set; }
    }
}
