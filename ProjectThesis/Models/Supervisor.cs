using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    [Table("Supers")]
    public class Supervisor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }
        public int StudentLimit { get; set; }

        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        
        public List<Thesis> Theses { get; set; }
        public List<Student> Students { get; set; }
    }
}
