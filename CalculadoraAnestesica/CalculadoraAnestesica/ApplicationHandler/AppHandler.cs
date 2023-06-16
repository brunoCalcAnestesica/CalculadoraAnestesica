using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.Shared;
using Xamarin.Essentials;

namespace CalculadoraAnestesica.ApplicationHandler
{
	public static class AppHandler
	{
        public static void AppStart()
        {
            LoadDatabase();
            LoadMedicationsListGlobal();
        }

        private static void LoadMedicationsListGlobal()
        {
            Task.Run(() =>
            {
                var grupos = Resolver
                   .Get<IMedicamentosDataAccess>()
                   .GetGrupoNomes();

                foreach (var grupo in grupos)
                {
                    var med = Resolver
                        .Get<IMedicamentosDataAccess>()
                        .GetMedicamento(Utils.ConvertToTableSchema(grupo.NomeGrupo));

                    if (AppSource.MedicamentosList is null)
                        AppSource.MedicamentosList = new List<Model.Medicamento>();

                    AppSource.MedicamentosList.AddRange(med);
                }
            });
        }

        private static void LoadDatabase()
        {
            if (!File.Exists(PlatformPaths.Dbpath))
                DatabaseHandler.Instance.CreateAppDatabase();

            DatabaseHandler.Instance.InitDatabase();
        }

        public static void CreateIoC()
        {
            Resolver.Build();
        }

        public static bool IsFirstAccess()
        {
            return Resolver.Get<IUserContextDataAccess>()
                .SelectAllItems()
                .Count == 0;
        }
    }
}

