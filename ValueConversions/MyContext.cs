using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;

namespace ValueConversions
{
    public class MyContext : DbContext
    {
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Rider1> Rider1s { get; set; }
        public DbSet<Rider2> Rider2s { get; set; }
        public DbSet<Rider3> Rider3s { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                "Data Source = localhost; Initial Catalog=EFModeling3; Persist Security Info=True;User ID=test; Password=test;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Conversions can be configured in OnModelCreating to store the enum values as strings such as "Donkey", "Mule", etc. 
            //in the database; you simply need to provide one function which converts from the ModelClrType to the ProviderClrType, and another for the opposite conversion:
            modelBuilder
                .Entity<Rider>()
                .Property(e => e.Mount)
                .HasConversion(
                    v => v.ToString(),
                    v => (EquineBeast)Enum.Parse(typeof(EquineBeast), v)
                 );
              //Some database types have facets that modify how the data is stored.These include:
              // - Precision and scale for decimals and date / time columns
              // - Size / length for binary and string columns
              // - Unicode for string columns

              //---------------------------------
              //- Pre-defined conversions
              //---------------------------------
              //For example, enum to string conversions are used as an example above, 
              //but EF Core will actually do this automatically when the provider type is configured as string using the generic type of HasConversion:

              modelBuilder
                .Entity<Rider1>()
                .Property(e => e.Mount)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode(false); 

            //The same thing can be achieved by explicitly specifying the database column type. For example, if the entity type is defined like so:
            modelBuilder
                .Entity<Rider2>()
                .Property(e => e.Mount)
                .HasColumnType("nvarchar(24)");
            //---------------------------------
            //- The ValueConverter class
            //---------------------------------
            var converter = new ValueConverter<EquineBeast, string>(
                v => v.ToString(),
                v => (EquineBeast)Enum.Parse(typeof(EquineBeast), v));
            modelBuilder
                .Entity<Rider3>()
                .Property(e => e.Mount)
                .HasConversion(converter)
                .HasMaxLength(20)
                .IsUnicode(false); ;
            //---------------------------------
            //- Built-in converters
            //---------------------------------
            modelBuilder
                .Entity<Rider>()
                .Property(e => e.IsActive)
                .HasConversion<int>();
            var converter1 = new BoolToZeroOneConverter<int>();

            modelBuilder
                .Entity<Rider1>()
                .Property(e => e.IsActive)
                .HasConversion(converter1);
            //---------------------------------
            //- Column facets and mapping hints
            //---------------------------------
            //However, if by default all EquineBeast columns should be varchar(20), then this information can be given to the value converter as a ConverterMappingHints. For example:
            /*var converter2 = new ValueConverter<EquineBeast, string>(
                v => v.ToString(),
                v => (EquineBeast)Enum.Parse(typeof(EquineBeast), v),
                new ConverterMappingHints(size: 20, unicode: false));

            modelBuilder
                .Entity<Rider>()
                .Property(e => e.Mount)
                .HasConversion(converter1);*/
            //---------------------------------
            //- Simple value objects
            //---------------------------------
            modelBuilder
                .Entity<Order>()
                .Property(e => e.Price)
                .HasConversion(
                    v => v.Amount,
                    v => new Dollars(v)
                );

            //---------------------------------
            //- Composite value objects
            //---------------------------------
            modelBuilder
                .Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Money>(v, null));

            //---------------------------------
            //- Collections of value objects
            //---------------------------------
            //Using System.Text.Json again:
            //ICollection<string> represents a mutable reference type. This means that a ValueComparer<T> is needed so that EF Core can track and detect changes correctly. See Value Comparers for more information.
            modelBuilder.Entity<Post>()
                .Property(e => e.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, null),
                    new ValueComparer<ICollection<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => (ICollection<string>)c.ToList()));
        }

        public class Rider
        {
            [Key]
            public int Id { get; set; }
            public EquineBeast Mount { get; set; }
            public bool IsActive { get; set; }
        }
        public class Rider1
        {
            [Key]
            public int Id { get; set; }
            public EquineBeast Mount { get; set; }
            public bool IsActive { get; set; }
        }
        public class Rider2
        {
            [Key]
            public int Id { get; set; }
            [Column(TypeName = "nvarchar(24)")]
            public EquineBeast Mount { get; set; }
            public bool IsActive { get; set; }
        }

        public class Rider3
        {
            [Key]
            public int Id { get; set; }
            public EquineBeast Mount { get; set; }
        }
        public enum EquineBeast
        {
            Donkey,
            Mule,
            Horse,
            Unicorn
        }
        //---------------------------------
        //- Simple value objects
        //---------------------------------
        public class Order
        {
            public int Id { get; set; }
            public Dollars Price { get; set; }
        }
        public readonly struct Dollars
        {
            public Dollars(decimal amount)
                => Amount = amount;

            public decimal Amount { get; }

            public override string ToString()
                => $"${Amount}";
        }
        //---------------------------------
        //- Composite value objects
        //---------------------------------
        public class OrderDetail
        {
            public int Id { get; set; }

            public Money Price { get; set; }
        }
        public readonly struct Money
        {
            [JsonConstructor]
            public Money(decimal amount, Currency currency)
            {
                Amount = amount;
                Currency = currency;
            }

            public override string ToString()
                => (Currency == Currency.UsDollars ? "$" : "£") + Amount;

            public decimal Amount { get; }
            public Currency Currency { get; }
        }

        public enum Currency
        {
            UsDollars,
            PoundsStirling
        }

        //---------------------------------
        //- Collections of value objects
        //---------------------------------
        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Contents { get; set; }

            public ICollection<string> Tags { get; set; }
        }

        //---------------------------------
        //- Use case-insensitive string keys
        //---------------------------------

        //---------------------------------
        //- Handle fixed-length database strings
        //---------------------------------

        //---------------------------------
        //- Encrypt property values
        //---------------------------------
    }

}
