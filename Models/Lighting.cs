using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows.Media;

namespace Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        public abstract void Update(Entity entityNew);
    }

    public class Lighting : Entity, IDisposable
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public List<LightingCycle> Cycles { get; set; } = new List<LightingCycle>();

        public override void Update(Entity entityNew)
        {
            var x = entityNew as Lighting;
            if (x is null) { throw new ArgumentNullException(nameof(x)); }
            this.Name = x.Name;
        }
        public Lighting CloneWithoutLightingCycles(List<LightingCycle> cycles)
        {
            return new Lighting() { Id = this.Id, Guid = this.Guid, Name = this.Name, Cycles = cycles };
        }

        public void Dispose()
        {
            Cycles.Clear();

        }
    }

    public class LightingCycle : Entity
    {
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

        public override void Update(Entity entityNew)
        {
            var x = entityNew as LightingCycle;
            if (x is null) { throw new ArgumentNullException(nameof(x)); }
            this.IndexNumber = x.IndexNumber;
            this.LightingId = x.LightingId;
            this.ColorWheelStart = x.ColorWheelStart;
            this.ColorWheelEnd = x.ColorWheelEnd;
            this.ColorSecondStart = x.ColorSecondStart;
            this.ColorSecondEnd = x.ColorSecondEnd;
            this.DisplayTime = x.DisplayTime;
        }
    }

    public class FrameCycle
    {
        public List<string> ColorPicture { get; set; } = (new string[18]).Select(x => x = "000000").ToList();
        public double DisplayTime { get; set; } = 0.005;
    }
}
