using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public double DisplayTime { get; set; } = 10;
        public int Step { get; set; } = 0;
        public Action LightingType { get; set; } 
    }
}
