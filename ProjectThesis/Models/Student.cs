using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    [Table("Studs")]
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Ten numer albumu jest niepoprawny")]
        //[Required(ErrorMessage = "Numer albumu jest wymagany")]
        //[Range(100000, 999999, ErrorMessage = "Ten numer albumu jest niepoprawny")]
        public int StudentNo { get; set; }

        [Required(ErrorMessage = "Rok studiów jest wymagany")]
        public int DegreeCycle { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Kierunek jest wymagany")]
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public Thesis ChosenThesis { get; set; }
    }
}
