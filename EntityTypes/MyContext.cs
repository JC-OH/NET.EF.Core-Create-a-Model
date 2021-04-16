using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityTypes
{
    internal class MyContext : DbContext
    {
        //Blog is included because it's exposed in a DbSet property on the context.
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                "Data Source = localhost; Initial Catalog=EFModeling; Persist Security Info=True;User ID=test; Password=test;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //AuditEntry because it is specified in OnModelCreating.
            modelBuilder.Entity<AuditEntry>();
            //If you don't want a type to be included in the model, you can exclude it:
            modelBuilder.Ignore<BlogMetadata>();

            //With this configuration migrations will not create the AspNetUsers table, but IdentityUser is still included in the model and can be used normally.
            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        //Post is included because it's discovered via the Blog.Posts navigation property.
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Blog Blog { get; set; }
    }

    public class AuditEntry
    {
        public int AuditEntryId { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
    }

    public class IdentityUser
    {
        public int IdentityUserId { get; set; }

    }
    //Excluding types from the model
    [NotMapped]
    public class BlogMetadata
    {
        public DateTime LoadedFromDatabase { get; set; }
    }
}
