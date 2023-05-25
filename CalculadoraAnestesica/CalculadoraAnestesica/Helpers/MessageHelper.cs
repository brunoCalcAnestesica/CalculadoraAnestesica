using System;
namespace CalculadoraAnestesica.Helpers
{
	public class MessageHelper
	{
		public static void ShowErrorMessage()
		{
			NavigationHelper.GetPage()
				.DisplayAlert(
					"Atenção",
					"Ocorreu um erro inesperado, tente novamente",
					"Ok"
				);
		}
	}
}

