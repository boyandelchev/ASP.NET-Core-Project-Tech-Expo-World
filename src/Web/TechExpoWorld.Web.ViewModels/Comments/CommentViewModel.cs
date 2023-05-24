namespace TechExpoWorld.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class CommentViewModel
    {
        public int Id { get; init; }

        public string Content { get; init; }

        public string CreatedOn { get; init; }

        public string UserName { get; init; }

        public int? ParentCommentId { get; init; }

        public IEnumerable<CommentViewModel> ChildrenComments { get; set; } = new List<CommentViewModel>();
    }
}
