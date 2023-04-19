using System;
using System.Collections.Generic;
using CalculadoraAnestesica.DbContext.Tables;
using System.Reflection;
using CalculadoraAnestesica.View.Intercaces;
using HtmlAgilityPack;
using Xamarin.Forms;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.Model;
using CalculadoraAnestesica.ApplicationHandler;
using CalculadoraAnestesica.ViewModel;
using CalculadoraAnestesica.View.Base;
using CalculadoraAnestesica.ViewModel.Interfaces;
using Syncfusion.XForms.TabView;
using CalculadoraAnestesica.ViewModel.DependencyInjection;
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.Effects;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraAnestesica.Converters;
using Xamarin.CommunityToolkit.UI.Views;

namespace CalculadoraAnestesica.View
{	
	public partial class AppCenterView : ViewBase, IAppCenterView
    {
        Image ImageButton;

        public AppCenterView()
        {
			InitializeComponent();
        }

        public override void AddComponents()
        {
            base.AddComponents();
            CreateTabView();
        }

        private void CreateTabView()
        {
            var viewModel = BindingContext as IAppCenterViewModel;
            var grupoMedicamentos = viewModel.GetGrupoNomes();

            for (int i = 0; i < grupoMedicamentos.Count; i++)
            {
                string identifier = grupoMedicamentos[i].NomeGrupo.ToString();
                string grupoNome = grupoMedicamentos[i].NomeGrupo;

                var label = new Label
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    Text = grupoNome,
                    FontSize = 15,
                    TextColor = Color.Black
                };

                var imageArrow = new Image
                {
                    Source = "arrow_icon.png",
                    HorizontalOptions = LayoutOptions.End,
                    HeightRequest = 30,
                    WidthRequest = 30
                };
                imageArrow.AutomationId = identifier;

                var grid = new Grid();
                grid.Children.Add(label);
                grid.Children.Add(imageArrow);

                var frame = new Frame
                {
                    CornerRadius = 15,
                    HasShadow = false,
                    Padding = 10,
                    BackgroundColor = Color.White,
                    Content = grid
                };
                frame.AutomationId = identifier;

                var collectionView = CreateContent();
                collectionView.IsVisible = false;
                collectionView.AutomationId = identifier;

                expanderItems.Children.Add(frame);
                expanderItems.Children.Add(collectionView);

                var tap = new TapGestureRecognizer();
                tap.Tapped += Tap_Tapped;
                frame.GestureRecognizers.Add(tap);
            }
        }

        Xamarin.Forms.View LastCollectionViewVisible = null;
        string lastSelectedIdentifier = "";

        private void Tap_Tapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            string id = frame.AutomationId;

            if (lastSelectedIdentifier == id)
            {
                CloseGroup(frame);
                return;
            }

