using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.ViewModel.Base;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Xamarin.Forms;

namespace CalculadoraAnestesica.ViewModel
{
	public class FavoriteViewModel : MedicationsViewModelBase, IFavoriteViewModel
    {
        private List<Medicamento> FavMedications;

        private ObservableCollection<GroupFavoriteMedications> favoriteMedications;
        public ObservableCollection<GroupFavoriteMedications> FavoriteMedications
        {
            get { return favoriteMedications; }
            set
            {
                favoriteMedications = value;
                RaisePropertyChanged();
            }
        }

        public FavoriteViewModel() : base()
        {
            FavMedications = new List<Medicamento>();
            FavoriteMedications = new ObservableCollection<GroupFavoriteMedications>();
            Task.Run(SetFavoriteMedsInMemory);
        }

        public override void OnAppearing()
        {
            CheckHasValuesChanged();
            FillFavoriteMedications();
        }

        private void FillFavoriteMedications()
        {
            var favoriteMeds = Resolver
                .Get<IFavoriteMedicationsDataAccess>()
                .SelectAllItems();

            var listFavMeds = new List<GroupFavoriteMedications>();

            foreach (var med in favoriteMeds)
            {
                if (!listFavMeds.Any(x => x.GroupName == med.GroupName))
                {
                    var list = _medicationDataAccess
                        .GetFavoriteMedications(Utils.ConvertToTableSchema(med.GroupName));

                    if (list != null && list.Any())
                        listFavMeds.Add(new GroupFavoriteMedications(med.GroupName, list));
                }
            }

            FavoriteMedications = new ObservableCollection<GroupFavoriteMedications>(listFavMeds);
            GroupAuxList = new List<GrupoNomesDTO>(GrupoNomes);

            ExecuteCalculationFavoriteMed();
        }

        private void SetFavoriteMedsInMemory()
        {
            var favoriteMeds = Resolver
                .Get<IFavoriteMedicationsDataAccess>()
                .SelectAllItems();

            foreach (var med in favoriteMeds)
            {
                if (!FavMedications.Any(x => x.IdGrupo == med.IdGrupo))
                {
                    var favMeds = _medicationDataAccess
                        .GetFavoriteMedications(Utils.ConvertToTableSchema(med.GroupName));

                    FavMedications.AddRange(favMeds);
                }
            }
        }

        private void ExecuteCalculationFavoriteMed()
        {
            foreach (var item in FavoriteMedications)
                base.ExecuteCalculation(item);
        }

        public override List<Medicamento> GetMedications(GrupoNomesDTO grupo)
        {
            return _medicationDataAccess
                .GetFavoriteMedications(Utils.ConvertToTableSchema(grupo.NomeGrupo));
        }

        protected override void FavotireIconClicked(object item)
        {
            base.FavotireIconClicked(item);

            if (!((Medicamento)item).IsFavorite)
            {
                GroupFavoriteMedications model = FavoriteMedications
                    .FirstOrDefault(x => x.Contains((Medicamento)item));

                if (model.Count > 1)
                    model.Remove((Medicamento)item);
                else
                    FavoriteMedications.Remove(model);
            }
        }
    }
}

