using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeCorner2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string YourUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string TK { get; set; }
        public string Telephone { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        const string connectionString = @"Data Source=DESKTOP-G20562D\SQLEXPRESS;Initial Catalog=HomeCorner;Integrated Security=True";

        public ApplicationDbContext()
            : base(connectionString, throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Collection managing housies
        /// </summary>
        public DbSet<House> Houses { get; set; }

        /// <summary>
        /// Collection managing customers
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Features> Features { get; set; }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configures one-to-many relationship
            modelBuilder.Entity<House>()
                .HasRequired<Customer>(s => s.Owner)
                .WithMany(g => g.Houses)
                .HasForeignKey<int>(s => s.OwnerId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<House>()
                .HasRequired<Region>(s => s.Region)
                .WithMany(g => g.Houses)
                .HasForeignKey<byte>(s => s.RegionId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Images>()
                .HasRequired<House>(s => s.House)
                .WithMany(g => g.Images)
                .HasForeignKey<int>(s => s.HouseId)
                .WillCascadeOnDelete();
        }
    }
}