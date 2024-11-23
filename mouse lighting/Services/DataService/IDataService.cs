using Models;
using mouse_lighting.Models;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.DataService
{
    internal interface IDataService
    {
        ApplContextSqLite DB { get; }
        Setting Setting { get; }
        event Action UpdatePathDbEvent;

    }
}
