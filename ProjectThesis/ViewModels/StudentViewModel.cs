﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProjectThesis.Models;

namespace ProjectThesis.ViewModels
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public string UserPassword { get { return Student.User.Password; } }

        [Compare("UserPassword", ErrorMessage = "Wpisane zostało inne hasło")]
        public string ConfirmPassword { get; set; }
    }
}
