using System;
using CalculadoraAnestesica.Model.Base;

namespace CalculadoraAnestesica.Model
{
	public class HemodinamicoResult : ModelBase
	{
		public string Text { get; set; }

		private string value;
        public string Value
		{
			get { return value; }
			set
			{
				this.value = value; 
				OnPropertyChanged();
			}
		}

		public HemodinamicoResult(string text, string value = "")
		{
			Text = text;
			Value = value;
		}
    }
}

