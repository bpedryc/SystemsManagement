using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    public class Thesis
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ThesisId { get; set; }
        public string Subject { get; set; }
        public int DegreeCycle { get; set; }

        public int FosId { get; set; }
        public FieldOfStudy Fos { get; set; }
        
        public int SupId { get; set; }
        public Supervisor Sup { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
