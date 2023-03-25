using System;
using System.Collections.Generic;
using CalculadoraAnestesica.DataAccess.Base.Interface;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.DataAccess.Interfaces
{
	public interface IMedicamentosDataAccess : IDataAccessBase<IMedicamento, Medicamento>
	{
        void InsertMedicamento(string tableName, string value, string descricao);
        List<Medicamento> GetMedicamentos(string tableName);
        void CreateMedicamentosTables();
        List<Medicamentos> GetAllMedicamentos();
    }
}

