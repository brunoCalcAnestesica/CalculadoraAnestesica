using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Helpers;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.Shared;

namespace CalculadoraAnestesica.ApplicationHandler
{
	public static class AppHandler
	{
        public static void InitDatabase()
        {
            string pathAppData = Environment
                .GetFolderPath(Environment
                .SpecialFolder
                .LocalApplicationData
            );

            DatabaseHandler.Instance
              .CreateDatabase(Path.Combine(pathAppData, "userdb"));
        }

        public static void AppStart()
        {
            DatabaseHandler.Instance
              .CreateTables(new List<Type>()
              {
                  Resolver.GetType<IUserContext>(),
                  Resolver.GetType<IFavoriteMedications>()
              });

            if (IsFirstAccess())
            {
                Resolver
                 .Get<IMedicamentosDataAccess>()
                 .CreateMedicamentosTables();
            }
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

