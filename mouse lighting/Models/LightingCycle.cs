using mouse_lighting.Services.LightingHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;


namespace mouse_lighting.Models
{

    class Lighting
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public ObservableCollection<LightingCycle> Cycles { get; set; } = new ObservableCollection<LightingCycle>();
    }
    internal class LightingCycle
    {
        private static int id = 0;
        public LightingCycle()
        {
            Id = id++;
        }
        public int Id { get; private set; }
        public Color ColorWheelStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorWheelEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public float DisplayTime { get; set; } = 1;
        public int Step { get; set; } = 32;
        public LightingHandlersEnum Handler { get; set; } = LightingHandlersEnum.ShowHide;
    }
    public class FrameCycle
    {
        public List<string> ColorPicture { get; set; } = (new string[18]).Select(x => x = "000000").ToList();
        public double DisplayTime { get; set; } = 0.005;
    }
}
