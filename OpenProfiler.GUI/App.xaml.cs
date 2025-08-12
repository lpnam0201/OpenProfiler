using Ninject;
using OpenProfiler.GUI.DI;
using OpenProfiler.GUI.Service;
using OpenProfiler.GUI.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OpenProfiler.GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IKernel _kernel;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _kernel = new StandardKernel();
        ConfigureServices();

        MainWindow = new MainWindow();
        MainWindow.DataContext = _kernel.Get<MainWindowViewModel>();
        MainWindow.Show();
    }

    private void ConfigureServices()
    {
        _kernel.Load<OpenProfilerGUIModule>();
    }

}

