using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mouse_lighting.Services.db
{
    public class ApplicationContextSqLite : DbContext
    {
        string endFolder;
        public ApplicationContextSqLite(string PathToDb)
        {
            if (string.IsNullOrEmpty(PathToDb))
            {
                throw new ArgumentNullException(nameof(PathToDb));
            }
            else
            {
                endFolder = PathToDb;
            }
            Database.EnsureCreated();
        }
        internal DbSet<Lighting> Lighting { get; set; }
        internal DbSet<LightingCycle> LightingCycles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!Directory.Exists(endFolder))
            {
                Directory.CreateDirectory(endFolder);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite($"Data Source={endFolder}\\SLED.db");

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Color>().HaveConversion<ColorEFConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lighting>().Navigation(e => e.Cycles).AutoInclude();

        }
    }
    public class ColorEFConverter : ValueConverter<Color, string>
    {
        public ColorEFConverter() : base(v => v.ToString(), v => (Color)ColorConverter.ConvertFromString(v))
        {
        }
    }
    internal interface IAppContext
    {
        public List<Lighting> Lighting { get; }
        public void Update();

        public void Save();
    }
}
