using Models;
using mouse_lighting.Services.DataService;
using System.Collections.ObjectModel;
namespace mouse_lighting.Models
{
    internal class CyclesModels : ICyclesModels
    {
        private readonly IExportService ExportService;
        private readonly IDataService _DataService;
        public Lighting? Lighting { get; set; }
        public ObservableCollection<LightingCycle> Cycles { get; private set; }

        public event Action? UpdateCyclesEvent;

        public CyclesModels(IDataService dataService, IExportService exportService)
        {
            _DataService = dataService;
            Cycles = new ObservableCollection<LightingCycle>();
            ExportService = exportService;
        }

        public void UpdateCycles(int id)
        {

            Lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Id == id);
            Cycles.Clear();
            if (Lighting != null)
            {
                foreach (var item in Lighting.Cycles.OrderBy(c => c.IndexNumber))
                {
                    Cycles.Add(item);
                }
            }
            UpdateCyclesEvent?.Invoke();
        }

        public void AddNew()
        {
            if (Lighting == null) throw new ArgumentNullException(nameof(Lighting));

            LightingCycle cycle = new LightingCycle() { LightingId = Lighting.Id, IndexNumber = Cycles.Count() };
            cycle = _DataService.DB.Add(cycle).Entity;
            Cycles.Add(cycle);
            UpdateCyclesEvent?.Invoke();
        }

        public void Delete(object? p)
        {
            if (p is not int) { throw new ArgumentNullException(nameof(p)); }

            var indexNumber = (int)p;
            for (int i = indexNumber + 1; i < Cycles.Count; i++)
            {
                Cycles[i].IndexNumber -= 1;
                _DataService.DB.Update(Cycles[i]);
            }

            _DataService.DB.Remove(Cycles[indexNumber]);
            Cycles.RemoveAt(indexNumber);
            UpdateCyclesEvent?.Invoke();
        }

        public void Up(object? p)
        {
            if (p is not int) { throw new ArgumentNullException(nameof(p)); }
            var indexNumber = (int)p;
            if (indexNumber == 0) { return; }
            Down(indexNumber - 1);
        }

        public void Down(object? p)
        {
            if (p is not int) { throw new ArgumentNullException(nameof(p)); }
            var indexNumber = (int)p;
            if (indexNumber >= Cycles.Count) { return; }

            Down(indexNumber);
        }

        private void Down(int indexNumber)
        {
            Cycles[indexNumber].IndexNumber += 1;
            Cycles[indexNumber + 1].IndexNumber -= 1;
            var x = Cycles[indexNumber];
            Cycles[indexNumber] = Cycles[indexNumber + 1];
            Cycles[indexNumber + 1] = x;
            _DataService.DB.Update(Cycles[indexNumber]);
            _DataService.DB.Update(Cycles[indexNumber + 1]);
            UpdateCyclesEvent?.Invoke();
        }

        public void SaveInDb()
        {
            foreach (var item in Cycles)
            {
                if (item.Id == 0)
                {
                    _DataService.DB.Add(item);
                }
                else
                {
                    _DataService.DB.Update(item);
                }
            }
            _DataService.DB.SaveChanges();
        }

        public void Export()
        {
            if (Lighting != null) { ExportService.ExportXml(Lighting, _DataService.Setting.PathToXML); }
        }
    }
}

