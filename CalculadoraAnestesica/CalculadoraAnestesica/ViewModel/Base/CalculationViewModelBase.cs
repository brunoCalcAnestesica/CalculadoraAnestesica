using System;
using System.ComponentModel;
using System.Windows.Input;
using CalculadoraAnestesica.Shared;
using Xamarin.Forms;

namespace CalculadoraAnestesica.ViewModel.Base
{
	public class CalculationViewModelBase : ViewModelBase
	{
        protected bool HasChangedValues { get; set; } = false;

        private string entryWeight;
        public string EntryWeight
        {
            get { return entryWeight; }
            set
            {
                entryWeight = GetStringProperties(value);
                RaisePropertyChanged();
            }
        }

        private string entryAge;
        public string EntryAge
        {
            get { return entryAge; }
            set
            {
                entryAge = GetStringProperties(value);
                RaisePropertyChanged();
            }
        }

        private string GetStringProperties(string value)
        {
            return string.IsNullOrEmpty(value)
                ? "0"
                : value;
        }

        protected override void ViewModelBase_PropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {
            base.ViewModelBase_PropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(EntryAge):
                    AppSource.Idade = double.Parse(EntryAge);
                    break;
                case nameof(EntryWeight):
                    AppSource.Peso = double.Parse(EntryWeight);
                    break;
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            CheckHasValuesChanged();
        }

        public void CheckHasValuesChanged()
        {
            if (AppSource.Peso.HasValue && AppSource.Peso != double.Parse(EntryWeight ?? "0"))
            {
                EntryWeight = AppSource.Peso.Value.ToString();
                HasChangedValues = true;
            }

            if (AppSource.Idade.HasValue && AppSource.Idade.Value != double.Parse(EntryAge ?? "0"))
            {
                EntryAge = AppSource.Idade.Value.ToString();
                HasChangedValues = true;
            }
        }
    }
}