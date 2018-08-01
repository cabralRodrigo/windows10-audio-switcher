using System;
using System.Windows.Input;

namespace AudioSwitcher.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action command;

        public RelayCommand(Action command) => this.command = command;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => this.command();
    }
}