using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac.Core;

namespace AutofacDiagnostics
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // Load the ExampleAutofac assembly
            const string dll = @"E:\Code\WPF\ExampleAutofac\ExampleAutofac\bin\Debug\ExampleAutofac.exe";
            var exampleAutofacAssembly = Assembly.LoadFrom(dll);

            // Register dependencies from ExampleAutofac
            builder.RegisterAssemblyTypes(exampleAutofacAssembly)
                .AsImplementedInterfaces();

            _container = builder.Build();

            // Output registration data to console
            var diagnostics = _container.ComponentRegistry.Registrations
                .Select(registration => new Registrations(
                    registration.Activator.LimitType.Name,
                    registration.Target.Services.First().Description.Split('.').Last(),
                    MapLifetime(registration.Sharing),
                    registration.Sharing.ToString(),
                    registration.Ownership.ToString(),
                    registration.Services.Select(s => s.Description.Split('.').Last()).ToList())
                );

            foreach (var diagnostic in diagnostics)
            {
                DisplayRegistrationDetails(diagnostic);
            }
        }

        private static void DisplayRegistrationDetails(Registrations diagnostic)
        {
            Console.WriteLine($@"Name: {diagnostic.Name}");
            Console.WriteLine($@"Type: {diagnostic.Type}");
            Console.WriteLine($@"Lifetime: {diagnostic.Lifetime}");
            Console.WriteLine($@"Scope: {diagnostic.Scope}");
            Console.WriteLine($@"Parent Scope: {diagnostic.ParentScope}");
            Console.WriteLine(@"Dependencies:");
            foreach (var dependency in diagnostic.Implementations)
            {
                Console.WriteLine($@"- {dependency}");
            }
            Console.WriteLine(@"--------------------------------------------------");
        }

        private static Lifetime MapLifetime(InstanceSharing sharing)
            => sharing == InstanceSharing.Shared ? Lifetime.SingleInstance : Lifetime.InstancePerDependency;

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }

    public enum Lifetime
    {
        SingleInstance,
        InstancePerDependency
    }
}