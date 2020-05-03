using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThesis.Models
{
    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
