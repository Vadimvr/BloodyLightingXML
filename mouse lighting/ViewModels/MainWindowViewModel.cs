using mouse_lighting.Services.Interfaces;
using mouse_lighting.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using mouse_lighting.Models;
using System;
using mouse_lighting.Infrastructure.Commands;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;
using mouse_lighting.Services.LightingHandlers;
namespace mouse_lighting.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Главное окно";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _Status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _Status; set => Set(ref _Status, value); }

        #endregion

        public MainWindowViewModel(IUserDialog UserDialog, IDataService DataService)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
        }

        private ObservableCollection<Color> _Colors;
        public ObservableCollection<Color> Colors { get => _Colors; set => Set(ref _Colors, value); }

        private ObservableCollection<Lighting> _Names = new ObservableCollection<Lighting>() {
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

        public ObservableCollection<Lighting> Names { get => _Names; set => Set(ref _Names, value); }


        private Lighting _SelectedLighting;
        public Lighting SelectedLighting
        {
            get { return _SelectedLighting; }
            set
            {
                if (Set(ref _SelectedLighting, value))
                {
                    Cycles = SelectedLighting.Cycles;
                }
            }
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
                    Status = value.ToString();
                }
            }
        }

        #region AddNewLightingCommand - описание команды 
        private LambdaCommand _AddNewLightingCommand;
        public ICommand AddNewLightingCommand => _AddNewLightingCommand ??=
            new LambdaCommand(OnAddNewLightingCommandExecuted, CanAddNewLightingCommandExecute);
        private bool CanAddNewLightingCommandExecute(object p) => true;
        private void OnAddNewLightingCommandExecuted(object p)
        {
            if (Names == null)
            {
                Names = new ObservableCollection<Lighting>();
            }
            var defNewName = "New animation";
            var newName = defNewName;
            var j = 0;
            for (int i = 0; i < Names.Count; i++)
            {
                if (Names[i].Name == newName)
                {
                    i = 0;
                    j++;
                    newName = $"{defNewName}_{j}";
                }
            }
            Names.Add(new Lighting() { Cycles = new ObservableCollection<LightingCycle>(), Guid = Guid.NewGuid(), Name = newName });
        }
        #endregion

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
        private bool CanExportToXmlLightingFileCommandExecute(object p) => SelectedLighting != null;
        LightingHandlerCreator LightingHandlerCreator = new LightingHandlerCreator();
        private void OnExportToXmlLightingFileCommandExecuted(object p)
        {
            if (SelectedLighting == null) { throw new ArgumentNullException(nameof(SelectedLighting)); }
            List<FrameCycle> frames = LightingHandlerCreator.Worker(SelectedLighting);
            _DataService.Save(SelectedLighting, frames);
            Status = "TODO export in xml";
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
                Status = "First item";
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
                Status = "last item";
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