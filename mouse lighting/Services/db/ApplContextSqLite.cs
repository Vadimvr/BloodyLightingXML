using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using mouse_lighting.Models;
using System.Windows.Media;

namespace mouse_lighting.Services.db
{
    public class ApplContextSqLite : DbContext
    {
        readonly Setting _Setting;
        public ApplContextSqLite(DbContextOptions<ApplContextSqLite> options, Setting setting) : base(options)
        {
            _Setting = setting;
            _Setting.UpdateDbPathEvent += UpUpdateDbPath;
            Database.EnsureCreated();
        }

        private void UpUpdateDbPath()
        {
            Database.CloseConnection();
        }

        internal DbSet<Lighting> Lighting { get; set; } = default!;
        internal DbSet<LightingCycle> LightingCycles { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_Setting.PathToDb)) { throw new ArgumentNullException(nameof(Setting.PathToDb)); }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite($"Data Source={_Setting.PathToDb}");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Color>().HaveConversion<ColorEFConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         //   modelBuilder.Entity<Lighting>().Navigation(e => e.Cycles).AutoInclude();
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
