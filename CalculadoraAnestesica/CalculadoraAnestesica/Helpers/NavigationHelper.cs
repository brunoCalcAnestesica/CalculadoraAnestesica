using System;
using System.Linq;
using Xamarin.Forms;

namespace CalculadoraAnestesica.Helpers
{
	public class NavigationHelper
	{
		public static Page GetPage()
		{
            if (Application.Current.MainPage is null)
                return null;

            var navigationStack = Application.Current
                .MainPage
                .Navigation
                .NavigationStack;

            if (navigationStack.Any())
            {
                var lastPage = navigationStack.Last();

                return lastPage is TabbedPage tabbedPage
                    ? tabbedPage.CurrentPage
                    : lastPage;
            }

            return Application.Current.MainPage;
        }
	}
}

