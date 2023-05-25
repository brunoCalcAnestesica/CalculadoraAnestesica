using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CalculadoraAnestesica.Model
{
	public class Medicamentos : ObservableCollection<Medicamento>
	{
		public string GrupoMedicamento { get; set; }
		public bool IsSelected { get; set; }
		public double Rotation { get; set; }

		public Medicamentos(string grupoMedicamento,
            ObservableCollection<Medicamento> medicamentos) : base(medicamentos)
		{
            GrupoMedicamento = grupoMedicamento;
        }
	}
}

