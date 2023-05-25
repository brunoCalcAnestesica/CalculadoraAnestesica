using System;
using CalculadoraAnestesica.Shared.Interfaces;
using Xamarin.Forms;

namespace CalculadoraAnestesica.View.Base
{
    public class ViewBase : ContentPage, IViewContext
    {
        public void SetBinding(IViewModelBase viewModelBase)
        {
            if (viewModelBase != null)
                BindingContext = viewModelBase;
        }
        
        protected override void OnAppearing()   
        {
            if (BindingContext != null)
                ((IViewModelBase)BindingContext).OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (BindingContext != null)
                ((IViewModelBase)BindingContext).OnDisappearing();
        }

        public virtual void AddComponents() { }
    }
}

