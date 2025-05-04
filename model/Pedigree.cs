using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.model
{
    public class Pedigree
    {

        [Key]
        public int pedigreeId { get; set; }
        public int childId { get; set; }
        public int breedGeneraId { get; set; }
        public int partnerGeneralId { get; set; }

    }
}
