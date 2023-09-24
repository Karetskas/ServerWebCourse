using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.PhoneBook.DataAccess
{
    public class PhoneBookDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;

        public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;

        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(builder =>
            {
                builder.Property(contact => contact.LastName)
                    .HasMaxLength(50);

                builder.Property(contact => contact.FirstName)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PhoneNumber>(builder =>
            {
                builder.Property(phoneNumber => phoneNumber.Phone)
                    .HasMaxLength(50);

                builder.HasOne(phoneNumber => phoneNumber.Contact)
                    .WithMany(contact => contact.PhoneNumbers)
                    .HasForeignKey(phoneNumber => phoneNumber.ContactId);
            });
        }
    }
}
