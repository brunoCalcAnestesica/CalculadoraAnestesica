using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CalculadoraAnestesica.ApplicationHandler;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.ViewModel.Base;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Xamarin.Forms;

namespace CalculadoraAnestesica.ViewModel
{
	public class AppCenterViewModel : ViewModelBase, IAppCenterViewModel
    {
		public ICommand SearchIconCommand { get; }
        public ICommand CancelCommand { get; }
        
        private ObservableCollection<Medicamento> medicamentos;
        public ObservableCollection<Medicamento> Medicamentos
        {
            get { return medicamentos; }
            set
            {
                medicamentos = value;
                OnPropertyChanged();
            }
        }

        private bool searchBarVisible;
        public bool SearchBarVisible
        {
            get { return searchBarVisible; }
            set
            {
                searchBarVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isParametersVisible;
		public bool IsParametersVisible
		{
			get { return isParametersVisible; }
			set
			{
				isParametersVisible = value;
				OnPropertyChanged();
			}
		}

		private bool isSearchVisible;
		private void Teste()
		{
			if (!isSearchVisible)
            {
                isSearchVisible = true;
                SearchBarVisible = true;
                IsParametersVisible = false;
            }
            else
            {
                SearchBarVisible = false;
                IsParametersVisible = true;
                isSearchVisible = false;
            }
        }

        private string grupoMedicamento;
        public string GrupoMedicamento
        {
            get { return grupoMedicamento; }
            set
            {
                grupoMedicamento = value;
                OnPropertyChanged();
            }
        }

        private double itemsSize;
        public double ItemsSize
        {
            get { return itemsSize; }
            set
            {
                itemsSize = value;
                OnPropertyChanged();
            }
        }

        public AppCenterViewModel()
        {
            SetFirstAccess();
            SearchIconCommand = new Command(Teste);
            CancelCommand = new Command(Teste);
            IsParametersVisible = true;
        }

        public override void OnAppearing()
        {
            var medicamentos = Resolver
                .Get<IMedicamentosDataAccess>()
                .GetMedicamento(nameof(TablesSchema.ANALGESICOS));

            Medicamentos = new ObservableCollection<Medicamento>(medicamentos);
        }

        private void SetFirstAccess()
        {
            if (AppHandler.IsFirstAccess())
            {
                var userContext = Resolver.Get<IUserContext>();
                userContext.IsFirstAccess = true;

                Resolver
                .Get<IUserContextDataAccess>()
                .Insert(userContext);
            }
        }

        public List<GrupoNomesDTO> GetGrupoNomes()
        {
            return Resolver
                .Get<IMedicamentosDataAccess>()
                .GetGrupoNomes();
        }

        public void SetGrupoMedicamento(string grupoMedicamento)
        {
            GrupoMedicamento = grupoMedicamento;
        }

        public List<Medicamento> SetMedicamentos(string grupoNome,
            int id, double peso)
        {
            try
            {
                var medicamentos = Resolver
                        .Get<IMedicamentosDataAccess>()
                        .GetMedicamentosById(grupoNome, id);

                foreach (var med in medicamentos)
                {
                    (double result1, double? result2) = Calculate(
                        med.DosagemMedicamento, peso
                    );

                    med.Resultado = result2.HasValue ?
                        $"{result1} - {result2}mg" :
                        $"{result1}mg";
                }

                return medicamentos;
            }
            catch (Exception ex)
            {
                return new List<Medicamento>();
            }
        }

        private string GetOnlyNumber(string doseString)
        {
            var result = doseString.Where(c => char.IsDigit(c) || c == ',');
            return string.Join("", result);
        }

        private (double, double?) Calculate(string dosagem, double peso)
        {
            if (dosagem.Contains("-"))
            {
                string[] arr = dosagem.Split('-');

                string n1 = arr[0].Trim();
                string[] arrNum2 = arr[1].Trim().Split(' ');
                string n2 = GetOnlyNumber(arrNum2[0]);

                double result1 = double.Parse(n1) * peso;
                double result2 = double.Parse(n2) * peso;

                return (result1, result2);
            }

            string[] arr2 = dosagem.Split(' ');

            int? index = null;

            for (int i = 0; i < arr2.Length; i++)
            {
                string item = arr2[i].Trim();

                foreach (char c in item)
                {
                    if (char.IsDigit(c))
                    {
                        index = i;
                        break;
                    }

                    break;
                }

                if (index.HasValue)
                    break;
            }

            string num1 = GetOnlyNumber(arr2[index.Value]);

            double resultNum1 = double.Parse(num1) * peso;

            return (resultNum1, null);
        }

        private void ExecuteFilter()
        {
            var tables = GetGrupoNomes();
        }
    }
}

