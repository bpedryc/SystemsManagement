using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectThesis.Models;

namespace ProjectThesis.ViewModels
{
    public class ThesesListViewModel
    {
        public int FacultyId { get; set; }
        public int DegreeCycle { get; set; }
        public IEnumerable<Supervisor> Supervisors { get; set; }
        public Dictionary<int, int> SupervisorsByStudentCounts { get; set; }
    }
}
