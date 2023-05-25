using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CalculadoraAnestesica.View.Base;
using CalculadoraAnestesica.View.Intercaces;
using CalculadoraAnestesica.ViewModel;
using ExcelDataReader;
using Xamarin.Forms;

namespace CalculadoraAnestesica.View
{	
	public partial class HemodinamicoCalcView : ViewBase, IHemodinamicoCalcView
    {	
		public HemodinamicoCalcView()
		{
			InitializeComponent();
        }

        void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var binding = (HemodinamicoCalcViewModel)BindingContext;
            binding.ExecuteSearch.Execute(null);
        }
    }
}

