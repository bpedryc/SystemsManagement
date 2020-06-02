using ProjectThesis.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.ViewModels
{
    public class RegisterStudentViewModel
    {
        public Student Student {get; set;}
        public User User { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; } 
        public IEnumerable<Specialty> Specialties { get; set; }
        public string UserPassword { get { return User.Password; } }

        //[DataType(DataType.Password)]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }
    }
}
