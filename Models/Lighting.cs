using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Models
{
    public class Lighting
    {
        public static Action<string, string, int> NameChanged;
        private string name;
        public int Id { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (Id > 0 && !name.Equals(value))
                {
                    name = value;
                    NameChanged?.Invoke(name, value, Id);
                }
                else
                {
                    name = value;
                }
            }
        }
        public Guid Guid { get; set; }
        public List<LightingCycle> Cycles { get; set; } = new List<LightingCycle>();
    }

    public class LightingCycle
    {
        public int Id { get; set; }
        public int IndexNumber { get; set; }
        public int LightingId { get; set; }
        public Color ColorWheelStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorWheelEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public float DisplayTime { get; set; } = 1;
        public int Step { get; set; } = 32;

        public LightingCycle Clone()
        {
            return new LightingCycle()
            {
                Id = Id,
                IndexNumber = IndexNumber,
                LightingId = LightingId,
                ColorWheelStart = ColorWheelStart,
                ColorWheelEnd = ColorWheelEnd,
                ColorSecondStart = ColorSecondStart,
                ColorSecondEnd = ColorSecondEnd,
                DisplayTime = DisplayTime,
                Step = Step
            };
        }
        //public LightingHandlersEnum Handler { get; set; } = LightingHandlersEnum.ShowHide;
    }

    public class FrameCycle
    {
        public List<string> ColorPicture { get; set; } = (new string[18]).Select(x => x = "000000").ToList();
        public double DisplayTime { get; set; } = 0.005;
    }
}
