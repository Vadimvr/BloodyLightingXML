using Models;

namespace mouse_lighting.Services.DataTransferBetweenView
{
    internal class DataTransferBetweenViews : IDataTransferBetweenViews
    {
        public string Name { get; private set; } = default!;
        public Guid Guid { get; private set; }
        public int Id { get; private set; }


        public event Action? UpdateSelectedLighting;

        public event Action<string>? PrintInStatusEvent;
        public event Action<int>? UpdateSelectedLightingEvent;

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
                Name = default!;
                Guid = Guid.Empty;
                Id = -1;
            }
            UpdateSelectedLighting?.Invoke();
        }

        public void Update(int id)
        {
            Id = id;
            UpdateSelectedLightingEvent?.Invoke(id);
        }
    }
}
