namespace OpenProfiler.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Text;

    internal class AppenderBuilder
    {
        private readonly Assembly nhibernateAssembly;
        private readonly Assembly log4netAssembly;

        internal AppenderBuilder(Assembly nhibernateAssembly, Assembly log4netAssembly)
        {
            this.nhibernateAssembly = nhibernateAssembly;
            this.log4netAssembly = log4netAssembly;
        }

        internal object BuildAppender()
        {
            AssemblyName assemblyName = new AssemblyName("OpenProfiler.Appender");
            AssemblyBuilder assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "OpenProfilerAppender",
                TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass,
                log4netAssembly.GetType("log4net.Appender.UdpAppender"));

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "Append",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(void),
                new[] { log4netAssembly.GetType("log4net.Core.LoggingEvent") });

            methodBuilder.DefineParameter(1, ParameterAttributes.None, "loggingEvent");

            ILGenerator appendIL = methodBuilder.GetILGenerator();

            EmitAppendMethod(appendIL);

            Type t = typeBuilder.CreateType();

            return Activator.CreateInstance(t);
        }

        private void EmitAppendMethod(ILGenerator appendIL)
        {
            /* For storing SessionIdLoggingContext["sessionId"] */
            appendIL.DeclareLocal(typeof(Guid?));

            /* Load the "loggingEvent" argument passed to the method. */
            appendIL.Emit(OpCodes.Ldarg_1);

            /* Call "loggingEvent.Properties", push the result onto the stack */
            appendIL.Emit(OpCodes.Callvirt, log4netAssembly.GetType("log4net.Core.LoggingEvent").GetMethod("get_Properties"));
            appendIL.Emit(OpCodes.Ldstr, "sessionId");

            /* Call SessionIdLoggingContext.SessionId and store the result in the local variable. */
            appendIL.Emit(OpCodes.Call, nhibernateAssembly.GetType("NHibernate.Impl.SessionIdLoggingContext").GetMethod("get_SessionId"));
            appendIL.Emit(OpCodes.Stloc_0);

            /* Load the SessionId back onto the stack and call .ToString on it: */
            appendIL.Emit(OpCodes.Ldloca_S, (byte)0);
            appendIL.Emit(OpCodes.Constrained, typeof(Guid?));
            appendIL.Emit(OpCodes.Callvirt, typeof(object).GetMethod("ToString"));

            /* Set loggingEvent.Properties["sessionId"] equal to the .ToString() result. */
            appendIL.Emit(OpCodes.Callvirt, log4netAssembly.GetType("log4net.Util.ReadOnlyPropertiesDictionary").GetMethod("set_Item"));
            
            /* Call the base class (UdpAppender) .Append method */
            appendIL.Emit(OpCodes.Ldarg_0);
            appendIL.Emit(OpCodes.Ldarg_1);
            appendIL.Emit(
                OpCodes.Call,
                log4netAssembly.GetType("log4net.Appender.UdpAppender").GetMethod(
                    "Append", BindingFlags.Instance | BindingFlags.NonPublic,
                    Type.DefaultBinder,
                    new[] { log4netAssembly.GetType("log4net.Core.LoggingEvent") },
                    null));

            appendIL.Emit(OpCodes.Ret);
        }
    }
}
