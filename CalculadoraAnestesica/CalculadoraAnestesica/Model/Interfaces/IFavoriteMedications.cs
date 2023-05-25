using System;
namespace CalculadoraAnestesica.Model.Interfaces
{
	public interface IFavoriteMedications
	{
        int Id { get; set; }
        int MedicationId { get; set; }
        int IdGrupo { get; set; }
        string GroupName { get; set; }
        string MedicationName { get; set; }
    }
}

