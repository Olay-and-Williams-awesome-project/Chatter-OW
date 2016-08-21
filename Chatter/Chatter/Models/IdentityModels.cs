using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Chatter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string DisplayTitle { get; set; }
        public string Description { get; set; }
        public string ProfileImage { get; set; }
        public string JoinedDate { get; set; }
        public string BirthDate { get; set; }
        public virtual ICollection<Chit> Chits { get; set; }

        public virtual ICollection<ApplicationUser> Followers { get; set; }
        public virtual ICollection<ApplicationUser> Following { get; set; }

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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
               .HasMany(x => x.Followers).WithMany(x => x.Following)
               .Map(x => x.ToTable("Followers")
               .MapLeftKey("UserId")
               .MapRightKey("FollowerId"));

            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.DbSet<Chatter.Models.Chit> Chits { get; set; }
    }
}