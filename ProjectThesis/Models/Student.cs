using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int StudentId { get; set; }
        public int StudentNo { get; set; }
        public int DegreeCycle { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        

        [AllowNull]
        public int SupId { get; set; }
        public Supervisor Sup { get; set; }

        public Thesis ChosenThesis { get; set; }
    }
}
