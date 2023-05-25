namespace TechExpoWorld.Services.Data.Comments
{
    using System.Collections.Generic;

    public class CommentServiceModel
    {
        public int Id { get; init; }

        public string Content { get; init; }

        public string CreatedOn { get; init; }

        public string UserName { get; init; }

        public int? ParentCommentId { get; init; }

        public IEnumerable<CommentServiceModel> ChildrenComments { get; set; } = new List<CommentServiceModel>();
    }
}
