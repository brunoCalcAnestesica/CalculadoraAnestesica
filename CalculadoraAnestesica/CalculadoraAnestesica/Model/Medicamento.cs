using System;
using CalculadoraAnestesica.Model.Base;
using CalculadoraAnestesica.Model.Interfaces;

namespace CalculadoraAnestesica.Model
{
	public class Medicamento : ModelBase, IMedicamento
    {
		public int Id { get; set; }
		public string NomeMedicamento { get; set; }
        public string DosagemMedicamento { get; set; }
        public string DescricaoMedicamento { get; set; }
        public int IdGrupo { get; set; }
        public string Formula { get; set; }

        private string resultado;
        public string Resultado
        {
            get { return resultado; }
            set
            {
                if (resultado != value)
                {
                    resultado = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isFavorite;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                if (isFavorite != value)
                {
                    isFavorite = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

