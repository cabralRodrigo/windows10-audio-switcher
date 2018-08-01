using AudioSwitcher.Commands;
using AudioSwitcher.Services;
using AudioSwitcher.Util;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AudioSwitcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly AudioService audioService;
        private readonly StartupService startupService;
        private readonly Task Background;

        public StateType State { get; set; }
        public ICommand SwitchCommand { get; set; }
        public ICommand SairCommand { get; set; }
        public ICommand AddAppStartupCommand { get; set; }
        public ICommand RemoverAppStartupCommand { get; set; }

        public MainWindowViewModel()
        {
            this.audioService = new AudioService();
            this.startupService = new StartupService();
            this.Background = Task.Run(this.RefreshState);
            this.SwitchCommand = new RelayCommand(this.SwitchState);
            this.SairCommand = new RelayCommand(this.Sair);
            this.AddAppStartupCommand = new RelayCommand(this.AddAppStartup);
            this.RemoverAppStartupCommand = new RelayCommand(this.RemoverAppStartup);
        }

        private void RemoverAppStartup()
        {
            if (!this.startupService.RemoverApp())
                MessageBox.Show("Erro ao remover a execução automática da aplicação.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void AddAppStartup()
        {
            if (!this.startupService.AdicionarApp())
                MessageBox.Show("Erro ao adicionar a execução automática da aplicação.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Sair()
        {
            Application.Current.Shutdown();
        }

        private void SwitchState()
        {
            var currentState = this.audioService.GetCurrentState();
            StateType newState;

            switch (currentState)
            {
                case StateType.Stereo:
                    newState = StateType.Mono;
                    break;
                case StateType.Mono:
                    newState = StateType.Stereo;
                    break;
                case StateType.Unknown:
                default:
                    MessageBox.Show("Não foi possível determinar se o windows está com audio mono ou não.", "Erro", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
            }

            try
            {
                this.audioService.SetCurrentState(currentState, newState);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao trocar o modo do audio.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RefreshState()
        {
            while (true)
            {
                this.State = this.audioService.GetCurrentState();
                await Task.Delay(1000);
            }
        }
    }
}