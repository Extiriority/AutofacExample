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
            builder.RegisterType<ServiceD>().As<IServiceD>();
            builder.RegisterType<ServiceE>().As<IServiceE>();

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

    // ServiceB depends on ServiceA, ServiceD, and ServiceE
    public class ServiceB : IServiceB, IServiceA, IServiceD, IServiceE
    {
        private readonly IServiceA _serviceA;
        private readonly IServiceD _serviceD;

        public ServiceB(IServiceA serviceA, IServiceD serviceD)
        {
            _serviceA = serviceA;
            _serviceD = serviceD;
        }

        public void DoB()
        {
            _serviceA.DoA();
            // Implementation of DoB
            Console.WriteLine("ServiceB.DoB() has been called.");
        }

        public void DoA()
        {
            Console.WriteLine("ServiceB.DoA() has been called.");
        }

        public void DoD()
        {
            _serviceD.DoD();
            // Implementation of DoD
            Console.WriteLine("ServiceB.DoD() has been called.");
        }

        public void DoE()
        {
            Console.WriteLine("ServiceB.DoE() has been called.");
        }
    }

    public interface IServiceC
    {
        void DoC();
    }

    // ServiceC depends on ServiceB
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
    
    public interface IServiceD
    {
        void DoD();
    }

    public class ServiceD : IServiceD
    {
        public void DoD()
        {
            // Implementation of DoD
            Console.WriteLine("ServiceD.DoD() has been called.");
        }
    }

    public interface IServiceE
    {
        void DoE();
    }

    public class ServiceE : IServiceE
    {
        public void DoE()
        {
            // Implementation of DoE
            Console.WriteLine("ServiceE.DoE() has been called.");
        }
    }
}