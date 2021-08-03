namespace TechExpoWorld.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Comment;

    public class CommentFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        [Display(Name ="Add Comment")]
        public string Content { get; init; }
    }
}
