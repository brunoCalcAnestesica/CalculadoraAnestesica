using System;
using CalculadoraAnestesica.ViewModel.Base;

namespace CalculadoraAnestesica.Shared.Interfaces
{
    public interface IViewContext
	{
        void SetBinding(IViewModelBase viewModelBase);
        void AddComponents();
    }
}

