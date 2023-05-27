namespace TechExpoWorld.Services.Data.Comments
{
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;

    using TechExpoWorld.Data.Models;
    using TechExpoWorld.Services.Mapping;

    using static TechExpoWorld.Common.GlobalConstants.System;

    public class CommentServiceModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public string Content { get; init; }

        public string CreatedOn { get; init; }

        public string UserName { get; init; }

        public int? ParentCommentId { get; init; }

        public IEnumerable<CommentServiceModel> ChildrenComments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, CommentServiceModel>()
                .ForMember(
                    m => m.CreatedOn,
                    opt => opt.MapFrom(c => c.CreatedOn.ToString(DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    m => m.UserName,
                    opt => opt.MapFrom(c => c.User.UserName));
        }
    }
}
