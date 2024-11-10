using mouse_lighting.Services.LightingHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Media;

namespace mouse_lighting.Models
{
    class Lighting
    {
        public static Action<string, string, int> NameChanged;
        private string name;
        public int Id { get; set; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                if (Id > 0 && !name.Equals(value))
                {
                    NameChanged?.Invoke(name, value, Id);
                }
            }
        }
        public Guid Guid { get; set; }
        public ObservableCollection<LightingCycle> Cycles { get; set; } = new ObservableCollection<LightingCycle>();
    }
    internal class LightingCycle
    {
        public int Id { get; set; }
        public int IndexNumber { get; set; }
        public int LightingId { get; set; }
        //public string ColorWheelStartString { get; set; } = "#000000";
        //public string ColorWheelEndString { get; set; } = "#000000";
        //public string ColorSecondStartString { get; set; } = "#000000";
        //public string ColorSecondEndString { get; set; } = "#000000";

        public Color ColorWheelStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorWheelEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondStart { get; set; } = Color.FromRgb(0, 0, 0);
        public Color ColorSecondEnd { get; set; } = Color.FromRgb(0, 0, 0);
        public float DisplayTime { get; set; } = 1;
        public int Step { get; set; } = 32;
        public LightingHandlersEnum Handler { get; set; } = LightingHandlersEnum.ShowHide;

        public void SetColorFromString()
        {

            //ColorWheelStart = (Color)ColorConverter.ConvertFromString(ColorWheelStartString);
            //ColorWheelEnd = (Color)ColorConverter.ConvertFromString(ColorWheelEndString);
            //ColorSecondStart = (Color)ColorConverter.ConvertFromString(ColorSecondStartString);
            //ColorSecondEnd = (Color)ColorConverter.ConvertFromString(ColorSecondEndString);
        }
        public void SetStringFromColor()
        {
        }

    }
    public class FrameCycle
    {
        public List<string> ColorPicture { get; set; } = (new string[18]).Select(x => x = "000000").ToList();
        public double DisplayTime { get; set; } = 0.005;
    }
}
