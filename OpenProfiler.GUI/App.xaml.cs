using Ninject;
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
        MainWindow = _kernel.Get<MainWindow>();
        MainWindow.DataContext = _kernel.Get<MainWindowViewModel>();
    }

    private void ConfigureServices()
    {
        _kernel.Bind<MainWindowViewModel>().ToSelf();
        _kernel.Bind<IDataReceivingService>().To<DataReceivingService>().InSingletonScope();
        _kernel.Bind<IBufferService>().To<BufferService>().InSingletonScope();
        _kernel.Bind<IFormatService>().To<FormatService>().InSingletonScope();
    }

}

