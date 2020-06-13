using ProjectThesis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.ViewModels
{
    public class StudentPanelViewModel
    {
        public User User { get; set; }
        public Student Student { get; set; }
        public Thesis Thesis { get; set; }
        public User Supervisor { get; set; }
    }
}
