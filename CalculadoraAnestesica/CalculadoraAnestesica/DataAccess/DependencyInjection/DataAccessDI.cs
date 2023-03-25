using System;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CalculadoraAnestesica.DataAccess.DependencyInjection
{
	public class DataAccessDI : IDependencyInjection
    {
        public void AddDependencies(IServiceCollection services)
        {
            services.AddTransient<IMedicamentosDataAccess, MedicamentosDataAccess>();
            services.AddTransient<IUserContextDataAccess, UserContextDataAcceess>();
        }
    }
}

