using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public FavoriteViewModel() : base()
        {
            FavMedications = new List<Medicamento>();
        }

        public override void OnAppearing()
        {
            var favoriteMeds = Resolver
                .Get<IFavoriteMedicationsDataAccess>()
                .SelectAllItems();

            Task.Run(() => SetFavoriteMedsInMemory(favoriteMeds));

            foreach (var med in favoriteMeds)
            {
                if (!GrupoNomes.Any(x => x.NomeGrupo == med.GroupName))
                {
                    GrupoNomes.Add(new GrupoNomesDTO
                    {
                        Id = med.IdGrupo,
                        NomeGrupo = med.GroupName
                    });
                }
            }

            AuxList = new List<GrupoNomesDTO>(GrupoNomes);
        }

        private void SetFavoriteMedsInMemory(IList<FavoriteMedications> favoriteMeds)
        {
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

        public override List<Medicamento> ExecuteCalculation(List<Medicamento> medicamentos)
        {
            var group =_medicationDataAccess
                .GetGrupoNomeById(medicamentos.FirstOrDefault().IdGrupo);

            var meds = GetMedications(new GrupoNomesDTO { NomeGrupo = group });
            return base.ExecuteCalculation(meds);
        }

        public override List<Medicamento> GetMedications(GrupoNomesDTO grupo)
        {
            return _medicationDataAccess
                .GetFavoriteMedications(Utils.ConvertToTableSchema(grupo.NomeGrupo));
        }
    }
}

