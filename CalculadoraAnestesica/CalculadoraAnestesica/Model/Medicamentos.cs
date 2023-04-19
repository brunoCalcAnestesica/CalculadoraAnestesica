using System;
using System.Collections.Generic;

namespace CalculadoraAnestesica.Model
{
	public class Medicamentos : List<Medicamento>
	{
		public string GrupoMedicamento { get; set; }

		public Medicamentos(string grupoMedicamento, List<Medicamento> medicamentos) : base(medicamentos)
		{
            GrupoMedicamento = grupoMedicamento;
        }
	}
}

