using mouse_lighting.Models;
using System.Collections.Generic;
using System;

namespace mouse_lighting.Services.Interfaces
{
    internal interface IDataService
    {
        public void Save(Lighting lighting, List<FrameCycle> frames);

    }
}
