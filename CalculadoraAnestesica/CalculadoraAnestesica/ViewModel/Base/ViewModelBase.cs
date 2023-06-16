using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CalculadoraAnestesica.Shared.Interfaces;

namespace CalculadoraAnestesica.ViewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModelBase()
        {
            PropertyChanged += ViewModelBase_PropertyChanged;
        }

        protected virtual void ViewModelBase_PropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {

        }

        public virtual void AfterBinding()
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

    }
}

