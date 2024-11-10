﻿using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Models;
using mouse_lighting.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using mouse_lighting.Services.Interfaces;
using System.Windows.Media;
using System.Linq;

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
            mouse_lighting.Models.Lighting.NameChanged += LightingNameChanged;

            SetDataFromDb();
        }

        private void LightingNameChanged(string name, string value, int id)
        {
            var lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Id == id);
            if(lighting != null)
            {
                lighting.Name = name;
                _DataService.DB.Lighting.Update(lighting);
                _DataService.DB.SaveChanges();
            }
        }

        private ObservableCollection<Color> _Colors;
        public ObservableCollection<Color> Colors { get => _Colors; set => Set(ref _Colors, value); }

        private ObservableCollection<Lighting> _Lighting;
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

            _DataService.DB.Lighting.Add(new Lighting() { Cycles = new ObservableCollection<LightingCycle>(), Guid = Guid.NewGuid(), Name = newName });
            _DataService.DB.SaveChanges();
            SetDataFromDb();
        }
        #endregion

        private void SetDataFromDb()
        {
            var x = _DataService.DB.Lighting;
            foreach (var cycles in x)
            {
                foreach (var cycle in cycles.Cycles)
                {
                    cycle.SetColorFromString();
                }
            }
            Lighting = new ObservableCollection<Lighting>(x);
        }
    }
}
