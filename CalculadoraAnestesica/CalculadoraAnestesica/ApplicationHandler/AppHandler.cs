using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.ApplicationHandler
{
	public static class AppHandler
	{
        public static void InitDatabase()
        {
            string pathAppData = Environment.GetFolderPath(Environment
                .SpecialFolder
                .LocalApplicationData
            );

            DatabaseHandler.Instance.CreateDatabase(
               dbPath: Path.Combine(pathAppData, "userdb")
            );
        }

        public static void AppStart()
        {
            DatabaseHandler
            .Instance
            .CreateTables(new List<Type>()
            {
                Resolver.GetType<IUserContext>()
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

