using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Academits.Karetskas.PhoneBook.BackgroundTasks;
using Academits.Karetskas.PhoneBook.BusinessLogic.DataConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.BusinessLogic.Handlers;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string dbConnectionString = builder.Configuration.GetConnectionString("PhoneBookConnection") ?? "";
            builder.Services.AddDbContext<PhoneBookDbContext>(options =>
            {
                options.UseSqlServer(dbConnectionString)
                    .UseLazyLoadingProxies();
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IExcel, Excel>();
            builder.Services.AddHostedService<UploadContactsDaily>(provider =>
            {
                var path = builder.Configuration.GetValue<string>("BackgroundTasks:UploadContactsDaily");
                var excel = provider.GetRequiredService<IExcel>();

                return new UploadContactsDaily(path, excel, provider);
            });
            builder.Services.AddTransient<DbInitializer>();
            builder.Services.AddTransient<GetContactsHandler>();
            builder.Services.AddTransient<AddContactHandler>();
            builder.Services.AddTransient<DeleteContactsHandler>();
            builder.Services.AddTransient<GetContactsCountHandler>();
            builder.Services.AddTransient<DownloadExcelFileHandler>();
            builder.Services.AddTransient<IContactRepository, ContactRepository>();
            builder.Services.AddTransient<IPhoneNumberRepository, PhoneNumberRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWorkPhoneBook>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbInitializer = services.GetRequiredService<DbInitializer>();
                    dbInitializer.Initialize();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");

                    throw;
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}