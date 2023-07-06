using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
using CalculadoraAnestesica.Model;
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
                    var medications = Resolver
                        .Get<IMedicamentosDataAccess>()
                        .GetMedicamento(Utils.ConvertToTableSchema(grupo.NomeGrupo));

                    if (AppSource.MedicamentosList is null)
                        AppSource.MedicamentosList = new List<Model.Medicamento>();

                    if (AppSource.MedicamentosGroupList is null)
                        AppSource.MedicamentosGroupList = new List<Model.Medicamento>();

                    var list = new List<Medicamento>();

                    foreach (Medicamento med in medications)
                    {
                        med.DosagemMedicamento = med.DosagemMedicamento.Trim().TrimStart('-');
                        list.Add(med);
                    }

                    if (MedicamentosHelper.IsGroupedMedication(grupo.NomeGrupo))
                        AppSource.MedicamentosGroupList.AddRange(list);
                    else
                        AppSource.MedicamentosList.AddRange(list);
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

