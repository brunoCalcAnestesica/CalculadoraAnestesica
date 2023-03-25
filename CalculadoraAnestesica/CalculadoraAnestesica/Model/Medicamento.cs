using System;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.Model
{
	public class Medicamento : IMedicamento
    {
		public int Id { get; set; }
		public string NomeMedicamento { get; set; }
        public string DescricaoMedicamento { get; set; }
    }
}

