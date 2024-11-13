using mouse_lighting.Infrastructure.Commands;
using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using mouse_lighting.Services.LightingHandlers;
using System.Collections.Generic;
using Models;

namespace mouse_lighting.ViewModels
{
    internal class LightingCycleViewMode : ViewModel
    {
        private IDataTransferBetweenViews _DataTransferView;
        private IUserDialog _UserDialog;
        private IDataService _DataService;

        public LightingCycleViewMode(
            IUserDialog UserDialog,
            IDataService DataService,
            IDataTransferBetweenViews dataTransferView)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _DataTransferView = dataTransferView;
            _DataTransferView.UpdateSelectedLighting += UpdateCyclesView;
        }

        private ObservableCollection<LightingCycle> _Cycles;
        public ObservableCollection<LightingCycle> Cycles { get => _Cycles; set => Set(ref _Cycles, value); }

        private void UpdateCyclesView()
        {
            _DataService.DB.ChangeTracker.Clear();
            Cycles = new ObservableCollection<LightingCycle>(_DataService.DB.LightingCycles
                .Where(x => x.LightingId == _DataTransferView.Id)
                .OrderBy(c => c.IndexNumber));
        }

        private int _IndexLightingCycle;
        public int IndexLightingCycle
        {
            get => _IndexLightingCycle;
            set
            {
                if (Set(ref _IndexLightingCycle, value))
                {
                    _DataTransferView.Status(value.ToString());
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
            var Lighting = _DataService.DB.Lighting.FirstOrDefault(x => x.Id == _DataTransferView.Id);
            if (Lighting != null)
            {
                var index = -1;
                foreach (var item in Lighting.Cycles)
                {
                    if (index < item.IndexNumber)
                    {
                        index = item.IndexNumber;
                    }
                }
                Lighting.Cycles.Add(new() { IndexNumber = ++index });
                _DataService.DB.SaveChanges();
                UpdateCyclesView();
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
            var temp = _DataService.DB.LightingCycles.FirstOrDefault(x => x.Id == id);
            if (temp != null)
            {
                var index = 0;

                index = temp.IndexNumber;

                foreach (var item in Cycles)
                {
                    if (index < item.IndexNumber)
                    {
                        item.IndexNumber -= 1;
                        _DataService.DB.Update(item);
                    }
                }

                _DataService.DB.Remove(temp);
                _DataService.DB.SaveChanges();

                Cycles = new ObservableCollection<LightingCycle>(_DataService.DB.LightingCycles
                    .Where(x => x.LightingId == _DataTransferView.Id)
                    .OrderBy(x => x.IndexNumber));
            }

        }
        #endregion

        #region ExportToXmlLightingFileCommand - описание команды 
        private LambdaCommand _ExportToXmlLightingFileCommand;
        public ICommand ExportToXmlLightingFileCommand => _ExportToXmlLightingFileCommand ??=
            new LambdaCommand(OnExportToXmlLightingFileCommandExecuted, CanExportToXmlLightingFileCommandExecute);
        private bool CanExportToXmlLightingFileCommandExecute(object p) => _DataTransferView.Id > 0;
        LightingHandlerCreator LightingHandlerCreator = new LightingHandlerCreator();
        private void OnExportToXmlLightingFileCommandExecuted(object p)
        {
            var lighting = _DataService.DB.Lighting
                .FirstOrDefault(x => x.Id == _DataTransferView.Id);
            if (lighting != null)
            {
                List<FrameCycle> frames = LightingHandlerCreator
                    .Worker(lighting);
                _DataService.SaveToXML(lighting, frames);
                _DataTransferView.Status($"TODO export in File_{_DataTransferView.Guid}.xml");
            }
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

            var cycle_0 = Cycles.FirstOrDefault(x => x.Id == id);
            if (cycle_0.IndexNumber == 0) { return; }

            int index_0 = cycle_0.IndexNumber;
            int index_1 = index_0 - 1;

            var cycle_1 = Cycles.FirstOrDefault(x => x.IndexNumber == index_1);

            cycle_0.IndexNumber = index_1;
            cycle_1.IndexNumber = index_0;

            Cycles = new ObservableCollection<LightingCycle>(Cycles.OrderBy(c => c.IndexNumber));
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


            var cycle_0 = Cycles.FirstOrDefault(x => x.Id == id);

            if (cycle_0.IndexNumber == Cycles.Count - 1) { return; }

            int index_0 = cycle_0.IndexNumber;
            int index_1 = index_0 + 1;

            var cycle_1 = Cycles.FirstOrDefault(x => x.IndexNumber == index_1);

            cycle_0.IndexNumber = index_1;
            cycle_1.IndexNumber = index_0;


            Cycles = new ObservableCollection<LightingCycle>(Cycles.OrderBy(c => c.IndexNumber));
            IndexLightingCycle = -1;
        }
        #endregion

        #region SaveInDBCommand - описание команды 
        private LambdaCommand _SaveInDBCommand;
        public ICommand SaveInDBCommand => _SaveInDBCommand ??=
            new LambdaCommand(OnSaveInDBCommandExecuted, CanSaveInDBCommandExecute);
        private bool CanSaveInDBCommandExecute(object p) => _DataTransferView.Id > 0;
        private void OnSaveInDBCommandExecuted(object p)
        {
            foreach (var item in Cycles)
            {
                _DataService.DB.Update(item);
            }
            _DataService.DB.SaveChanges();
        }
        #endregion

    }
}
