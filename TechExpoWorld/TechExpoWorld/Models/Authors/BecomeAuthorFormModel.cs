namespace TechExpoWorld.Models.Authors
{
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.Author;

    public class BecomeAuthorFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = DisplayPhoneNumber)]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; init; }

        [Required]
        [Url]
        [Display(Name = DisplayPhotoUrl)]
        public string PhotoUrl { get; init; }
    }
}
