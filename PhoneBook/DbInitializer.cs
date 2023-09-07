using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;

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

            if (!_context.Contacts.Any())
            {
                var ivanovII = new Contact
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    SecondName = "Ivanovich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "+79054345433",
                            PhoneType = PhoneNumberType.Mobile
                        },

                        new()
                        {
                            Phone = "2-22-22",
                            PhoneType = PhoneNumberType.Home
                        }
                    }
                };
            
                var PetrovPP = new Contact
                {
                    FirstName = "Petr",
                    LastName = "Petrov",
                    SecondName = "Petrovich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "+79053453454",
                            PhoneType = PhoneNumberType.Work
                        }
                    }
                };
            
                var SidorovSS = new Contact
                {
                    FirstName = "Sidor",
                    LastName = "Sidorov",
                    SecondName = "Sidorovich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "89080989899",
                            PhoneType = PhoneNumberType.Mobile
                        }
                    }
                };

                var OgurcovOO = new Contact
                {
                    FirstName = "Oleg",
                    LastName = "Ogurcov",
                    SecondName = "Olegovich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "89083289823",
                            PhoneType = PhoneNumberType.Mobile
                        },

                        new()
                        {
                            Phone = "3-45-33",
                            PhoneType = PhoneNumberType.Home
                        },

                        new()
                        {
                            Phone = "4-44-44",
                            PhoneType = PhoneNumberType.Work
                        }
                    }
                };

                var PetrosyanPP = new Contact
                {
                    FirstName = "Petr",
                    LastName = "Petrosyan",
                    SecondName = "Petorvich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "2-45-73",
                            PhoneType = PhoneNumberType.Home
                        }
                    }
                };

                var KirkorovKK = new Contact
                {
                    FirstName = "Kiril",
                    LastName = "Kirkorov",
                    SecondName = "Kirilovich",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "+79080989899",
                            PhoneType = PhoneNumberType.Mobile
                        }
                    }
                };

                _context.Contacts.AddRange(ivanovII, PetrovPP, SidorovSS, OgurcovOO, PetrosyanPP, KirkorovKK);

                _context.SaveChanges();
            }
        }
    }
}
