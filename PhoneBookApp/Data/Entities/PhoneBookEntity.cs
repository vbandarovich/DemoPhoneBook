using PhoneBookApp.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookApp.Data.Entities
{
    public class PhoneBookEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ContactType Type { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string TextComments { get; set; }

        //Additional for type Person
        public string BirthDate { get; set; }

        //Additional for type Public Organisation
        public string RegistrationNumber { get; set; }

        //Additional for type Private Organisation
        public string OwnerName { get; set; }
    }
}
