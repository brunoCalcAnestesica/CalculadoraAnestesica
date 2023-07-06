using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CalculadoraAnestesica.View.Base;
using CalculadoraAnestesica.View.Intercaces;
using CalculadoraAnestesica.ViewModel;
using ExcelDataReader;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace CalculadoraAnestesica.View
{	
	public partial class HemodinamicoCalcView : ViewBase, IHemodinamicoCalcView
    {	
		public HemodinamicoCalcView()
		{
			InitializeComponent();
            inputsCalculation.CalculationCallback = SetPickerFocus;
        }

        void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var binding = (HemodinamicoCalcViewModel)BindingContext;
            binding.ExecuteSearch.Execute(null);
        }

        void SetPickerFocus()
        {
            picker.Focus();
        }

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            picker.Focus();
        }
    }
}

