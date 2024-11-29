using Models;
using mouse_lighting.Commands.Base;
using mouse_lighting.Models;
using mouse_lighting.Services.DataTransferBetweenView;
using mouse_lighting.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace mouse_lighting.ViewModels
{
    internal class LightingViewModel : ViewModelBase
    {

        private readonly LightingModel _LightingModel;
        private readonly IDataTransferBetweenViews _DataTransferView;

        private ObservableCollection<Lighting> _Lighting = new();
        public ObservableCollection<Lighting> Lighting { get => _Lighting; set => Set(ref _Lighting, value); }


        private Lighting? _SelectedLighting;
        public Lighting? SelectedLighting
        {
            get => _SelectedLighting; set
            {
                if (Set(ref _SelectedLighting, value))
                {
                    _DataTransferView.Update(_SelectedLighting == null ? 0 : _SelectedLighting.Id);
                }
            }
        }

        public LightingViewModel(
            IDataTransferBetweenViews dataTransferView, LightingModel lightingModel)
        {
            _DataTransferView = dataTransferView;

            _LightingModel = lightingModel;
            _LightingModel.UpdateLightingEvent += UpdateLighting;

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            Lighting.Clear();
            _LightingModel.Lighting.ForEach(x => { Lighting.Add(x); });
        }

        #region AddNewLightingCommand - описание команды 
        private LambdaCommand? _AddNewLightingCommand;
        public ICommand AddNewLightingCommand => _AddNewLightingCommand ??=
            new LambdaCommand(OnAddNewLightingCommandExecuted, CanAddNewLightingCommandExecute);
        private bool CanAddNewLightingCommandExecute(object? p) => _LightingModel.Lighting != null;
        private void OnAddNewLightingCommandExecuted(object? p)
        {
            var id = _LightingModel.AddNew().Id;
            SelectedLighting = Lighting.SingleOrDefault(x => x.Id == id);
        }
        #endregion

        #region RemoveLightingCommand - описание команды 
        private LambdaCommand? _RemoveLightingCommand;
        public ICommand RemoveLightingCommand => _RemoveLightingCommand ??=
            new LambdaCommand(OnRemoveLightingCommandExecuted, CanRemoveLightingCommandExecute);
        private bool CanRemoveLightingCommandExecute(object? p) => SelectedLighting != null;
        private void OnRemoveLightingCommandExecuted(object? p) { if (SelectedLighting != null) { _LightingModel.Remove(SelectedLighting); } }
        #endregion


        #region UpdateNameCommand - описание команды 
        private LambdaCommand? _UpdateNameCommand;
        public ICommand UpdateNameCommand => _UpdateNameCommand ??=
            new LambdaCommand(OnUpdateNameCommandExecuted, CanUpdateNameCommandExecute);
        private bool CanUpdateNameCommandExecute(object? p) => SelectedLighting != null;
        private async void OnUpdateNameCommandExecuted(object? p)
        {

            if (SelectedLighting != null && SelectedLighting.Id > 0 && !string.IsNullOrEmpty(SelectedLighting.Name))
            {
                await _LightingModel.UpdateNameAsync(SelectedLighting).ConfigureAwait(false);
            }
        }
        #endregion
    }
}




