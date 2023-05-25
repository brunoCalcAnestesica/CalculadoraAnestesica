using System;
using CalculadoraAnestesica.DependencyInjection;
using CalculadoraAnestesica.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraAnestesica.Model.DependencyInjection
{
    public class ModelDI : IDependencyInjection
    {
        public void AddDependencies(IServiceCollection services)
        {
            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<IFavoriteMedications, FavoriteMedications>();
        }
    }
}

