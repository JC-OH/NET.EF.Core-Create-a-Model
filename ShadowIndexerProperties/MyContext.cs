using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShadowIndexerProperties
{
    internal class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                "Data Source = localhost; Initial Catalog=EFModeling2; Persist Security Info=True;User ID=test; Password=test;");
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            //Configuring shadow properties
            modelBuilder.Entity<Blog>()
                .Property<DateTime>("LastUpdated");
            //Configuring indexer properties
            /*modelBuilder.Entity<Blog>()
                .IndexerProperty<DateTime>("LastUpdated");*/

            //Property bag entity types
            /*modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Blog", bb =>
            {
                bb.Property<int>("BlogId");
                bb.Property<string>("Url");
                bb.Property<DateTime>("LastUpdated");
            });*/
            //Required and optional relationships
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .IsRequired();
            //Cascade delete
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            //The dependent side is considered optional by default, but can be configured as required.
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.BlogImage)
                .WithOne(i => i.Blog)
                .HasForeignKey<BlogImage>(b => b.BlogId);

            //With this configuration the columns corresponding to ShippingAddress will be marked as non-nullable in the database.
            /*            modelBuilder.Entity<Order>(
                            ob =>
                            {
                                ob.OwnsOne(
                                    o => o.ShippingAddress,
                                    sa =>
                                    {
                                        sa.Property(p => p.Street).IsRequired();
                                        sa.Property(p => p.City).IsRequired();
                                    });

                                ob.Navigation(o => o.ShippingAddress)
                                    .IsRequired();
                            });*/
            //Join entity type configuration
            modelBuilder
                .Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(p => p.Posts)
                .UsingEntity(j => j.ToTable("PostTags"));
            //--------------------------
            //- Indexes
            //--------------------------
            //You can specify an index over a column as follows
            //By default, indexes aren't unique: multiple rows are allowed to have the same value(s) for the index's column set. You can make an index unique as follows:
            //By convention, indexes created in a relational database are named IX_<type name>_<property name>. For composite indexes, <property name> becomes an underscore separated list of property names.
            modelBuilder.Entity<Blog>()
                .HasIndex(b => b.Url)
                .IsUnique()
                .HasDatabaseName("Index_Url");

            // An index can also span more than one column:
            modelBuilder.Entity<Person>()
                .HasIndex(p => new { p.FirstName, p.LastName });

            // Table-per-hierarchy and discriminator configuration
            modelBuilder.Entity<Blog>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Blog>("blog_base")
                .HasValue<RssBlog>("blog_rss");

            modelBuilder.Entity<Blog>()
                .Property("Discriminator")
                .HasMaxLength(200);
        }
        //Blog is the principal entity

        //[Index(nameof(Url))]
        //[Index(nameof(Url), IsUnique = true)]
        [Index(nameof(Url), IsUnique = true, Name = "Index_Url")]

        public class Blog {
            //Blog.BlogId is the principal key (in this case it is a primary key rather than an alternate key)
            public int BlogId { get; set; }
            public string Url { get; set; }
            //Blog.Posts is a collection navigation property
            public List<Post> Posts { get; set; }
            public BlogImage BlogImage { get; set; }
        }

        //Post is the dependent entity
        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            // Since there is no CLR property which holds the foreign
            // key for this relationship, a shadow property is created.

            //Foreign key shadow properties
            //For example, the following code listing will result in a BlogId shadow property being introduced to the Post entity:
            //Post.Blog is a reference navigation property
            //Post.Blog is the inverse navigation property of Blog.Posts (and vice versa)
            public Blog Blog { get; set; }
            //Post.BlogId is the foreign key
            public ICollection<Tag> Tags { get; set; }

            public int AuthorUserId { get; set; }
            public User Author { get; set; }

            public int ContributorUserId { get; set; }
            public User Contributor { get; set; }

        }

        public class BlogImage
        {
            public int BlogImageId { get; set; }
            public byte[] Image { get; set; }
            public string Caption { get; set; }
            [ForeignKey("BlogId")]
            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }

        public class Tag
        {
            public string TagId { get; set; }
            public ICollection<Post> Posts { get; set; }
        }

        public class User
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            [InverseProperty("Author")]
            public List<Post> AuthoredPosts { get; set; }

            [InverseProperty("Contributor")]
            public List<Post> ContributedToPosts { get; set; }
        }

        [Index(nameof(FirstName), nameof(LastName))]
        public class Person
        {
            public int PersonId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class RssBlog : Blog
        {
            public string RssUrl { get; set; }
        }

    }
}
