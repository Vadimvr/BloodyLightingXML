using Models;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.DataService.Repository;
using mouse_lighting.Services.Observer;
using mouse_lighting.Services.UserDialog;
using System.Diagnostics;

namespace mouse_lighting.Models
{
    internal class LightingModel
    {
        private Lighting? selectedLighting = default!;
        private readonly IRepository<Lighting> _Lightings;

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
        private readonly IObserverStatusBar _StatusBar;

        public LightingModel(IDataService dataService, IRepository<Lighting> Lightings, IObserverStatusBar StatusBar)
        {
            //   UpdateLightingEvent += updateLighting;

            _DataService = dataService;
            _DataService.UpdatePathDbEvent += UpdateDbPath;
            _Lightings = Lightings;
            _StatusBar = StatusBar;
            Lighting = _Lightings.Items.ToList();
        }

        private void UpdateDbPath()
        {
            SelectedLighting = null;
            Lighting = _Lightings.Items.ToList();
            UpdateLightingEvent?.Invoke();
        }

        event Action? UpdateSelectedLightingEvent;
        public event Action? UpdateLightingEvent;

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

            Lighting x = _Lightings.Add(new Lighting() { Cycles = new List<LightingCycle>(), Guid = Guid.NewGuid(), Name = newName });
            UpdateData();
            SelectedLighting = x;
            return x;
        }

        internal void Remove(Lighting lighting)
        {
            if (lighting == null) throw new ArgumentNullException(nameof(lighting));
            if (lighting.Id <= 0) throw new ArgumentNullException(nameof(lighting.Id));
            _Lightings.Delete(lighting.Id);
            RemoveFile(lighting);
            UpdateData();
        }

        private void RemoveFile(Lighting lighting)
        {
            //TODO:   throw new NotImplementedException();
        }

        private void UpdateData()
        {
            _Lightings.Save();
            Lighting = _Lightings.Items.ToList();
            UpdateLightingEvent?.Invoke();
        }

        internal Task UpdateNameAsync(Lighting selectedLighting) => _Lightings.UpdateAsync(selectedLighting.Id, selectedLighting);
    }
}
