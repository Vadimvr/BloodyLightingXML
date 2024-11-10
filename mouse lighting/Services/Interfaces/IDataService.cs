using mouse_lighting.Models;
using System.Collections.Generic;
using System;
using mouse_lighting.Services.db;

namespace mouse_lighting.Services.Interfaces
{
    internal interface IDataService
    {
        public void Save(Lighting lighting, List<FrameCycle> frames);

        ApplicationContextSqLite DB { get; }

    }
}
