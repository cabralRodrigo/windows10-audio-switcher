using AudioSwitcher.ViewModels;
using System.Windows;

namespace AudioSwitcher.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}