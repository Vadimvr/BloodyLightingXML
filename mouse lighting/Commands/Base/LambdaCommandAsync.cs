using System.Windows.Input;

namespace mouse_lighting.Commands.Base
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
    internal abstract class CommandAsync : ICommand
    {
        private bool _Executable = true;
        public bool Executable
        {
            get => _Executable;
            set
            {
                if (_Executable == value) return;
                _Executable = value;
                ExecutableChanged?.Invoke(this, EventArgs.Empty);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public event EventHandler? ExecutableChanged;
        event EventHandler? ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        bool ICommand.CanExecute(object? parameter) => _Executable && CanExecute(parameter);
        async void ICommand.Execute(object? parameter)
        {
            if (!((ICommand)this).CanExecute(parameter)) return;
            try
            {
                Executable = false;
                await ExecuteAsync(parameter);
            }
            catch
            {
                throw;
            }
            finally
            {
                Executable = true;
            }
        }
        protected virtual bool CanExecute(object? p) => true;
        protected abstract Task ExecuteAsync(object? p);
    }

    internal class LambdaCommandAsync : CommandAsync
    {
        private readonly ActionAsync<object?> _Execute;
        private readonly Func<object?, bool>? _CanExecute;
        public LambdaCommandAsync(ActionAsync Execute, Func<bool>? CanExecute = null)
            : this(async p => await Execute(), CanExecute is null ? (Func<object?, bool>?)null : p => CanExecute()) { }
        public LambdaCommandAsync(ActionAsync<object?> Execute, Func<object?, bool>? CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }
        protected override bool CanExecute(object? p) => _CanExecute?.Invoke(p) ?? true;
        //TODO: need test 
        protected override Task ExecuteAsync(object? p) => _Execute(p);
    }
}

