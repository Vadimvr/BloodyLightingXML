using mouse_lighting.ViewModels.Base;
namespace mouse_lighting.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
      
        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Lighting for the bloody mouse";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion
      
        public MainWindowViewModel()
        {
          

        }
    }
}