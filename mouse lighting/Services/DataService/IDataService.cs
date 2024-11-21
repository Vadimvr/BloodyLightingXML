using Models;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.DataService
{
    internal interface IDataService
    {
        public void SaveToXML(Lighting lighting, List<FrameCycle> frames);
        public void SaveToXML(Lighting lighting, List<FrameCycle> frames, string path);
        public void SaveSetting();
        ApplicationContextSqLite DB { get; }
        Setting Setting { get; }
    }
}
