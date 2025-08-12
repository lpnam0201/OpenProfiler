using Ninject;
using Ninject.Modules;
using OpenProfiler.GUI.Service;
using OpenProfiler.GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.DI
{
    public class OpenProfilerGUIModule : NinjectModule
    {
        public override void Load()
        {   
            Bind<MainWindowViewModel>().ToSelf();
            Bind<IDataReceivingService>().To<DataReceivingService>().InSingletonScope();
            Bind<IBufferService>().To<BufferService>().InSingletonScope();
            Bind<IFormatService>().To<FormatService>().InSingletonScope();
        }
    }
}
