﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mouse_lighting.Models;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.db;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.Services.LightingHandlers;
using mouse_lighting.Services.Observer;
using mouse_lighting.Services.UserDialog;
using System.Diagnostics;

namespace mouse_lighting.Services
{
    internal static class ServicesRegistrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddSingleton<Setting>();
            services.AddSingleton<PathsModel>();

            services.AddTransient<IDataService, DataService.DataService>();
            services.AddTransient<IUserDialog, UserDialogService>();

            services.AddSingleton<IObserverStatusBar, ObserverStatusBar>();
            services.AddSingleton<IDataTransferBetweenViews, DataTransferBetweenViews>();
            services.AddSingleton<IExportService, ExportService>();
            services.AddSingleton<ILightingConvert, LightingConvert>();
            services.AddSingleton<ICyclesModels, CyclesModels>();


            var type = Configuration["TypeDB"];
            var connectionString = Configuration[$"DataBase:ConnectionStrings:{type}"];
            Debug.WriteLine(type);
            Debug.WriteLine(connectionString);
            Debug.WriteLine(Configuration.GetConnectionString(type));
            services.AddDbContext<ApplContextSqLite>(opt =>
            {
                switch (type)
                {
                    case null: throw new InvalidOperationException("DataBase TypeDb is Null.");
                    default: throw new InvalidOperationException($"{type} not implement");

                    case "SQLite":
                      // opt.UseSqlite(Configuration.GetConnectionString(type));
                        break;
                }

            }, ServiceLifetime.Transient);

            return services;
        }
    }
}
