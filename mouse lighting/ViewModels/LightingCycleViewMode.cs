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
            var temp = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
            if (temp != null)
            {
                var index = -1;
                foreach (var item in temp.Cycles)
                {
                    if (index < item.IndexNumber)
                    {
                        index = item.IndexNumber;
                    }
                }
                temp.Cycles.Add(new() { IndexNumber = ++index });
                _DataService.DB.SaveChanges();
                temp = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
                if (temp != null)
                {
                    Cycles = new ObservableCollection<LightingCycle>(temp.Cycles.OrderBy(c => c.IndexNumber));
                }

            }
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
            var temp = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
            if (temp != null)
            {
                var index = 0;
                var cycle = temp.Cycles.FirstOrDefault(x => x.Id == id);

                index = cycle.IndexNumber;

                _DataService.DB.Remove(cycle);
                foreach (var item in temp.Cycles)
                {
                    if (index < item.IndexNumber)
                    {
                        item.IndexNumber -= 1;
                        _DataService.DB.Update(item);
                    }
                }

                _DataService.DB.SaveChanges();
                temp = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
                if (temp != null)
                {
                    Cycles = new ObservableCollection<LightingCycle>(temp.Cycles.OrderBy(c => c.IndexNumber));
                }

            }

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

            var lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
            if (lighting != null)
            {
                var cycle_0 = lighting.Cycles.FirstOrDefault(x => x.Id == id);
                if (cycle_0.IndexNumber == 0) { return; }

                int index_0 = cycle_0.IndexNumber;
                int index_1 = index_0 - 1;

                var cycle_1 = lighting.Cycles.FirstOrDefault(x => x.IndexNumber == index_1);

                cycle_0.IndexNumber = index_1;
                cycle_1.IndexNumber = index_0;
                _DataService.DB.Update(cycle_0);
                _DataService.DB.Update(cycle_1);
                _DataService.DB.SaveChanges();

                lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
                if (lighting != null)
                {
                    Cycles = new ObservableCollection<LightingCycle>(lighting.Cycles.OrderBy(c => c.IndexNumber));
                }
            }
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

            var lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
            if (lighting != null)
            {
                var cycle_0 = lighting.Cycles.FirstOrDefault(x => x.Id == id);

                if (cycle_0.IndexNumber == lighting.Cycles.Count - 1) { return; }

                int index_0 = cycle_0.IndexNumber;
                int index_1 = index_0 + 1;

                var cycle_1 = lighting.Cycles.FirstOrDefault(x => x.IndexNumber == index_1);

                cycle_0.IndexNumber = index_1;
                cycle_1.IndexNumber = index_0;
                _DataService.DB.Update(cycle_0);
                _DataService.DB.Update(cycle_1);
                _DataService.DB.SaveChanges();

                lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Guid == _LightingViewModel.SelectedLighting.Guid);
                if (lighting != null)
                {
                    Cycles = new ObservableCollection<LightingCycle>(lighting.Cycles.OrderBy(c => c.IndexNumber));
                }
            }
            IndexLightingCycle = -1;
        }
        #endregion


        #region SaveInDBCommand - описание команды 
        private LambdaCommand _SaveInDBCommand;
        public ICommand SaveInDBCommand => _SaveInDBCommand ??=
            new LambdaCommand(OnSaveInDBCommandExecuted, CanSaveInDBCommandExecute);
        private bool CanSaveInDBCommandExecute(object p) => true;
        private void OnSaveInDBCommandExecuted(object p)
        {
            foreach (var item in Cycles)
            {
                var cycle = _DataService.DB.LightingCycles.FirstOrDefault(x => x.Id == item.Id);
                if (cycle != null)
                {
                    cycle.Step = item.Step;
                    cycle.DisplayTime = item.DisplayTime;
                    cycle.ColorWheelStart = item.ColorWheelStart;
                    cycle.ColorWheelEnd = item.ColorWheelEnd;
                    cycle.ColorSecondStart = item.ColorSecondStart;
                    cycle.ColorSecondEnd = item.ColorSecondEnd;
                    _DataService.DB.Update(cycle);
                }
            }
            _DataService.DB.SaveChanges();
        }
        #endregion

    }
}
