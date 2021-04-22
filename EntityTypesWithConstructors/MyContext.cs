using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EntityTypesWithConstructors
{
    public class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                "Data Source = localhost; Initial Catalog=EFModeling4; Persist Security Info=True;User ID=test; Password=test;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Blog>(
                b =>
                {
                    b.HasKey("_id");
                    b.Property(e => e.Author);
                    b.Property(e => e.Name);
                });

            modelBuilder.Entity<Post>(
                b =>
                {
                    b.HasKey("_id");
                    b.Property(e => e.Title);
                    b.Property(e => e.PostedOn);
                });*/
        }
        //--------------------------------------------
        //-Binding to mapped properties
        //--------------------------------------------
        //Consider a typical Blog/Post model:
        //When EF Core creates instances of these types, such as for the results of a query, it will first call the default parameterless constructor and then set each property to the value from the database.However, if EF Core finds a parameterized constructor with parameter names and types that match those of mapped properties, then it will instead call the parameterized constructor with values for those properties and will not set each property explicitly.For example:
        public class Blog
        {
            public Blog(int id, string name, string author)
            {
                Id = id;
                Name = name;
                Author = author;
            }
            public int Id { get; private set; }
            public string Name { get; set; }
            public string Author { get; set; }

            public ICollection<Post> Posts { get; } = new List<Post>();
        }
        //EF Core sees a property with a private setter as read-write, 
        //which means that all properties are mapped as before and the key can still be store-generated.
        public class Post
        {
            public Post(int id, string title, DateTime postedOn)
            {
                Id = id;
                Title = title;
                PostedOn = postedOn;
            }
            public int Id { get; private set; }

            public string Title { get; private set; }
            public string Content { get; set; }
            public DateTime PostedOn { get; private set; }

            public Blog Blog { get; set; }
        }

        /*public class Blog
        {
            private int _id;

            public Blog(string name, string author)
            {
                Name = name;
                Author = author;
            }

            public string Name { get; }
            public string Author { get; }

            public ICollection<Post> Posts { get; } = new List<Post>();
        }

        public class Post
        {
            private int _id;

            public Post(string title, DateTime postedOn)
            {
                Title = title;
                PostedOn = postedOn;
            }

            public string Title { get; }
            public string Content { get; set; }
            public DateTime PostedOn { get; }

            public Blog Blog { get; set; }
        }*/
    }
}
