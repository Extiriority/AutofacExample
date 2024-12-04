using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace ExampleAutofac
{
    public partial class App
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // Register dependencies
            builder.RegisterType<ServiceA>().As<IServiceA>().SingleInstance();
            builder.RegisterType<ServiceB>().As<IServiceB>();
            builder.RegisterType<ServiceC>().As<IServiceC>();

            _container = builder.Build();

            var serviceProvider = new AutofacServiceProvider(_container);

            // Resolve IServiceC and call its method
            var serviceC = serviceProvider.GetService<IServiceC>();
            serviceC.DoC();

            // Output to console
            Console.WriteLine("ServiceC.DoC() has been called.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }

    public interface IServiceA
    {
        void DoA();
    }

    public class ServiceA : IServiceA
    {
        public void DoA()
        {
            // Implementation of DoA
            Console.WriteLine("ServiceA.DoA() has been called.");
        }
    }

    public interface IServiceB
    {
        void DoB();
    }

    public class ServiceB : IServiceB
    {
        private readonly IServiceA _serviceA;

        public ServiceB(IServiceA serviceA)
        {
            _serviceA = serviceA;
        }

        public void DoB()
        {
            _serviceA.DoA();
            // Implementation of DoB
            Console.WriteLine("ServiceB.DoB() has been called.");
        }
    }

    public interface IServiceC
    {
        void DoC();
    }

    public class ServiceC : IServiceC
    {
        private readonly IServiceB _serviceB;

        public ServiceC(IServiceB serviceB)
        {
            _serviceB = serviceB;
        }

        public void DoC()
        {
            _serviceB.DoB();
            // Implementation of DoC
            Console.WriteLine("ServiceC.DoC() has been called.");
        }
    }
}