using System;
using CalculadoraAnestesica.Model;
using System.Collections.Generic;

namespace CalculadoraAnestesica.ViewModel.Interfaces
{
	public interface IAppCenterViewModel
	{
        List<GrupoNomesDTO> GetGrupoNomes();
        void SetGrupoMedicamento(string grupoMedicamento);
        List<Medicamento> SetMedicamentos(string grupoNome, int id, double peso);
    }
}

