using System;
using CalculadoraAnestesica.DependencyInjection;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraAnestesica.ViewModel.DependencyInjection
{
    public class ViewModelDI : IDependencyInjection
    {
        public void AddDependencies(IServiceCollection services)
        {
            services.AddTransient<IAppCenterViewModel, AppCenterViewModel>();
            services.AddTransient<IHemodinamicoCalcViewModel, HemodinamicoCalcViewModel>();
            services.AddTransient<IFavoriteViewModel, FavoriteViewModel>();
        }
    }
}

