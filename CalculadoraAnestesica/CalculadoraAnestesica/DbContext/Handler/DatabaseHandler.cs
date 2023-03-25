using System;
using System.Collections.Generic;
using System.Linq;
using CalculadoraAnestesica.Model;
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

        public void CreateDatabase(string dbPath)
		{
            SQLiteConnectionString connection = new SQLiteConnectionString(dbPath);
            Connection = new SQLiteConnection(connection);
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

