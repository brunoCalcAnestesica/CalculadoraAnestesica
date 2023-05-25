using System;
namespace CalculadoraAnestesica.Model
{
	public class GrupoNomesDTO
	{
		public int Id { get; set; }
		public string NomeGrupo { get; set; }
        public bool IsSelected { get; set; }
        public double Rotation { get; set; }
    }
}

