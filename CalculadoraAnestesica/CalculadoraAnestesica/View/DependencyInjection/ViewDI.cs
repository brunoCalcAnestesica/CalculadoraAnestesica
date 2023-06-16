using System;
using CalculadoraAnestesica.DependencyInjection;
using CalculadoraAnestesica.View.Intercaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraAnestesica.View.DependencyInjection
{
    public class ViewDI : IDependencyInjection
    {
        public void AddDependencies(IServiceCollection services)
        {
            services.AddTransient<IAppCenterView, AppCenterView>();
            services.AddTransient<IHemodinamicoCalcView, HemodinamicoCalcView>();
            services.AddTransient<IFavoriteMedicationsView, FavoriteMedicationsView>();
        }
    }
}

