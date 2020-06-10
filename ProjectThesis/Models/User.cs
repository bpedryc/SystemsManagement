using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThesis.Models
{
    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "To imię nie jest poprawne")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "To nazwisko nie jest poprawne")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ten adres email nie jest poprawny")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        //TODO: regex for password
        public string Password { get; set; }
    }
}
