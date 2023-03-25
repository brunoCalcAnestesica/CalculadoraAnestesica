using System;
using CalculadoraAnestesica.DataAccess;
using CalculadoraAnestesica.DataAccess.DependencyInjection;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.Model.DependencyInjection;
using CalculadoraAnestesica.View.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CalculadoraAnestesica.DependencyInjection
{
	public static class DependenciesHandler
	{
        private static IServiceCollection _serviceCollection;
        private static IServiceCollection ServiceCollection
        {
            get
            {
                if (_serviceCollection is null)
                    _serviceCollection = new ServiceCollection();

                return _serviceCollection;
            }
        }

        public static IServiceCollection ConfigureServices(this
            IServiceCollection services)
        {
            AddDependencies(new DataAccessDI());
            AddDependencies(new ViewDI());
            AddDependencies(new ModelDI());
            return ServiceCollection;
        }

        private static void AddDependencies(IDependencyInjection dependency)
        {
            dependency.AddDependencies(ServiceCollection);
        }
    }
}

