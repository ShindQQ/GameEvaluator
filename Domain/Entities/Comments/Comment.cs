using Domain.Entities.Games;
using Domain.Entities.Users;

namespace Domain.Entities.Comments;

public sealed class Comment
{
    public CommentId Id { get; private set; } = null!;

    public string Text { get; private set; } = string.Empty;

    public GameId GameId { get; private set; } = null!;

    public Game Game { get; private set; } = null!;

    public UserId UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public CommentId? ParentCommentId { get; private set; }

    public Comment? ParentComment { get; private set; }

    public HashSet<Comment> ChildrenComments { get; private set; } = new();

    public static Comment Create(string text, Game game, User user)
        => new()
        {
            Id = new CommentId(Guid.NewGuid()),
            GameId = game.Id,
            UserId = user.Id,
            Game = game,
            User = user,
            Text = text,
            ParentCommentId = null,
            ParentComment = null
        };

    public Comment CreateChild(string text, User user)
    {
        var comment = Create(text, Game, user);
        comment.ParentCommentId = Id;
        comment.ParentComment = this;

        ChildrenComments.Add(comment);

        return comment;
    }

    public void Update(string text)
    {
        Text = text;
    }
}
