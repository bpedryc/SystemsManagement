using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    [Table("Specials")]
    public class Specialty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int FacId { get; set; }
        public Faculty Fac { get; set; }
    }
}
