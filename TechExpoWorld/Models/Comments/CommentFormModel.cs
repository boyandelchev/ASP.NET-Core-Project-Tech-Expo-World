namespace TechExpoWorld.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static GlobalConstants.Comment;

    public class CommentFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        [Display(Name = DisplayAddComment)]
        public string Content { get; init; }
    }
}
