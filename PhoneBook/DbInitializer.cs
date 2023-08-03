using System;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.PhoneBook
{
    public class DbInitializer
    {
        private readonly PhoneBookDbContext _context;

        public DbInitializer(PhoneBookDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), $"The argument \"{nameof(context)}\" is null.");
        }

        public void Initialize()
        {
            _context.Database.Migrate();

            // TODO: Добавить какие-то данные

            var ivanovII = new Contact
            {
                FirstName = "Иван",
                LastName = "Иванов",
                SecondName = "Иванович"
            };
            
            var PetrovPP = new Contact
            {
                FirstName = "Петр",
                LastName = "Петров",
                SecondName = "Петрович"
            };
            
            var SidorovSS = new Contact
            {
                FirstName = "Сидор",
                LastName = "Сидоров",
                SecondName = "Сидорович"
            };

            _context.Contacts.AddRange(ivanovII, PetrovPP, SidorovSS);

            _context.SaveChanges();
        }
    }
}
