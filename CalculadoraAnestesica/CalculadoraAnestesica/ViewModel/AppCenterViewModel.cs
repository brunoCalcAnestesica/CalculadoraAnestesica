using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CalculadoraAnestesica.ApplicationHandler;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.ViewModel.Base;
using CalculadoraAnestesica.ViewModel.DependencyInjection;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Xamarin.Forms;

namespace CalculadoraAnestesica.ViewModel
{
	public class AppCenterViewModel : MedicationsViewModelBase, IAppCenterViewModel
    {
        private bool searchBarVisible;
        public bool SearchBarVisible
        {
            get { return searchBarVisible; }
            set
            {
                searchBarVisible = value;
                RaisePropertyChanged();
            }
        }

        public AppCenterViewModel() : base()
        {
            SetFirstAccess();
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
                var medicamentos = Resolver.Get<IMedicamentosDataAccess>()
                    .GetMedicamentosById(grupoNome, id);

                foreach (var med in medicamentos)
                {
                    (double result1, double? result2) = Calculate(
                        med.DosagemMedicamento, peso
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

        private void ExecuteFilter()
        {
            var tables = GetGrupoNomes();
        }
    }
}

