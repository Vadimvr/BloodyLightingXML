using Models;

namespace mouse_lighting.Services.DataService.Repository
{
    internal class AppDataBase
    {
        public AppDataBase(IRepository<Lighting> lighting, IRepository<LightingCycle> cycle)
        {
            Lighting = lighting;
            Cycle = cycle;
        }

        public IRepository<Lighting> Lighting { get; }
        public IRepository<LightingCycle> Cycle { get; }
    }
}
