namespace TechExpoWorld.Models.Authors
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Author;

    public class BecomeAuthorFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; init; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; init; }

        [Required]
        [Url]
        [Display(Name = "Photo URL")]
        public string PhotoUrl { get; init; }
    }
}
