using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Models;
using mouse_lighting.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using mouse_lighting.Services.Interfaces;
using System.Windows.Media;

namespace mouse_lighting.ViewModels
{
    internal class LightingViewModel : ViewModel
    {
        private LambdaCommand _DownCycleCommand;
        private MainWindowViewModel _MainWindowViewModel;
        private LightingCycleViewMode _LightingCycleViewMode;
        private IUserDialog _UserDialog;
        private IDataService _DataService;

        public LightingViewModel(IUserDialog UserDialog, IDataService DataService, MainWindowViewModel mainWindowViewModel, LightingCycleViewMode LightingCycleViewMode)
        {
            LightingCycleViewMode._LightingViewModel = this;
            _MainWindowViewModel = mainWindowViewModel;
            _LightingCycleViewMode = LightingCycleViewMode;
            _UserDialog = UserDialog;
            _DataService = DataService;
        }

        private ObservableCollection<Color> _Colors;
        public ObservableCollection<Color> Colors { get => _Colors; set => Set(ref _Colors, value); }

        private ObservableCollection<Lighting> _Lighting = new ObservableCollection<Lighting>() {
        new Lighting(){Name = "Name 1", Guid = Guid.Parse("4ab3fe39-9546-455b-86b4-18cc165e3d9f"), Cycles = new ObservableCollection<LightingCycle>(){
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,1,1), ColorSecondStart = Color.FromRgb(255,255,1), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,1,2), ColorSecondStart = Color.FromRgb(255,255,2), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,1,3), ColorSecondStart = Color.FromRgb(255,255,3), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,1,4), ColorSecondStart = Color.FromRgb(255,255,4), DisplayTime =5, Step = 20},
        } },
         new Lighting(){Name = "Name 2", Guid = Guid.Parse("b1095166-e706-47df-bc1a-f246af930e5b"), Cycles = new ObservableCollection<LightingCycle>(){
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,2,1), ColorSecondStart = Color.FromRgb(255,255,1), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,2,2), ColorSecondStart = Color.FromRgb(255,255,2), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,2,3), ColorSecondStart = Color.FromRgb(255,255,3), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,2,4), ColorSecondStart = Color.FromRgb(255,255,4), DisplayTime =5, Step = 20},
        } },
          new Lighting(){Name = "Name 3", Guid = Guid.Parse("ea8f2706-0959-4c99-8be9-8c929ff4b57c"), Cycles = new ObservableCollection<LightingCycle>(){
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,3,1), ColorSecondStart = Color.FromRgb(255,255,1), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,3,2), ColorSecondStart = Color.FromRgb(255,255,2), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,3,3), ColorSecondStart = Color.FromRgb(255,255,3), DisplayTime =5, Step = 20},
            new LightingCycle(){ColorWheelStart = Color.FromRgb(255,3,4), ColorSecondStart = Color.FromRgb(255,255,4), DisplayTime =5, Step = 20},
        } },
        };

        public ObservableCollection<Lighting> Lighting { get => _Lighting; set => Set(ref _Lighting, value); }

        private Lighting _SelectedLighting;
        public Lighting SelectedLighting
        {
            get { return _SelectedLighting; }
            set
            {
                if (Set(ref _SelectedLighting, value))
                {
                    _LightingCycleViewMode.Cycles = SelectedLighting.Cycles;
                }
            }
        }

        #region AddNewLightingCommand
        private LambdaCommand _AddNewLightingCommand;
        public ICommand AddNewLightingCommand => _AddNewLightingCommand ??=
            new LambdaCommand(OnAddNewLightingCommandExecuted, CanAddNewLightingCommandExecute);
        private bool CanAddNewLightingCommandExecute(object p) => true;
        private void OnAddNewLightingCommandExecuted(object p)
        {
            if (Lighting == null)
            {
                Lighting = new ObservableCollection<Lighting>();
            }
            var defNewName = "New animation";
            var newName = defNewName;
            var j = 0;
            for (int i = 0; i < Lighting.Count; i++)
            {
                if (Lighting[i].Name == newName)
                {
                    i = 0;
                    j++;
                    newName = $"{defNewName}_{j}";
                }
            }
            Lighting.Add(new Lighting() { Cycles = new ObservableCollection<LightingCycle>(), Guid = Guid.NewGuid(), Name = newName });
        }
        #endregion
    }
}
