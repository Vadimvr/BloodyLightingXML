using Models;
using mouse_lighting.Services.DataService.Repository;

namespace mouse_lighting.Services.db
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
