using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            modelBuilder.Entity<Blog>()
                .ToTable("blogs");
            modelBuilder.Entity<Blog>()
                .HasComment("Blogs managed on the web site.");
            //If you prefer to configure your columns with different names, you can do so as following code snippet:
            modelBuilder.Entity<Blog>()
                .Property(b => b.BlogId)
                .HasColumnName("BlogId");
            //You can configure a single property to be the primary key of an entity as follows:
            modelBuilder.Entity<Blog>()
                .HasKey(b => b.BlogId);
            modelBuilder.Entity<Blog>(
                eb => 
                {
                    eb.Property(b => b.Url).HasColumnType("varchar(200)");
                    eb.Property(b => b.Rating).HasColumnType("decimal(5, 2)");
                });
            //By convention, all public properties with a getter and a setter will be included in the model.
            //Specific properties can be excluded as follows:
            modelBuilder.Entity<Blog>()
                    .Ignore(b => b.LoadedFromDatabase);
            //In the following example, configuring a maximum length of 500 will cause a column of type nvarchar(500) to be created on SQL Server:
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .HasMaxLength(200);
            //You can set an arbitrary text comment that gets set on the database column, allowing you to document your schema in the database:
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .HasComment("The URL of the Blog");
            //A property that would be optional by convention can be configured to be required as follows:
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .IsRequired();
            //In the following example, configuring the Score property to have precision 14 and scale 2 will cause a column of type decimal(14,2) to be created on SQL Server, 
            //and configuring the LastUpdated property to have precision 3 will cause a column of type datetime2(3):
            modelBuilder.Entity<Blog>()
                .Property(b => b.Score)
                .HasPrecision(14, 2);
            /*modelBuilder.Entity<Blog>()
                .Property(b => b.LastUpdated)
                .HasPrecision(3);*/

            //Alternate Keys
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogUrl)
                .HasPrincipalKey(b => b.Url);

            //AuditEntry because it is specified in OnModelCreating.
            modelBuilder.Entity<AuditEntry>();
            //If you don't want a type to be included in the model, you can exclude it:
            modelBuilder.Ignore<BlogMetadata>();

            //With this configuration migrations will not create the AspNetUsers table, but IdentityUser is still included in the model and can be used normally.
            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

            //You can also configure multiple properties to be the key of an entity - this is known as a composite key. 
            //Composite keys can only be configured using the Fluent API; 
            //conventions will never set up a composite key, and you can not use Data Annotations to configure one.
            /*modelBuilder.Entity<Car>()
                .HasKey(c => new { c.State, c.LicensePlate });*/


            //----------------------------------------------
            // Generated Values - Default values
            //----------------------------------------------
            //On relational databases, a column can be configured with a default value; if a row is inserted without a value for that column, the default value will be used.
            //You can configure a default value on a property:
            modelBuilder.Entity<Blog>()
                .Property(b => b.Rating)
                .HasDefaultValue(3);
            //You can also specify a SQL fragment that is used to calculate the default value:
            //Configuring a date/time column to have the creation timestamp of the row is usually a matter of configuring a default value 
            //with the appropriate SQL function. For example, on SQL Server you may use the following:
            //Be sure to select the appropriate function, as several may exist (e.g. GETDATE() vs. GETUTCDATE()).
            modelBuilder.Entity<Blog>()
                .Property(b => b.Created)
                .HasDefaultValue("getdate()");

            //----------------------------------------------
            // Generated Values - Computed columns
            //----------------------------------------------
            //On most relational databases, a column can be configured to have its value computed in the database, 
            //typically with an expression referring to other columns:
            modelBuilder.Entity<Person>()
                .Property(p => p.DisplayName)
                .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");

            //The above creates a virtual computed column, whose value is computed every time it is fetched from the database. 
            //You may also specify that a computed column be stored (sometimes called persisted), 
            //meaning that it is computed on every update of the row, and is stored on disk alongside regular columns:
            modelBuilder.Entity<Person>()
                .Property(p => p.NameLength)
                .HasComputedColumnSql("LEN([LastName]) + LEN([FirstName])", stored: true);

            //----------------------------------------------
            // Primary keys
            //----------------------------------------------
            //Explicitly configuring value generation

            //We saw above that EF Core automatically sets up value generation for primary keys - but we may want to do the same for non-key properties. 
            //You can configure any property to have its value generated for inserted entities as follows:
            modelBuilder.Entity<Blog>()
                .Property(b => b.Inserted)
                .ValueGeneratedOnAdd();
            //Similarly, a property can be configured to have its value generated on add or update:
            modelBuilder.Entity<Blog>()
                .Property(b => b.LastUpdated)
                .ValueGeneratedOnAddOrUpdate();

            //----------------------------------------------
            // No value generation
            //----------------------------------------------
           /* modelBuilder.Entity<Blog>()
                .Property(b => b.BlogId)
                .ValueGeneratedNever();*/
        }
    }
    [Table("blogs")]
    public class Blog
    {
        //By convention, a property named Id or <type name>Id will be configured as the primary key of an entity.
        //You can configure a single property to be the primary key of an entity as follows:
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("BlogId")]
        public int BlogId { get; set; }
        //In the following example, configuring a maximum length of 500 will cause a column of type nvarchar(500) to be created on SQL Server:
        [Column(TypeName = "varchar(200)")]
        [MaxLength(200)]
        [Required]
        public string Url { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rating { get; set; }
        //Post is included because it's discovered via the Blog.Posts navigation property.
        public decimal Score { get; set; }
        public string Created { get; set; }
        //We saw above that EF Core automatically sets up value generation for primary keys - but we may want to do the same for non-key properties. 
        //You can configure any property to have its value generated for inserted entities as follows:
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }
        //Similarly, a property can be configured to have its value generated on add or update:
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        public List<Post> Posts { get; set; }

        //By convention, a property whose .NET type can contain null will be configured as optional, 
        //whereas properties whose .NET type cannot contain null will be configured as required. 
        //For example, all properties with .NET value types (int, decimal, bool, etc.) are configured as required, 
        //and all properties with nullable .NET value types (int?, decimal?, bool?, etc.) are configured as optional.

        // - If nullable reference types are disabled(the default), all properties with.NET reference types are configured as optional by convention(for example, string).
        // - If nullable reference types are enabled, properties will be configured based on the C# nullability of their .NET type: string? will be configured as optional, but string will be configured as required.
        [NotMapped]
        public DateTime LoadedFromDatabase { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string BlogUrl { get; set; }
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
        [Required] // Data annotations needed to configure as required
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; } // Data annotations needed to configure as required
        public string MiddleName { get; set; } // Optional by convention

    }
    public class Customer
    {
        //C# 8 introduced a new feature called nullable reference types (NRT), 
        //which allows reference types to be annotated, indicating whether it is valid for them to contain null or not. 

        //Using nullable reference types is recommended since it flows the nullability expressed in C# code to EF Core's model and to the database, 
        //and obviates the use of the Fluent API or Data Annotations to express the same concept twice.
        public int CustomerId { get; set; }
        public string FirstName { get; set; } // Required by convention
        public string LastName { get; set; } // Required by convention
        public string? MiddleName { get; set; } // Optional by convention

        // Note the following use of constructor binding, which avoids compiled warnings
        // for uninitialized non-nullable properties.
        public Customer(string firstName, string lastName, string? middleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }
    }

    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public int NameLength { get; set; }

    }
    //Excluding types from the model
    [NotMapped]
    public class BlogMetadata
    {
        public DateTime LoadedFromDatabase { get; set; }
    }

}