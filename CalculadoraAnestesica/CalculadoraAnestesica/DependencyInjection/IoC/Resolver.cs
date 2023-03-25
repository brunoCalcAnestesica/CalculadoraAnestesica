using System;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraAnestesica.DependencyInjection.IoC
{
	public class Resolver
	{
        public static IServiceProvider ServiceProvider { get; set; }
        public static ServiceCollection ServiceCollection { get; set; }

        public static void Build()
        {
            ServiceCollection = new ServiceCollection();

            var serviceProvider = ServiceCollection
                .ConfigureServices()
                .BuildServiceProvider();

            ServiceProvider = serviceProvider;
        }

        public static T Get<T>()
        {
            var service = ServiceProvider.GetService<T>();

            if (service == null)
            {
                throw new Exception("Dependencia não encontrada!");
            }

            return service;
        }

        public static Type GetType<T>() =>
            ServiceProvider.GetService<T>().GetType();
    }
}

