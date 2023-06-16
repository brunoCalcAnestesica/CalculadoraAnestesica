using System;
using CalculadoraAnestesica.Model;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.Model.Interfaces;
using System.Linq;
using CalculadoraAnestesica.Helpers;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalculadoraAnestesica.Shared;
using System.Diagnostics;

namespace CalculadoraAnestesica.ViewModel.Base
{
	public class MedicationsViewModelBase : CalculationViewModelBase
	{
        protected readonly IMedicamentosDataAccess _medicationDataAccess;
        protected List<GrupoNomesDTO> AuxList;
        public List<Medicamento> FilteredMedicamentosList;
        public bool IsFiltering;

        public ICommand SearchIconCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (isHeaderVisible)
                    {
                        IsHeaderVisible = false;
                    }
                    else
                    {
                        IsHeaderVisible = true;
                    }
                });
            }
        }
        private ObservableCollection<GrupoNomesDTO> grupoNomes;
        public ObservableCollection<GrupoNomesDTO> GrupoNomes
        {
            get { return grupoNomes; }
            set
            {
                if (grupoNomes != value)
                {
                    grupoNomes = value;
                    RaisePropertyChanged();
                }       
            }
        }

        private bool isSearchBarVisible = false;
        public bool IsSearchBarVisible
        {
            get { return isSearchBarVisible; }
            set
            {
                isSearchBarVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool isHeaderVisible = true;
        public bool IsHeaderVisible
        {
            get { return isHeaderVisible; }
            set
            {
                isHeaderVisible = value;
                RaisePropertyChanged();
            }
        }

        private string searchBarText;
        public string SearchBarText
        {
            get { return searchBarText; }
            set
            {
                searchBarText = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Medicamentos> medicamentos;
        public ObservableCollection<Medicamentos> Medicamentos
        {
            get { return medicamentos; }
            set
            {
                medicamentos = value;
                RaisePropertyChanged();
            }
        }           

        private bool isParametersVisible;
        public bool IsParametersVisible
        {
            get { return isParametersVisible; }
            set
            {
                isParametersVisible = value;
                RaisePropertyChanged();
            }
        }

        private string grupoMedicamento;
        public string GrupoMedicamento
        {
            get { return grupoMedicamento; }
            set
            {
                grupoMedicamento = value;
                RaisePropertyChanged();
            }
        }

        private double itemsSize;
        public double ItemsSize
        {
            get { return itemsSize; }
            set
            {
                itemsSize = value;
                RaisePropertyChanged();
            }
        }

        public ICommand FavoriteIconCommand
        {
            get
            {
                return new Command((item) => { FavotireIconClicked(item); });
            }
        }

        public MedicationsViewModelBase()
        {
            _medicationDataAccess = Resolver.Get<IMedicamentosDataAccess>();
            Medicamentos = new ObservableCollection<Medicamentos>();
            GrupoNomes = new ObservableCollection<GrupoNomesDTO>();
            AuxList = new List<GrupoNomesDTO>();
        }

        public override void OnAppearing()
        {
            try
            {
                if (!GrupoNomes.Any())
                {
                    List<GrupoNomesDTO> grupos = _medicationDataAccess
                        .GetGrupoNomes()
                        .OrderBy(x => x.NomeGrupo)
                        .ToList();

                    if (grupos != null)
                    {
                        foreach (var item in grupos)
                            GrupoNomes.Add(item);
                    }

                    AuxList = new List<GrupoNomesDTO>(GrupoNomes);
                }

                base.OnAppearing();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage();
            }
        }

        protected override void ViewModelBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ViewModelBase_PropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(SearchBarText):
                    ApplyFilter();
                    break;
                case nameof(IsHeaderVisible):
                    IsSearchBarVisible = !IsHeaderVisible;
                    break;
            }
        }

        protected virtual void FavotireIconClicked(object item)
        {
            try
            {
                Medicamento model = (Medicamento)item;
                model.IsFavorite = !model.IsFavorite;

                string groupName = _medicationDataAccess
                    .GetGrupoNomeById(model.IdGrupo);

                SetFavorite(model, groupName);
                UpdateMedicationList(model);
                
            }
            catch (Exception)
            {
                MessageHelper.ShowErrorMessage();
            }
        }

        private void UpdateMedicationList(Medicamento model)
        {
            List<Medicamento> list = AppSource.MedicamentosList;

            Medicamento item = list.FirstOrDefault(
                x => x.Id == model.Id &&
                x.NomeMedicamento == model.NomeMedicamento
            );

            item.IsFavorite = model.IsFavorite;
        }

        private void SetIsFavorite(Medicamento item)
        {
            throw new NotImplementedException();
        }

        public virtual List<Medicamento> GetMedications(GrupoNomesDTO grupo)
        {
            try
            {
                return AppSource.MedicamentosList
                    .Where(x => x.IdGrupo == grupo.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<Medicamento>();
            }
        }

        public virtual List<Medicamento> ExecuteCalculation(List<Medicamento> medicamentos)
        {
            try
            {
                double weight = string.IsNullOrEmpty(EntryWeight)
                    ? 0
                    : double.Parse(EntryWeight);

                foreach (var med in medicamentos)
                {
                    (double result1, double? result2) = Calculate(
                        med.DosagemMedicamento, weight
                    );

                    med.Resultado = result2.HasValue
                        ? $"{result1} - {result2}mg"
                        : $"{result1}mg";
                }

                return medicamentos;
            }
            catch (Exception ex)
            {
                return new List<Medicamento>();
            }
        }

        public virtual void ExecuteCalculation(ObservableCollection<Medicamento> medicamentos)
        {
            try
            {
                double weight = string.IsNullOrEmpty(EntryWeight)
                    ? 0
                    : double.Parse(EntryWeight);

                foreach (var med in medicamentos)
                {
                    (double result1, double? result2) = Calculate(
                        med.DosagemMedicamento, weight
                    );

                    med.Resultado = result2.HasValue
                        ? $"{result1} - {result2}mg"
                        : $"{result1}mg";
                }
            }
            catch (Exception ex)
            {
            }
        }

        public (double, double?) Calculate(string dosagem, double peso)
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

        private string GetOnlyNumber(string doseString)
        {
            var result = doseString.Where(c => char.IsDigit(c) || c == ',');
            return string.Join("", result);
        }

        private void SetFavorite(Medicamento model,
            string groupName)
        {
            _medicationDataAccess.SetFavoriteMedication(
                Utils.ConvertToTableSchema(groupName),
                model.Id,
                model.IsFavorite
            );

            var favoriteMed = Resolver.Get<IFavoriteMedications>();
            favoriteMed.MedicationName = model.NomeMedicamento;
            favoriteMed.GroupName = groupName;
            favoriteMed.MedicationId = model.Id;
            favoriteMed.IdGrupo = model.IdGrupo;

            if (model.IsFavorite)
            {
                Resolver
                 .Get<IFavoriteMedicationsDataAccess>()
                 .Insert(favoriteMed);

                return;
            }

            Resolver
             .Get<IFavoriteMedicationsDataAccess>()
             .Delete(favoriteMed);
        }

        private void ApplyFilter()
        {
            string filter = SearchBarText;
            IsFiltering = true;

            if (string.IsNullOrEmpty(filter))
            {
                GrupoNomes = new ObservableCollection<GrupoNomesDTO>(AuxList);
                IsFiltering = false;
                return;
            }

            FilteredMedicamentosList = AppSource.MedicamentosList
                .Where(item =>  item.NomeMedicamento.Contains(filter))
                .ToList();

            var items = FilteredMedicamentosList
                .GroupBy(x => x.IdGrupo)
                .Select(c => c.First())
                .ToList();

            var list = new List<GrupoNomesDTO>();

            for (int i = 0; i < items.Count(); i++)
            {
                var newItems = AuxList.Where(x => x.Id == items[i].IdGrupo);

                if (newItems.Any())
                    list.AddRange(newItems);
            }

            GrupoNomes = new ObservableCollection<GrupoNomesDTO>(list);
        }
    }
}