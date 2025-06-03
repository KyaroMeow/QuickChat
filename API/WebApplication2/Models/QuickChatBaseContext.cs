using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

public partial class QuickChatBaseContext : DbContext
{
    public QuickChatBaseContext()
    {
    }

    public QuickChatBaseContext(DbContextOptions<QuickChatBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=QuickChatBase;Username=postgres;Password=sa");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chats_pkey");

            entity.ToTable("chats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isgroup)
                .HasDefaultValue(false)
                .HasColumnName("isgroup");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("messages_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Chatid).HasColumnName("chatid");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Senderid).HasColumnName("senderid");
            entity.Property(e => e.Sentat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("sentat");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Chatid)
                .HasConstraintName("messages_chatid_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Senderid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("messages_senderid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatarurl)
                .HasMaxLength(255)
                .HasColumnName("avatarurl");
            entity.Property(e => e.Isonline)
                .HasDefaultValue(false)
                .HasColumnName("isonline");
            entity.Property(e => e.Lastonline).HasColumnName("lastonline");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasMany(d => d.Chats).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userchat",
                    r => r.HasOne<Chat>().WithMany()
                        .HasForeignKey("Chatid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userchats_chatid_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userchats_userid_fkey"),
                    j =>
                    {
                        j.HasKey("Userid", "Chatid").HasName("userchats_pkey");
                        j.ToTable("userchats");
                        j.IndexerProperty<int>("Userid").HasColumnName("userid");
                        j.IndexerProperty<int>("Chatid").HasColumnName("chatid");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
