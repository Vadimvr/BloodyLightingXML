using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Models;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using mouse_lighting.Services.LightingHandlers;
using System.Collections.Generic;

namespace mouse_lighting.ViewModels
{
    internal class LightingCycleViewMode : ViewModel
    {
        public LightingViewModel _LightingViewModel;
        private MainWindowViewModel _MainWindowViewModel;
        private IUserDialog _UserDialog;
        private IDataService _DataService;


        private int _Name = 148;
        public int Name { get => _Name; set => Set(ref _Name, value); }

        public LightingCycleViewMode(
            IUserDialog UserDialog,
            IDataService DataService,
            MainWindowViewModel MainWindowViewModel
            )
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _MainWindowViewModel = MainWindowViewModel;
        }

        private ObservableCollection<LightingCycle> _Cycles;
        public ObservableCollection<LightingCycle> Cycles { get => _Cycles; set => Set(ref _Cycles, value); }

        private int _IndexLightingCycle;
        public int IndexLightingCycle
        {
            get => _IndexLightingCycle;
            set
            {
                if (Set(ref _IndexLightingCycle, value))
                {
                    _MainWindowViewModel.Status = value.ToString();
                }
            }
        }

        #region AddNewCycleCommand - описание команды 
        private LambdaCommand _AddNewCycleCommand;
        public ICommand AddNewCycleCommand => _AddNewCycleCommand ??=
            new LambdaCommand(OnAddNewCycleCommandExecuted, CanAddNewCycleCommandExecute);
        private bool CanAddNewCycleCommandExecute(object p) => Cycles != null;
        private void OnAddNewCycleCommandExecuted(object p)
        {
            Cycles.Add(new());
        }
        #endregion

        #region RemoveCycleCommand - описание команды 
        private LambdaCommand _RemoveCycleCommand;
        public ICommand RemoveCycleCommand => _RemoveCycleCommand ??=
            new LambdaCommand(OnRemoveCycleCommandExecuted, CanRemoveCycleCommandExecute);
        private bool CanRemoveCycleCommandExecute(object p) => true;
        private void OnRemoveCycleCommandExecuted(object p)
        {
            int id = (int)p;
            var cycle = Cycles.FirstOrDefault(x => x.Id == id);
            if (cycle == null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }
            Cycles.Remove(cycle);

        }
        #endregion

        #region ExportToXmlLightingFileCommand - описание команды 
        private LambdaCommand _ExportToXmlLightingFileCommand;
        public ICommand ExportToXmlLightingFileCommand => _ExportToXmlLightingFileCommand ??=
            new LambdaCommand(OnExportToXmlLightingFileCommandExecuted, CanExportToXmlLightingFileCommandExecute);
        private bool CanExportToXmlLightingFileCommandExecute(object p) => _LightingViewModel.SelectedLighting != null;
        LightingHandlerCreator LightingHandlerCreator = new LightingHandlerCreator();
        private void OnExportToXmlLightingFileCommandExecuted(object p)
        {
            if (_LightingViewModel.SelectedLighting == null) { throw new ArgumentNullException(nameof(_LightingViewModel.SelectedLighting)); }
            List<FrameCycle> frames = LightingHandlerCreator.Worker(_LightingViewModel.SelectedLighting);
            _DataService.Save(_LightingViewModel.SelectedLighting, frames);
            _MainWindowViewModel.Status = "TODO export in xml";
        }
        #endregion

        #region UpCycleCommand - описание команды 
        private LambdaCommand _UpCycleCommand;
        public ICommand UpCycleCommand => _UpCycleCommand ??=
            new LambdaCommand(OnUpCycleCommandExecuted, CanUpCycleCommandExecute);
        private bool CanUpCycleCommandExecute(object p) => true;
        private void OnUpCycleCommandExecuted(object p)
        {
            int id = (int)p;
            int i = 0;
            for (; i < Cycles.Count; i++)
            {
                if (Cycles[i].Id == id)
                {
                    break;
                }
            }

            if (i == Cycles.Count) { throw new ArgumentOutOfRangeException(nameof(Cycles)); }

            if (i == 0)
            {
                _MainWindowViewModel.Status = "First item";
                return;
            }

            var temp = Cycles[i - 1];
            Cycles.RemoveAt(i - 1);

            Cycles.Insert(i, temp);
            IndexLightingCycle = -1;
        }
        #endregion

        #region DownCyrcleCommand - описание команды 

        private LambdaCommand _DownCycleCommand;
        public ICommand DownCycleCommand => _DownCycleCommand ??=
            new LambdaCommand(OnDownCycleCommandExecuted, CanDownCycleCommandExecute);
        private bool CanDownCycleCommandExecute(object p) => true;
        private void OnDownCycleCommandExecuted(object p)
        {
            int id = (int)p;
            int i = 0;
            for (; i < Cycles.Count; i++)
            {
                if (Cycles[i].Id == id)
                {
                    break;
                }
            }

            if (i == Cycles.Count) { throw new ArgumentOutOfRangeException(nameof(Cycles)); }

            if (i == Cycles.Count - 1)
            {
                _MainWindowViewModel.Status = "last item";
                return;
            }

            var temp = Cycles[i];
            Cycles.RemoveAt(i);

            Cycles.Insert(i + 1, temp);
            IndexLightingCycle = -1;
        }
        #endregion
    }
}
