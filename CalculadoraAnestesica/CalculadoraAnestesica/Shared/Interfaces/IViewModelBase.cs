using System;
namespace CalculadoraAnestesica.Shared.Interfaces
{
	public interface IViewModelBase
	{
        void OnAppearing();
        void OnDisappearing();
        void AfterBinding();
    }
}

