using System;
using CalculadoraAnestesica.DataAccess.Base;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.DataAccess
{
	public class UserContextDataAcceess : DataAccessBase<IUserContext, UserContext>, IUserContextDataAccess
    {
	}
}