            OpenGroup(frame);
        }

        private void CloseGroup(Frame frame)
        {
            string id = frame.AutomationId;

            LastCollectionViewVisible.IsVisible = false;
            var content = frame.Content;

            var image2 = ((Grid)content).Children
                .FirstOrDefault(
                    item => item is Image &&
                    item.AutomationId == id
                );

            Task.Run(async () => await image2.RotateTo(0));
        }

        private async void OpenGroup(Frame frame)
        {
            string id = frame.AutomationId;
            var content = frame.Content;

            Image image = (Image)((Grid)content).Children
                .FirstOrDefault(
                    item => item is Image &&
                    item.AutomationId == id
                );

            var view = expanderItems.Children
                .FirstOrDefault(
                    item => item is CollectionView &&
                    item.AutomationId == id
                );

            await Task.Run(async () => await SetGroupItems(image, view, id));
            view.IsVisible = true;

            LastCollectionViewVisible = view;
            lastSelectedIdentifier = id;
        }

        private async Task SetGroupItems(Image image,
            Xamarin.Forms.View view, string id)
        {
            await image.RotateTo(-180);

            var viewModel = BindingContext as IAppCenterViewModel;
            var source = viewModel.SetMedicamentos(ConvertToTableSchema(id), 0, GetDoubleFromPeso());

            Device.BeginInvokeOnMainThread(() =>
            {
                ((CollectionView)view).ItemsSource = source;
                ((CollectionView)view).HeightRequest = 60 * source.Count;
            });
        }

        private string ConvertToTableSchema(string name)
        {
            switch (name)
            {
                case TablesSchema.ANALGESICOS:
                    return nameof(TablesSchema.ANALGESICOS);

                case TablesSchema.ANESTESICOS_LOCAIS:
                    return nameof(TablesSchema.ANESTESICOS_LOCAIS);

                case TablesSchema.ANTIBIOTICOS:
                    return nameof(TablesSchema.ANTIBIOTICOS);

                case TablesSchema.ANTIEMETICOS:
                    return nameof(TablesSchema.ANTIEMETICOS);

                case TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR:
                    return nameof(TablesSchema.BLOQUEADORES_NEURO_MUSUCLAR);

                case TablesSchema.BLOQUEIOS_LOCOREGIONA:
                    return nameof(TablesSchema.BLOQUEIOS_LOCOREGIONA);

                case TablesSchema.CARDIO:
                    return nameof(TablesSchema.CARDIO);

                case TablesSchema.DIVESOS:
                    return nameof(TablesSchema.DIVESOS);

                case TablesSchema.DROGAS_DE_EMERGENCIA:
                    return nameof(TablesSchema.DROGAS_DE_EMERGENCIA);

                case TablesSchema.INALATORIOS:
                    return nameof(TablesSchema.INALATORIOS);

                case TablesSchema.INDUCAO:
                    return nameof(TablesSchema.INDUCAO);

                case TablesSchema.INFUSOES:
                    return nameof(TablesSchema.INFUSOES);

                case TablesSchema.OBSTETRICOS:
                    return nameof(TablesSchema.OBSTETRICOS);

                case TablesSchema.PRE_MEDICACAO:
                    return nameof(TablesSchema.PRE_MEDICACAO);

                case TablesSchema.PRE_MEDICACAO_IM_DART:
                    return nameof(TablesSchema.PRE_MEDICACAO_IM_DART);

                case TablesSchema.SEQUENCIAL_RAPIDO:
                    return nameof(TablesSchema.SEQUENCIAL_RAPIDO);

                case TablesSchema.VASOPRESSORES_DE_ACAO_CURTA:
                    return nameof(TablesSchema.VASOPRESSORES_DE_ACAO_CURTA);
            }

            return "";
        }

        private Xamarin.Forms.View CreateContent()
        {
            CollectionView collectionView = new CollectionView();
            collectionView.ItemTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid();
                grid.Margin = new Thickness(0, 10, 0, 0);
                //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
                //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 200 });
                //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                ImageButton imageFav = new ImageButton();
                imageFav.HeightRequest = 40;
                imageFav.WidthRequest = 40;
                imageFav.HorizontalOptions = LayoutOptions.Start;
                imageFav.BackgroundColor = Color.Transparent;
                imageFav.SetBinding(
                    Image.SourceProperty,
                    "IsFavorite",
                    BindingMode.TwoWay,
                    new BoolToIconConverter()
                );
                imageFav.Clicked += ImageFav_Clicked;
                IconTintColorEffect.SetTintColor(imageFav, Color.Green);

                StackLayout stackLayout = new StackLayout();
                stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                //stackLayout.Orientation = StackOrientation.Horizontal;
                stackLayout.Margin = new Thickness(50, 10, 0, 0);

                Label label = new Label();
                label.LineBreakMode = LineBreakMode.WordWrap;
                label.HorizontalTextAlignment = TextAlignment.Start;
                label.FontAttributes = FontAttributes.Bold;
                label.SetBinding(Label.TextProperty, "NomeMedicamento");
                label.TextColor = Color.FromHex("#006FCE");
                label.FontSize = 13;

                Label labelDosagem = new Label();
                labelDosagem.LineBreakMode = LineBreakMode.WordWrap;
                labelDosagem.HorizontalTextAlignment = TextAlignment.Start;
                labelDosagem.SetBinding(Label.TextProperty, "DosagemMedicamento");
                labelDosagem.TextColor = Color.FromHex("#006FCE");
                labelDosagem.FontSize = 12;

                stackLayout.Children.Add(label);
                stackLayout.Children.Add(labelDosagem);

                Label labelResultado = new Label();
                labelResultado.VerticalTextAlignment = TextAlignment.Center;
                labelResultado.HorizontalTextAlignment = TextAlignment.End;
                labelResultado.SetBinding(Label.TextProperty, "Resultado");
                labelResultado.TextColor = Color.FromHex("#003766");
                labelResultado.FontSize = 13;
                labelResultado.Margin = new Thickness(0, 0, 0, 0);

                grid.Children.Add(imageFav, 0, 0);
                grid.Children.Add(stackLayout, 0, 0);
                grid.Children.Add(labelResultado, 0, 0);

                return grid;
            });
            return collectionView;
        }

        private void ImageFav_Clicked(object sender, EventArgs e)
        {
            var model = (Medicamento)((ImageButton)sender).BindingContext;
            model.IsFavorite = !model.IsFavorite;
        }

        double GetDoubleFromPeso()
        {
            return double.Parse(entryPeso.Text.Replace("KG", "").Trim());
        }
    }
}

