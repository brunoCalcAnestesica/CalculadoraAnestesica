using System;
using CalculadoraAnestesica.iOS.Renderes;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace CalculadoraAnestesica.iOS.Renderes
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public override void ViewWillAppear(bool animated)
        {
            if (Element is TabbedPage tabs)
            {
                //tabs.UnselectedTabColor = Color.Gray;
                //tabs.SelectedTabColor = Color.Black;

                TabBar.BackgroundColor = UIColor.White;
                //TabBar.SelectedImageTintColor = UIColor.Black;

                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UpdateTabBarItem(TabBar.Items[i], tabs.Children[i].Icon);
                }
            }

            base.ViewWillAppear(animated);
        }

        private void TabBar_ItemSelected(object sender, UITabBarItemEventArgs e)
        {

        }

        private void UpdateTabBarItem(UITabBarItem item, string icon)
        {
            if (item == null)
                return;

            var textAtt = new UITextAttributes
            {
                Font = UIFont.SystemFontOfSize(25F)
            };

            item.SetTitleTextAttributes(textAtt, UIControlState.Application);
            item.Image = ScalingImageToSize(item.Image, new CGSize(55, 55));
        }

        public UIImage ScalingImageToSize(UIImage sourceImage, CGSize newSize)
        {

            try
            {
                if (UIScreen.MainScreen.Scale == 2.0) //@2x iPhone 6 7 8 
                {
                    UIGraphics.BeginImageContextWithOptions(newSize, false, 2.0f);
                }

                else if (UIScreen.MainScreen.Scale == 3.0) //@3x iPhone 6p 7p 8p...
                {
                    UIGraphics.BeginImageContextWithOptions(newSize, false, 3.0f);
                }

                else
                {
                    UIGraphics.BeginImageContext(newSize);
                }

                sourceImage.Draw(new CGRect(0, 0, newSize.Width, newSize.Height));

                UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();

                UIGraphics.EndImageContext();

                return newImage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

