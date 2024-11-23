using Microsoft.Extensions.DependencyInjection;
using mouse_lighting.Models;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.DataService
{
    internal class DataService : IDataService
    {
        private ApplContextSqLite _appContext;

        public event Action? UpdatePathDbEvent;

        public ApplContextSqLite DB => _appContext;
        public Setting Setting { get; set; }

        public DataService(Setting setting, ApplContextSqLite context)
        {
            Setting = setting;
            setting.UpdateDbPathEvent += UpdatePathDb;
            _appContext = context;
        }

        private void UpdatePathDb()
        {
           _appContext =  App.Services.GetRequiredService<ApplContextSqLite>();
            UpdatePathDbEvent?.Invoke();
        }
    }
}


