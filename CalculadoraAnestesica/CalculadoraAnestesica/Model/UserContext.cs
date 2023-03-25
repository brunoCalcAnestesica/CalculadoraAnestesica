using System;
using CalculadoraAnestesica.Model.Interfaces;
using SQLite;

namespace CalculadoraAnestesica.Model
{
	public class UserContext : IUserContext
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
		public bool IsFirstAccess { get; set; }
	}
}

