using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    public class Supervisor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int SupervisorId { get; set; }
        public int StudentLimit { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int FacId { get; set; }
        public Faculty Fac { get; set; }
        
        public List<Thesis> Theses { get; set; }
        public List<Student> Students { get; set; }
    }
}
