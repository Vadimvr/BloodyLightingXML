﻿using Models;
using mouse_lighting.Services.Interfaces;
using System;

namespace mouse_lighting.Services
{
    internal class DataTransferBetweenViews : IDataTransferBetweenViews
    {
        public string Name { get; private set; }
        public Guid Guid { get; private set; }
        public int Id { get; private set; }


        public event Action UpdateSelectedLighting;

        public event Action<string> PrintInStatusEvent;

        public void Status(string message)
        {
            PrintInStatusEvent?.Invoke(message);
        }
        public void SetLighting(Lighting lighting)
        {
            if (lighting != null)
            {
                Name = lighting.Name;
                Guid = lighting.Guid;
                Id = lighting.Id;
            }
            else
            {
                Name = null;
                Guid = Guid.Empty;
                Id = -1;
            }
            UpdateSelectedLighting?.Invoke();
        }
    }
}
