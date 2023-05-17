using Application.Common.Models.DTOs;
using AutoMapper;
using Domain.Entities.Comments;

namespace Application.Common.Mappings;

public class CommentResolver : IValueResolver<Comment, CommentDto, CommentDto>
{
    public CommentDto Resolve(Comment source, CommentDto destination, CommentDto destMember, ResolutionContext context)
    {
        return MapCommentToCommentDto(source)!;
    }

    public CommentDto? MapCommentToCommentDto(Comment comment)
    {
        if (comment is null)
            return null;

        var commentDto = new CommentDto
        {
            Id = comment.Id.Value,
            GameId = comment.GameId.Value,
            LeftBy = comment.User.Name,
            Text = comment.Text,
            ParentCommentId = comment.ParentCommentId!.Value
        };

        if (comment.ChildrenComments is not null && comment.ChildrenComments.Count > 0)
        {
            foreach (var child in comment.ChildrenComments)
            {
                var childDto = MapCommentToCommentDto(child);
                commentDto.ChildrenComments.Add(childDto!);
            }
        }

        return commentDto;
    }
}
