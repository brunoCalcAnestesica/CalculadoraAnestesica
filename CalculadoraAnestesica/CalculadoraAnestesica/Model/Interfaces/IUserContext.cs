using System;
namespace CalculadoraAnestesica.Model.Interfaces
{
	public interface IUserContext
	{
        int Id { get; set; }
        bool IsFirstAccess { get; set; }
    }
}

