using PhoneBookApp.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookApp.Common.Models
{
    public class PersonContactModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ContactType Type { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string TextComments { get; set; }

        [Required]
        public string BirthDate { get; set; }

    }
}
