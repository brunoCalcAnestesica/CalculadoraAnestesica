using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CalculadoraAnestesica.Components
{
	public class InputsCalculation : Grid
	{
        public static readonly BindableProperty EntryWeightTextProperty = BindableProperty.Create(
            nameof(EntryWeightText),
            typeof(string),
            typeof(InputsCalculation),
            default,
            BindingMode.TwoWay,
            propertyChanged: EntryWeightTextPropertyChanged
        );

        public static readonly BindableProperty EntryAgeTextProperty = BindableProperty.Create(
            nameof(EntryAgeText),
            typeof(string),
            typeof(InputsCalculation),
            default,
            BindingMode.TwoWay,
            propertyChanged: EntryAgeTextPropertyChanged
        );

        public static readonly BindableProperty SelectedLabelProperty = BindableProperty.Create(
            nameof(SelectedLabel),
            typeof(string),
            typeof(InputsCalculation),
            default,
            BindingMode.TwoWay,
            propertyChanged: SelectedLabelPropertyChanged
        );

        private static void EntryWeightTextPropertyChanged(BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var component = (InputsCalculation)bindable;
            component.EntryWeight.Text = (string)newValue;
        }

        private static void EntryAgeTextPropertyChanged(BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var component = (InputsCalculation)bindable;
            component.EntryAge.Text = (string)newValue;
        }

        private static void SelectedLabelPropertyChanged(BindableObject bindable,
           object oldValue,
           object newValue)
        {
            var component = (InputsCalculation)bindable;
            component.Label.Text = (string)newValue;
        }

        public string EntryWeightText
        {
            get { return (string)GetValue(EntryWeightTextProperty); }
            set { SetValue(EntryWeightTextProperty, value); }
        }

        public string EntryAgeText
        {
            get { return (string)GetValue(EntryAgeTextProperty); }
            set { SetValue(EntryAgeTextProperty, value); }
        }

        public string SelectedLabel
        {
            get { return (string)GetValue(SelectedLabelProperty); }
            set { SetValue(SelectedLabelProperty, value); }
        }

        public Action CalculationCallback { get; set; }
        public Entry EntryAge { get; set; }
        public Entry EntryWeight { get; set; }
        public Label Label { get; set; }

        public InputsCalculation()
		{
            ColumnSpacing = 75;

			RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Margin = new Thickness(0,15,0,0);
			HorizontalOptions = LayoutOptions.Center;
			VerticalOptions = LayoutOptions.Center;

            StackLayout stkAge = new StackLayout();
            stkAge.Margin = new Thickness(0, 0, -60, 0);
            stkAge.Orientation = StackOrientation.Horizontal;
            Children.Add(stkAge, 0, 1);

            EntryAge = new Entry();
			EntryAge.TextColor = Color.FromHex("#005299");
			EntryAge.Placeholder = "Idade...";
            EntryAge.PlaceholderColor = Color.Gray;
            EntryAge.Keyboard = Keyboard.Numeric;
			EntryAge.HorizontalTextAlignment = TextAlignment.Center;
			EntryAge.WidthRequest = 85;
            EntryAge.TextChanged += EntryAge_TextChanged;
            stkAge.Children.Add(EntryAge);

            Label = new Label();
            Label.VerticalTextAlignment = TextAlignment.Center;
            Label.TextColor = Color.FromHex("#005299");
            Label.FontSize = 15;
            stkAge.Children.Add(Label);

            StackLayout stackLayout = new StackLayout();
			stackLayout.Margin = new Thickness(0, 0, -50, 0);
			stackLayout.Orientation = StackOrientation.Horizontal;
            Children.Add(stackLayout, 1, 1);

            EntryWeight = new Entry();
            EntryWeight.TextColor = Color.FromHex("#005299");
            EntryWeight.Placeholder = "Peso...";
            EntryWeight.PlaceholderColor = Color.Gray;
            EntryWeight.Keyboard = Keyboard.Numeric;
            EntryWeight.HorizontalTextAlignment = TextAlignment.Center;
            EntryWeight.WidthRequest = 75;
            EntryWeight.TextChanged += EntryWeight_TextChanged;
			stackLayout.Children.Add(EntryWeight);

			Label labelKg = new Label();
			labelKg.VerticalTextAlignment = TextAlignment.Center;
			labelKg.Text = "KG";
			labelKg.TextColor = Color.FromHex("#005299");
			labelKg.FontSize = 15;
            stackLayout.Children.Add(labelKg);
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationCallback?.Invoke();
        }

        private void EntryWeight_TextChanged(object sender,
            TextChangedEventArgs e)
        {
            if (e.OldTextValue != e.NewTextValue)
            {
                string newValue = e.NewTextValue ?? "";
                string newTextValue = new string(newValue.Where(char.IsDigit).ToArray());

                if (newValue.Contains(",,"))
                    newValue = newValue.Replace(",,", ",");

                if (newValue.Contains(".."))
                    newValue = newValue.Replace("..", ".");

                if (newValue == "0" || string.IsNullOrEmpty(newValue))
                    return;
                    
                if (newTextValue.Length >= 3)
                {
                    if (!newValue.Contains(".") || !newValue.Contains(","))
                    {
                        decimal val = decimal.Parse(newTextValue) / 10.0m;

                        if (val > 99m)
                        {
                            EntryWeightText = "99";
                        }
                        else
                        {
                            newValue = val.ToString().Substring(0, 4);
                        }
                    }

                    EntryWeightText = newValue.Substring(0, 4);
                    CalculationCallback?.Invoke();
                    return;
                }

                EntryWeightText = newValue;
            }
        }

        private void EntryAge_TextChanged(object sender,
            TextChangedEventArgs e)
        {
            if (e.OldTextValue != e.NewTextValue)
            {
                string newValue = e.NewTextValue ?? "0";
                string text = new string(newValue.Where(char.IsDigit).ToArray());

                if (newValue.Contains(","))
                    newValue = newValue.Replace(",", "");

                if (newValue.Contains("."))
                    newValue = newValue.Replace(".", "");

                if (newValue == "0" || string.IsNullOrEmpty(newValue))
                    return;

                if (text.Length >= 2)
                {
                    EntryAgeText = newValue.Substring(0, 2);
                    EntryAge.Text = newValue.Substring(0, 2);
                    CalculationCallback?.Invoke();
                    EntryWeight.Focus();
                    return;
                }

                EntryAgeText = newValue;
            }
        }
    }
}

