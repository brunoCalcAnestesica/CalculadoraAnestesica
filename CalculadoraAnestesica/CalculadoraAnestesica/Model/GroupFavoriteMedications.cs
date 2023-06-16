using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CalculadoraAnestesica.Model
{
	public class GroupFavoriteMedications : ObservableCollection<Medicamento>
    {
		public string GroupName { get; set; }

		public GroupFavoriteMedications(string nomeGrupo, List<Medicamento>
			medicamentos) : base(medicamentos)
		{
			GroupName = nomeGrupo;
        }
	}
}	

