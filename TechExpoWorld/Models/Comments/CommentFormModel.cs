namespace TechExpoWorld.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.Comment;

    public class CommentFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        [Display(Name = DisplayAddAComment)]
        public string Content { get; init; }
    }
}
