using System.Collections.Generic;
using System.Windows.Automation;
using Models;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.Interfaces
{
    internal interface IDataService
    {
        public void SaveToXML(Lighting lighting, List<FrameCycle> frames);
        public void SaveSetting();
        ApplicationContextSqLite DB { get; }
        Setting Setting { get; }

    }
}
