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
        public List<LightingCycle> Cycles { get; private set; }

        public event Action? UpdateCyclesEvent;
        private List<LightingCycle> removedCycles;
        public CyclesModels(IDataService dataService, IExportService exportService)
        {
            _DataService = dataService;
            Cycles = new List<LightingCycle>();
            ExportService = exportService;
            removedCycles = new List<LightingCycle>();
        }

        public void UpdateCycles(int id)
        {
            removedCycles = new List<LightingCycle>();
            Lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Id == id);
            
            Cycles.Clear();
            Cycles.AddRange(_DataService.DB.LightingCycles.Where(x => x.LightingId == id));
            
            UpdateCyclesEvent?.Invoke();
        }

        public void AddNew()
        {
            if (Lighting == null) throw new ArgumentNullException(nameof(Lighting));

            for (int i = 0; i < 1000; i++)
            {
                LightingCycle cycle = new LightingCycle() { LightingId = Lighting.Id, IndexNumber = Cycles.Count() };
                Cycles.Add(cycle);
            }
            UpdateCyclesEvent?.Invoke();
        }



        public void Delete(object? p)
        {
            if (p is not int) { throw new ArgumentNullException(nameof(p)); }

            var indexNumber = (int)p;

            var removedCycle = Cycles.FirstOrDefault(x => x.IndexNumber == indexNumber);

            if (removedCycle == null) { return; }
            removedCycles.Add(removedCycle);

            for (int i = 0; i < Cycles.Count; i++)
            {
                if (Cycles[i].IndexNumber > indexNumber)
                {
                    Cycles[i].IndexNumber -= 1;
                }
            }
            Cycles.Remove(removedCycle);
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
            foreach (var item in removedCycles)
            {
                if (item.Id > 0)
                {
                    _DataService.DB.Remove(item);
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

