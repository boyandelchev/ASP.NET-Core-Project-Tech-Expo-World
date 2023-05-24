namespace TechExpoWorld.Web.ViewModels.Authors
{
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.Author;

    public class BecomeAuthorInputModel
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
