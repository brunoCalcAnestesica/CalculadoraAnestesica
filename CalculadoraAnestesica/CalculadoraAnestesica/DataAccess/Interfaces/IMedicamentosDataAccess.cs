using System;
using System.Collections.Generic;
using CalculadoraAnestesica.DataAccess.Base.Interface;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.DataAccess.Interfaces
{
	public interface IMedicamentosDataAccess : IDataAccessBase<IMedicamento, Medicamento>
	{
        void InsertMedicamento(string tableName, string value, string descricao, int id_grupo);
        List<Medicamento> GetMedicamentos(string tableName);
        void CreateMedicamentosTables();
        List<Medicamento> GetMedicamento(string tableName);
        List<GrupoNomesDTO> GetGrupoNomes();
        List<Medicamento> GetMedicamentosById(string tableName, int id_grupo);
    }
}

    