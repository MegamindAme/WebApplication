using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.DataDB;

public partial class TestApiContext : DbContext
{
    public TestApiContext()
    {
    }

    public TestApiContext(DbContextOptions<TestApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WebApplication4.Models.Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebApplication4.Models.Task>(entity =>
        {
            entity.ToTable("tasks");

            entity.Property(e => e.ID)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Assignee).HasColumnName("assignee");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Duedate)
                .HasColumnType("date")
                .HasColumnName("duedate");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.AssigneeNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.Assignee)
                .HasConstraintName("FK_user_tasks");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_Users");

            entity.ToTable("users");

            entity.Property(e => e.ID)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Password)
            .IsUnique();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
