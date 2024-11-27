using Models;
using mouse_lighting.Commands.Base;
using mouse_lighting.Models;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.Services.Observer;
using mouse_lighting.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class CyclesViewModel : ViewModelBase
    {
        //TODO: create style from color Picker
        private readonly IDataTransferBetweenViews _DataTransferView;
        private readonly ICyclesModels _CyclesModels;
        private readonly IObserverStatusBar statusBar;
        private ObservableCollection<LightingCycle> _Cycles = default!;
        public ObservableCollection<LightingCycle> Cycles { get => _Cycles; set => Set(ref _Cycles, value); }

        private int indexLightingCycle = -1;
        public int IndexLightingCycle { get => indexLightingCycle; set => Set(ref indexLightingCycle, value); }
        public CyclesViewModel(
            IDataTransferBetweenViews dataTransferView,
            ICyclesModels cyclesModels, IObserverStatusBar StatusBar)
        {
            _DataTransferView = dataTransferView;
            Cycles = new ObservableCollection<LightingCycle>();
            _CyclesModels = cyclesModels;
            statusBar = StatusBar;
            _DataTransferView.UpdateSelectedLightingEvent += _CyclesModels.UpdateCycles;
            _CyclesModels.UpdateCyclesEvent += UpdateCycles;
        }

        private void UpdateCycles()
        {
            Cycles.Clear();
            foreach (var cycle in _CyclesModels.Cycles)
            {
                Cycles.Add(cycle);
            }
        }


        #region AddNewCycleCommand - описание команды 
        private LambdaCommand? _AddNewCycleCommand;
        public ICommand AddNewCycleCommand => _AddNewCycleCommand ??=
            new LambdaCommand(OnAddNewCycleCommandExecuted, CanAddNewCycleCommandExecute);
        private bool CanAddNewCycleCommandExecute(object? p) => _DataTransferView.Id > 0;
        private void OnAddNewCycleCommandExecuted(object? p)
        {
            _CyclesModels.AddNew();
        }
        #endregion


        //#region AddNewCycleCommand - описание команды 
        //private LambdaCommand? _AddNewCycleCommand;
        //public ICommand AddNewCycleCommand => _AddNewCycleCommand ??=
        //    new LambdaCommand(
        //        (p) => _CyclesModels.AddNew(),
        //        (p) => _DataTransferView.Id > 0);
        //#endregion


        #region RemoveCycleCommand - описание команды 
        private LambdaCommand? _RemoveCycleCommand;
        public ICommand RemoveCycleCommand => _RemoveCycleCommand ??=
            new LambdaCommand((p) => _CyclesModels.Delete(p));
        #endregion

        #region ExportToXmlLightingFileCommand - описание команды 
        private LambdaCommand? _ExportToXmlLightingFileCommand;
        public ICommand ExportToXmlLightingFileCommand => _ExportToXmlLightingFileCommand ??=
            new LambdaCommand((p) => _CyclesModels.Export(), (p) => Cycles?.Count > 0);
        #endregion

        #region UpCycleCommand - описание команды 
        private LambdaCommand? _UpCycleCommand;
        public ICommand UpCycleCommand => _UpCycleCommand ??=
            new LambdaCommand((p) => _CyclesModels.Up(p));
        #endregion

        #region DownCyrcleCommand - описание команды 

        private LambdaCommand? _DownCycleCommand;
        public ICommand DownCycleCommand => _DownCycleCommand ??=
            new LambdaCommand((p) => _CyclesModels.Down(p));
        #endregion

        //#region SaveInDBCommand
        //private LambdaCommandAsync? _SaveInDBCommand;
        //public ICommand SaveInDBCommand => _SaveInDBCommand ??=
        //    new LambdaCommandAsync( async () =>await _CyclesModels.SaveInDb(), () => _DataTransferView.Id > 0);
        //#endregion


        #region SaveInDBCommand - описание команды 
        private LambdaCommandAsync? _SaveInDBCommand;
        public ICommand SaveInDBCommand => _SaveInDBCommand ??=
            new LambdaCommandAsync(OnSaveInDBCommandExecuted, CanSaveInDBCommandExecute);

        private bool CanSaveInDBCommandExecute() => _DataTransferView.Id > 0;

        private async Task OnSaveInDBCommandExecuted()
        {

            await Task.Run(() =>
            {
                statusBar.StatusBar("Start");
                _CyclesModels.SaveInDb();
                statusBar.StatusBar("End");
            });
        }
        //private bool CanSaveInDBCommandExecute(object p) => true;
        //private async void OnSaveInDBCommandExecuted(object p) { }
        #endregion

    }
}
