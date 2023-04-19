using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CalculadoraAnestesica.Shared.Interfaces;

namespace CalculadoraAnestesica.ViewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

