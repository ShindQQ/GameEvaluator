﻿using Domain.Entities.Intermidiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
{
    public void Configure(EntityTypeBuilder<UserGame> builder)
    {
        builder.ToTable("UserGame");

        builder.HasKey(userGame => new { userGame.UserId, userGame.GameId });

        builder.HasOne(userGame => userGame.User)
            .WithMany(user => user.UserGames)
            .HasForeignKey(userGame => userGame.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(userGame => userGame.Game)
            .WithMany(user => user.GameUsers)
            .HasForeignKey(userGame => userGame.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
