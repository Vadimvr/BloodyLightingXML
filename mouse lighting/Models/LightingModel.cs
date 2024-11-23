using Models;
using mouse_lighting.Services.DataService;

namespace mouse_lighting.Models
{
    class LightingModel
    {
        private Lighting? selectedLighting = default!;

        public List<Lighting> Lighting { get; set; } = default!;

        public Lighting? SelectedLighting
        {
            get => selectedLighting; set
            {
                selectedLighting = value;
                UpdateSelectedLightingEvent?.Invoke();
            }
        }
        public IDataService _DataService { get; }

        internal LightingModel(IDataService dataService, Action updateLighting)
        {
            UpdateLightingEvent += updateLighting;
            _DataService = dataService;
            _DataService.UpdatePathDbEvent += UpdateDbPath;
            Lighting = _DataService.DB.Lighting.ToList();
        }

        private void UpdateDbPath()
        {
            SelectedLighting = null;
            Lighting = _DataService.DB.Lighting.ToList();
            UpdateLightingEvent?.Invoke();
        }

        event Action? UpdateSelectedLightingEvent;
        event Action? UpdateLightingEvent;

        internal Lighting AddNew()
        {
            var defNewName = "New animation";
            var newName = defNewName;
            var j = 0;
            for (int i = 0; i < Lighting.Count; i++)
            {
                if (Lighting[i].Name == newName)
                {
                    i = 0;
                    newName = $"{defNewName}_{++j}";
                }
            }

            Lighting x = _DataService.DB.Lighting.Add(new Lighting() { Cycles = new List<LightingCycle>(), Guid = Guid.NewGuid(), Name = newName }).Entity;
            UpdateData();
            SelectedLighting = x;
            return x;
        }

        internal void Remove(Lighting lighting)
        {
            if (lighting == null) throw new ArgumentNullException(nameof(lighting));
            if (lighting.Id <= 0) throw new ArgumentNullException(nameof(lighting.Id));
            _DataService.DB.Lighting.Remove(lighting);
            RemoveFile(lighting);
            UpdateData();
        }

        private void RemoveFile(Lighting lighting)
        {
            //TODO:  throw new NotImplementedException();
        }

        private void UpdateData()
        {
            _DataService.DB.SaveChanges();
            Lighting = _DataService.DB.Lighting.ToList();
            UpdateLightingEvent?.Invoke();
        }
    }
}
