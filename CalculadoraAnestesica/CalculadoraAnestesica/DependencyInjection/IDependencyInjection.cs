using System;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraAnestesica.DependencyInjection
{
	public interface IDependencyInjection
	{
        void AddDependencies(IServiceCollection services);
    }
}

    