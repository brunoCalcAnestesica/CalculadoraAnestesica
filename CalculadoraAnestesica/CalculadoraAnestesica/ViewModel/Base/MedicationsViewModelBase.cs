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
using Xamarin.Forms.Internals;
using System.Globalization;

namespace CalculadoraAnestesica.ViewModel.Base
{
	public class MedicationsViewModelBase : CalculationViewModelBase
	{
        protected readonly IMedicamentosDataAccess _medicationDataAccess;
        protected List<GrupoNomesDTO> GroupAuxList;
        protected List<Medicamento> AuxMedicamentosList;
        public List<Medicamento> GroupFilteredMedicamentosList;
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

        private ObservableCollection<Medicamento> medicamentosList;
        public ObservableCollection<Medicamento> MedicamentosList
        {
            get { return medicamentosList; }
            set
            {
                if (medicamentosList != value)
                {
                    medicamentosList = value;
                    RaisePropertyChanged();
                }
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
            GroupAuxList = new List<GrupoNomesDTO>();
            MedicamentosList = new ObservableCollection<Medicamento>();
            AuxMedicamentosList = new List<Medicamento>();
        }

        public override void OnAppearing()
        {
            try
            {
                LoadMedicationList();
                LoadGroupMedicationList();
                base.OnAppearing();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage();
            }
        }

        protected virtual void LoadMedicationList()
        {
            if (!MedicamentosList.Any())
            {
                if (AppSource.Peso.HasValue)
                {
                    LoadMedicationListValues();
                }
                else
                {
                    MedicamentosList = new ObservableCollection<Medicamento>(AppSource.MedicamentosList);
                }

                AuxMedicamentosList = new List<Medicamento>(MedicamentosList);
            }
        }

        protected virtual void LoadGroupMedicationList()
        {
            if (!GrupoNomes.Any())
            {
                List<GrupoNomesDTO> grupos = _medicationDataAccess
                    .GetGrupoNomes()
                    .OrderBy(x => x.NomeGrupo)
                    .ToList();

                var newGroup = new List<GrupoNomesDTO>();

                foreach (var item in grupos)
                {
                    string nomeGrupo = item.NomeGrupo;

                    if (MedicamentosHelper.IsGroupedMedication(nomeGrupo))
                        newGroup.Add(item);
                }

                if (newGroup != null)
                {
                    foreach (var item in newGroup)
                        GrupoNomes.Add(item);
                }

                GroupAuxList = new List<GrupoNomesDTO>(GrupoNomes);
            }
        }

        protected virtual void LoadMedicationListValues()
        {
            foreach (var med in AppSource.MedicamentosList)
            {
                (double result1, double? result2) = Calculate(med.DosagemMedicamento, AppSource.Peso.Value);

                med.Resultado = result2.HasValue
                    ? $"{result1} - {result2}mg"
                    : $"{result1}mg";

                MedicamentosList.Add(med);
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
                case nameof(EntryWeight):
                    ExecuteCalculation(MedicamentosList?.ToList());
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
                return AppSource.MedicamentosGroupList
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

                    if (result1 == 0 && result2 is null)
                        return;

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
            try
            {
                if (string.IsNullOrEmpty(dosagem))
                    return (0, null);

                if (dosagem.StartsWith("-"))
                {
                    string n1 = GetOnlyNumber(dosagem);
                    double result = double.Parse(n1) * peso;
                    return (result, null);
                }

                if (dosagem.Contains("-"))
                {
                    string[] arr = dosagem.Split('-');

                    string n1 = arr[0].Trim().Replace(',', UserInfo.DecimalSeparator);
                    string[] arrNum2 = arr[1].Trim().Split(' ');
                    string n2 = GetOnlyNumber(arrNum2[0]);

                    double result1 = Math.Round(double.Parse(n1) * peso, 2);
                    double result2 = Math.Round(double.Parse(n2) * peso, 2);

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

                double resultNum1 = Math.Round(double.Parse(num1) * peso, 2);
                return (resultNum1, null);
            }
            catch (Exception ex)
            {
                return (0, null);
            }
        }

        private string GetOnlyNumber(string doseString)
        {
            doseString = doseString.Replace(',', UserInfo.DecimalSeparator);
            var result = doseString.Where(c => char.IsDigit(c) || c == UserInfo.DecimalSeparator);
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
            IsFiltering = true;

            if (string.IsNullOrEmpty(SearchBarText))
            {
                GrupoNomes = new ObservableCollection<GrupoNomesDTO>(GroupAuxList);
                MedicamentosList = new ObservableCollection<Medicamento>(AuxMedicamentosList);
                IsFiltering = false;
                return;
            }

            Task.Run(MedicamentosListFilterHandler);
            Task.Run(GroupFilterListHandler);
        }

        private void MedicamentosListFilterHandler()
        {
            List<Medicamento> filteredMedicamentos = AppSource.MedicamentosList
                .Where(item => item.NomeMedicamento.Contains(SearchBarText))
                .ToList();

            MedicamentosList = new ObservableCollection<Medicamento>(filteredMedicamentos);
        }

        private void GroupFilterListHandler()
        {
            GroupFilteredMedicamentosList = AppSource.MedicamentosGroupList
                .Where(item => item.NomeMedicamento.Contains(SearchBarText))
                .ToList();

            var items = GroupFilteredMedicamentosList
                .GroupBy(x => x.IdGrupo)
                .Select(c => c.First())
                .ToList();

            var list = new List<GrupoNomesDTO>();

            for (int i = 0; i < items.Count(); i++)
            {
                var newItems = GroupAuxList.Where(x => x.Id == items[i].IdGrupo);

                if (newItems.Any())
                    list.AddRange(newItems);
            }

            GrupoNomes = new ObservableCollection<GrupoNomesDTO>(list);
        }
    }
}