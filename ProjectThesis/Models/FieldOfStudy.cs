using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    public class FieldOfStudy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int FieldOfStudyId { get; set; }
        public string Name { get; set; }
    }
}
