using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;

namespace Academits.Karetskas.PhoneBook
{
    public class DbInitializer
    {
        private readonly PhoneBookDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DbInitializer(PhoneBookDbContext context, IUnitOfWork unitOfWork)
        {
            CheckArgument(context);
            CheckArgument(unitOfWork);

            _context = context;
            _unitOfWork = unitOfWork;
        }

        private void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument \"{nameof(obj)}\" is null.");
            }
        }

        public void Initialize()
        {
            _context.Database.Migrate();

            if (_unitOfWork.GetRepository<IContactRepository>()!.ContainAnyElements())
            {
                return;
            }

            try
            {
                _unitOfWork.BeginTransaction();

                var ivanovII = new Contact
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
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
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new()
                        {
                            Phone = "+79080989899",
                            PhoneType = PhoneNumberType.Mobile
                        }
                    }
                };

                _unitOfWork.GetRepository<IContactRepository>()!.AddRange(ivanovII, PetrovPP, SidorovSS, OgurcovOO, PetrosyanPP, KirkorovKK);

                _unitOfWork.Save();

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
            }
        }
    }
}
