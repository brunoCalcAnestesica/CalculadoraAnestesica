using System;
using CalculadoraAnestesica.iOS.Renderes;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(FixedSearchBarRenderer))]
namespace CalculadoraAnestesica.iOS.Renderes
{
    public class FixedSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && Control != null)
            {
                Control.SearchTextField.BackgroundColor = UIColor.FromRGB(255, 255, 255);
            }
        }
    }
}

