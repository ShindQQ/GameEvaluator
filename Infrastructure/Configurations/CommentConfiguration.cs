using Domain.Entities.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.Id).HasConversion(
            commentId => commentId.Value,
            value => new CommentId(value));

        builder.Property(comment => comment.Text)
            .HasMaxLength(500);

        builder.HasOne(comment => comment.Game).WithMany(game => game.Comments)
            .HasForeignKey(comment => comment.GameId);

        builder.HasOne(comment => comment.User).WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId);

        builder.HasMany(comment => comment.ChildrenComments)
            .WithOne(comment => comment.ParentComment)
            .HasForeignKey(comment => comment.ParentCommentId);
    }
}
