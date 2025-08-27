using Microsoft.Extensions.Configuration;
using Ninject;
using OpenProfiler.GUI.DI;
using OpenProfiler.GUI.Service;
using OpenProfiler.GUI.ViewModel;
using System.Configuration;
using System.Data;
using System.IO;
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
        ConfigureAppSettings();
        AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

        MainWindow = new MainWindow();
        MainWindow.DataContext = _kernel.Get<MainWindowViewModel>();
        MainWindow.Show();
    }

    private void ConfigureServices()
    {
        _kernel.Load<OpenProfilerGUIModule>();
    }

    private void ConfigureAppSettings()
    {
        var builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configurationRoot = builder.Build();
        _kernel.Bind<IConfiguration>().ToConstant(configurationRoot);
    }

    private void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        File.WriteAllText("crashlog.txt", e.ExceptionObject.ToString());
    }

}

