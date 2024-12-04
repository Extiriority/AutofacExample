using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Windows;

namespace AutofacDiagnostics
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // Load the ExampleAutofac assembly
            var exampleAutofacAssembly = Assembly.LoadFrom(@"E:\Code\WPF\ExampleAutofac\ExampleAutofac\bin\Debug\ExampleAutofac.exe");

            // Register dependencies from ExampleAutofac
            builder.RegisterAssemblyTypes(exampleAutofacAssembly)
                .AsImplementedInterfaces();

            _container = builder.Build();

            var serviceProvider = new AutofacServiceProvider(_container);

            // Output registration data to console
            foreach (var registration in _container.ComponentRegistry.Registrations)
            {
                Console.WriteLine($@"Service: {registration.Activator.LimitType}, Lifetime: {registration.Lifetime}");
            }

            // Resolve IServiceC and call its method using reflection
            var serviceCType = exampleAutofacAssembly.GetType("ExampleAutofac.ServiceC");
            var serviceC = serviceProvider.GetService(serviceCType);
            var doCMethod = serviceCType.GetMethod("DoC");
            doCMethod.Invoke(serviceC, null);

            // Output to console
            Console.WriteLine(@"ServiceC.DoC() has been called.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}