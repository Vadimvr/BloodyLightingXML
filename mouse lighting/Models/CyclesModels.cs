using Microsoft.EntityFrameworkCore;
using Models;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.DataService.Repository;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace mouse_lighting.Models
{
    internal class CyclesModels : ICyclesModels
    {
        private readonly IExportService ExportService;
        private readonly IRepository<Lighting> Lightings;
        private readonly IRepository<LightingCycle> LightingCycles;
        private readonly IDataService _DataService;


        public Lighting? Lighting { get; set; }
        public List<LightingCycle> Cycles { get; private set; } = new List<LightingCycle>();
        private List<LightingCycle> removedCycles = new List<LightingCycle>();
        private object lock_OBJECT = new object();

        public event Action? UpdateCyclesEvent;
        public CyclesModels(IDataService dataService, IExportService exportService, IRepository<Lighting> Lightings, IRepository<LightingCycle> LightingCycles)
        {
            _DataService = dataService;
            ExportService = exportService;
            this.Lightings = Lightings;
            this.LightingCycles = LightingCycles;
        }

        public async void UpdateCycles(int id)
        {
            removedCycles.Clear();
            Cycles.Clear();
            await Task.Run(async () =>
                  {
                      Monitor.Enter(lock_OBJECT);
                      try
                      {
                          if (id > 0)
                          {
                              Lighting = await Lightings.Items.SingleOrDefaultAsync(l => l.Id == id);
                              if (Lighting == null || Lighting.Cycles == null) { throw new ArgumentNullException(nameof(Lighting)); }

                              Cycles.AddRange(LightingCycles.Items.Where(x => x.LightingId == Lighting.Id));
                          }
                      }
                      finally
                      {
                          Monitor.Exit(lock_OBJECT);
                      }
                  });

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

        public async Task SaveInDb()
        {
            Monitor.Enter(lock_OBJECT);
            try
            {
                foreach (var item in Cycles)
                {
                    if (item.Id == 0)
                    {
                        await LightingCycles.AddAsync(item, false);
                    }
                    else
                    {
                        await LightingCycles.UpdateAsync(item.Id, item, false);
                    }
                }
                foreach (var item in removedCycles)
                {
                    if (item.Id > 0)
                    {
                        await LightingCycles.DeleteAsync(item.Id, false);
                    }
                }
                await LightingCycles.SaveAsync();
            }
            finally { Monitor.Exit(lock_OBJECT); }

        }
        public void Export()
        {
            if (Lighting != null)
            {
                var l = Lighting.CloneWithoutLightingCycles(Cycles);
                ExportService.ExportXml(l, _DataService.Setting.PathToXML);
            }
        }
    }
}

