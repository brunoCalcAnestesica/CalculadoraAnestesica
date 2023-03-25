using System;
using CalculadoraAnestesica.DataAccess.Base.Interface;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.DataAccess.Interfaces
{
	public interface IUserContextDataAccess : IDataAccessBase<IUserContext, UserContext>
	{

	}
}

