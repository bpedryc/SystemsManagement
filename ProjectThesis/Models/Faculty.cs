﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectThesis.Models
{
    public class Faculty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int FacultyId { get; set; }
        public string Name { get; set; }
    }
}