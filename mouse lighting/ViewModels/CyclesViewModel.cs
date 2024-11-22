using Models;
using mouse_lighting.Commands.Base;
using mouse_lighting.Models;
using mouse_lighting.Services.DataService;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.Services.UserDialog;
using mouse_lighting.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class CyclesViewModel : ViewModelBase
    {
        //TODO: create style from color Picker
        private readonly IDataTransferBetweenViews _DataTransferView;
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;
        readonly CyclesModels _CyclesModels;

        private ObservableCollection<LightingCycle> _Cycles = default!;
        public ObservableCollection<LightingCycle> Cycles { get => _Cycles; set => Set(ref _Cycles, value); }

        private int indexLightingCycle = -1;
        public int IndexLightingCycle { get => indexLightingCycle; set => Set(ref indexLightingCycle, value); }
        public CyclesViewModel(
            IUserDialog UserDialog,
            IDataService DataService,
            IDataTransferBetweenViews dataTransferView)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _DataTransferView = dataTransferView;
            Cycles = new ObservableCollection<LightingCycle>();
            _CyclesModels = new CyclesModels(_DataService);
            _DataTransferView.UpdateSelectedLightingEvent += _CyclesModels.UpdateCycles;
            _CyclesModels.UpdateCyclesEvent += UpdateCycles;
        }

        private void UpdateCycles() => Cycles = _CyclesModels.Cycles;


        #region AddNewCycleCommand - описание команды 
        private LambdaCommand? _AddNewCycleCommand;
        public ICommand AddNewCycleCommand => _AddNewCycleCommand ??=
            new LambdaCommand((p) => _CyclesModels.AddNew(), (p) => _DataTransferView.Id > 0);
        #endregion


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

        #region SaveInDBCommand
        private LambdaCommand? _SaveInDBCommand;
        public ICommand SaveInDBCommand => _SaveInDBCommand ??=
            new LambdaCommand(() => _CyclesModels.SaveInDb(), () => _DataTransferView.Id > 0);
        #endregion
    }
}
