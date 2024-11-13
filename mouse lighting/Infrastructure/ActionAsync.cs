using System.Threading.Tasks;

namespace mouse_lighting.Infrastructure
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}
