using Models;
using System.Collections.ObjectModel;

namespace mouse_lighting.Models
{
    internal interface ICyclesModels
    {
        List<LightingCycle> Cycles { get; }
        Lighting? Lighting { get; set; }

        event Action? UpdateCyclesEvent;

        void AddNew();
        void Delete(object? p);
        void Down(object? p);
        void Export();
        Task SaveInDb();
        void Up(object? p);
        void UpdateCycles(int id);
    }
}