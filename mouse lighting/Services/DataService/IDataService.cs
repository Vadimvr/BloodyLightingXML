using Models;
using mouse_lighting.Models;

namespace mouse_lighting.Services.DataService
{
    internal interface IDataService
    {
      //  ApplContextSqLite DB { get; }
        Setting Setting { get; }
        event Action UpdatePathDbEvent;

    }
}
