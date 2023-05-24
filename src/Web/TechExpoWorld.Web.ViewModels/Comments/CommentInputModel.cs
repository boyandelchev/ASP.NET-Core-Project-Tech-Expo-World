namespace TechExpoWorld.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static TechExpoWorld.Common.GlobalConstants.Comment;

    public class CommentInputModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        [Display(Name = DisplayAddAComment)]
        public string Content { get; init; }

        public int? ParentCommentId { get; init; }
    }
}
