using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.Shared;
using SQLite;

namespace CalculadoraAnestesica.DbContext.Tables
{
	public class DatabaseHandler
	{
        private static DatabaseHandler instance;
        public static DatabaseHandler Instance
        {
            get
            {
                if (instance is null)
                    instance = new DatabaseHandler();

                return instance;
            }
        }

        public SQLiteConnection Connection { get; set; }

        public void InitDatabase()
		{
            SQLiteConnectionString connection = new SQLiteConnectionString(PlatformPaths.Dbpath);
            Connection = new SQLiteConnection(connection);

            CreateTables(new List<Type>()
            {
                Resolver.GetType<IUserContext>(),
                Resolver.GetType<IFavoriteMedications>()
            });
        }

        public void CreateAppDatabase()
        {
            var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("CalculadoraAnestesica.EmbeddedResources.userdb.db");

            using (var fileStream = File.Create(PlatformPaths.Dbpath))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byte[] bytes = ms.ToArray();
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void CreateTables(IList<Type> tableTypes)
        {
            if (tableTypes.Any())
            {
                Connection.CreateTables(
                    CreateFlags.None,
                    tableTypes.ToArray()
                );
            }
        }

        public void CloseConnection()
        {
            Connection.Close();
        }
    }
}

