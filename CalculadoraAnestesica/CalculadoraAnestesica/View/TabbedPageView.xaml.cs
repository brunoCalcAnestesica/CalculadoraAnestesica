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

            var appCenterView = NavigationControl
                .GetPage<IAppCenterView, IAppCenterViewModel>();

            appCenterView.Title = "Cálculo";
            appCenterView.IconImageSource = "calc_icon.png";

			Children.Add(appCenterView);    
            Children.Add(new Page { Title = "Cálculo 2", IconImageSource = "calc_icon.png" });
            Children.Add(new Page { Title = "Favoritos", IconImageSource = "star_icon.png" });

			NavigationPage.SetHasNavigationBar(this, false);

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            BackgroundColor = Color.FromHex("#FFFFFF");
            SelectedTabColor = Color.FromHex("#006FCE");
            UnselectedTabColor = Color.Gray;
        }   
	}
}

