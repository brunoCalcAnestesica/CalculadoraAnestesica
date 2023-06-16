using System;
using System.Collections.Generic;
using CalculadoraAnestesica.Controls;
using CalculadoraAnestesica.View.Intercaces;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace CalculadoraAnestesica.View
{
    public partial class TabbedPageView : Xamarin.Forms.TabbedPage
    {	
		public TabbedPageView ()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

            var hemodinamicoCalcView = NavigationControl.GetPage<IHemodinamicoCalcView, IHemodinamicoCalcViewModel>();
            hemodinamicoCalcView.Title = "Hemodinâmico";
            hemodinamicoCalcView.IconImageSource = "cardiogram.png";

            var appCenterView = NavigationControl.GetPage<IAppCenterView, IAppCenterViewModel>();
            appCenterView.Title = "Medicamentos";
            appCenterView.IconImageSource = "pill_icon.png";

            var favoriteView = NavigationControl.GetPage<IFavoriteMedicationsView, IFavoriteViewModel>();
            favoriteView.Title = "Favoritos";
            favoriteView.IconImageSource = "star_icon.png";

            Children.Add(hemodinamicoCalcView);
			Children.Add(appCenterView);    
            Children.Add(favoriteView);

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            BackgroundColor = Color.FromHex("#FFFFFF");
            SelectedTabColor = Color.FromHex("#006FCE");
            UnselectedTabColor = Color.Gray;
        }   
	}
}

