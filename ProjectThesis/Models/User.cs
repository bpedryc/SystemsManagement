using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThesis.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "To imię nie jest poprawne")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [RegularExpression(@"^[a-zA-ZżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "To nazwisko nie jest poprawne")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Ten adres email nie jest poprawny")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [MinLength(6, ErrorMessage = "Hasło musi składać się z conajmniej 6 znaków")]
        public string Password { get; set; }
    }
}
