using System;
using CalculadoraAnestesica.Model.Interfaces;
using SQLite;

namespace CalculadoraAnestesica.Model
{
	public class FavoriteMedications : IFavoriteMedications
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string GroupName { get; set; }
        public string MedicationName { get; set; }
        public int MedicationId { get; set; }
    }
}

