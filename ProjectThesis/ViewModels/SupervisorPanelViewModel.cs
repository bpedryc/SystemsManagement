using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectThesis.Models;

namespace ProjectThesis.ViewModels
{
    public class SupervisorPanelViewModel
    {
        public IEnumerable<Student> Students;
        public IEnumerable<Thesis> ThesesNotChosen;
    }
}
