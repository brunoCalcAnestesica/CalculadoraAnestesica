using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CalculadoraAnestesica.Components
{
	public class HeaderApp : StackLayout
	{
        public static readonly BindableProperty IsSearchViewVisibleProperty = BindableProperty.Create(
            nameof(IsSearchViewVisible),
            typeof(bool),
            typeof(HeaderApp),
            default,
            BindingMode.TwoWay,
            propertyChanged: IsSearchViewVisiblePropertyChanged
        );

        public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(
			nameof(IsHeaderVisible),
			typeof(bool),
			typeof(HeaderApp),
            default,
			BindingMode.TwoWay,
			propertyChanged: IsHeaderVisiblePropertyChanged
        );

        public static readonly BindableProperty SearchIconCommandProperty = BindableProperty.Create(
            nameof(SearchIconCommand),
            typeof(ICommand),
            typeof(HeaderApp),
            default,
            BindingMode.TwoWay,
            propertyChanged: null
        );

        public static readonly BindableProperty SearchBarTextProperty = BindableProperty.Create(
            nameof(SearchBarText),
            typeof(string),
            typeof(HeaderApp),
            default,
            BindingMode.TwoWay,
            propertyChanged: SearchBarTextPropertyChanged
        );

        private static void IsSearchViewVisiblePropertyChanged(BindableObject bindable,
			object oldValue,
			object newValue)
        {
            var component = bindable as HeaderApp;
            var value = (bool)newValue;

			component.Frame.IsVisible = value;
			component.Button.IsVisible = value;
        }

        private static void IsHeaderVisiblePropertyChanged(BindableObject bindable,
			object oldValue,
			object newValue)
        {
            var component = bindable as HeaderApp;
			var value = (bool)newValue;

			component.Label1.IsVisible = value;
            component.Image.IsVisible = value;
            component.SearchIcon.IsVisible = value;
            component.Label1.IsVisible = value;
        }

        private static void SearchBarTextPropertyChanged(BindableObject bindable,
			object oldValue,
			object newValue)
        {
            var component = bindable as HeaderApp;
			component.SearchBar.Text = (string)newValue;
        }

        public bool IsSearchViewVisible
        {
            get { return (bool)GetValue(IsHeaderVisibleProperty); }
            set { SetValue(IsHeaderVisibleProperty, value); }
        }

        public bool IsHeaderVisible
        {
			get { return (bool)GetValue(IsHeaderVisibleProperty); }
			set { SetValue(IsHeaderVisibleProperty, value); }
		}

        public string SearchBarText
        {
            get { return (string)GetValue(SearchBarTextProperty); }
            set { SetValue(SearchBarTextProperty, value); }
        }

        public ICommand SearchIconCommand
        {
            get { return (ICommand)GetValue(SearchIconCommandProperty); }
            set { SetValue(SearchIconCommandProperty, value); }
        }

		private bool useSearchBar;
		public bool UseSearchBar
		{
			get { return useSearchBar; }
			set
			{
				useSearchBar = value;
				SearchIcon.IsVisible = value;
            }
		}

        public ImageButton SearchIcon;
        public SearchBar SearchBar;
		public Label Label1;
		public Image Image;
		public Frame Frame;
		public Button Button;

        public HeaderApp()
		{
            if (Device.RuntimePlatform == Device.iOS)
                Margin = new Thickness(0, -60, 0, 0);

			HeightRequest = 85;

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush
			{
				EndPoint = new Point(1.20, 0),
				GradientStops = new GradientStopCollection
				{
					new GradientStop
					{
						Color = Color.FromHex("#006FCE"),
						Offset = 0
					},
					new GradientStop
					{
                        Color = Color.FromHex("#00FFF5"),
						Offset = 1
                    }
				}
			};
			Background = linearGradientBrush;

			Grid grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			grid.HorizontalOptions = LayoutOptions.Fill;
			grid.VerticalOptions = LayoutOptions.Fill;

            if (Device.RuntimePlatform == Device.iOS)
                grid.Margin = new Thickness(0, 50, 0, 0);
            else
                grid.Margin = new Thickness(0, 40, 0, 0);

            Label1 = new Label();
			Label1.FontAttributes = FontAttributes.Bold;
			Label1.Text = "MedCalc";
			Label1.TextColor = Color.White;
			Label1.VerticalOptions = LayoutOptions.Start;
			Label1.FontSize = 27;
			Label1.CharacterSpacing = 1;

			Label1.Margin = Device.RuntimePlatform == Device.iOS
				? new Thickness(10, 17.5, 0, 0)
				: new Thickness(10, 0, 0, 0);

            Image = new Image();
			Image.Source = "logo_icon.png";
			Image.HeightRequest = 45;
			Image.WidthRequest = 45;
			Image.HorizontalOptions = LayoutOptions.Center;
			Image.VerticalOptions = LayoutOptions.Center;

			Image.Margin = Device.RuntimePlatform == Device.iOS
				? new Thickness(15, 12, 0, 0)
				: new Thickness(15, 0, 0, 0);

            SearchIcon = new ImageButton();
			SearchIcon.Source = "search_icon.png";
			SearchIcon.BackgroundColor = Color.Transparent;
			SearchIcon.HeightRequest = 55;
			SearchIcon.WidthRequest = 55;
			SearchIcon.HorizontalOptions = LayoutOptions.End;
			SearchIcon.VerticalOptions = LayoutOptions.Center;
			SearchIcon.Clicked += ClickedEvent;

			SearchIcon.Margin = Device.RuntimePlatform == Device.iOS
				? new Thickness(0, 5, 10, 0)
				: new Thickness(0, -5, 10, 0);

            Frame = new Frame();
			Frame.BackgroundColor = Color.White;
			Frame.HasShadow = false;
			Frame.CornerRadius = 15;
			Frame.IsVisible = false;

			Frame.Margin = Device.RuntimePlatform == Device.iOS
				? new Thickness(5, 10, 85, 20)
				: new Thickness(5, 0, 110, 20);

            SearchBar = new SearchBar();
			SearchBar.Margin = -15;
            SearchBar.TextChanged += SearchBar_TextChanged;

			Frame.Content = SearchBar;

			Button = new Button();
			Button.VerticalOptions = LayoutOptions.Center;
			Button.HorizontalOptions = LayoutOptions.End;
			Button.Text = "Cancelar";
			Button.TextColor = Color.White;
			Button.BackgroundColor = Color.Transparent;
			Button.FontAttributes = FontAttributes.Bold;
			Button.FontSize = 17;
            Button.Clicked += ClickedEvent;
			Button.IsVisible = false;

			Button.Margin = Device.RuntimePlatform == Device.iOS
				? new Thickness(0, 0, 10, 10)
				: new Thickness(0, 0, 5, 15);

            grid.Children.Add(Label1);
            grid.Children.Add(Image);
			grid.Children.Add(SearchIcon);
            grid.Children.Add(Frame);
            grid.Children.Add(Button);

			Children.Add(grid);
        }

        private void SearchBar_TextChanged(object sender,
			TextChangedEventArgs e)
        {
			SearchBarText = e.NewTextValue;
        }

        private void ClickedEvent(object sender, EventArgs e)
        {
			SearchIconCommand?.Execute(null);
        }
    }
}

