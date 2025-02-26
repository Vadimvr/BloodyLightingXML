﻿using Models;

namespace mouse_lighting.Services.DataTransferBetweenView
{
    interface IDataTransferBetweenViews
    {
        public string Name { get; }
        public Guid Guid { get; }
        public int Id { get; }
        event Action UpdateSelectedLighting;
        event Action<string> PrintInStatusEvent;
        void SetLighting(Lighting lighting);
        public void Status(string message);

        void Update(int id);
        event Action<int> UpdateSelectedLightingEvent;
    }
}
