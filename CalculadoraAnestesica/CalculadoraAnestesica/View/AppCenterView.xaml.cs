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
using System.Collections.ObjectModel;
using CalculadoraAnestesica.ViewModel.Base;
using System.Text.RegularExpressions;

namespace CalculadoraAnestesica.View
{	
	public partial class AppCenterView : ViewBase, IAppCenterView
    {
        public bool IsFavoriteView { get; set; }

        public AppCenterView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        int lastIndex = 0;
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            MedicationsViewModelBase appCenterViewModel = BindingContext as MedicationsViewModelBase;
            var source = (GrupoNomesDTO)((Frame)sender).BindingContext;

            var parent = (StackLayout)((Frame)sender).Parent;
            CollectionView cView = (CollectionView)parent.FindByName("cView");

            bool newCollection = cView.ItemsSource is null;
            List<Medicamento> medications = null;

            if (appCenterViewModel.IsFiltering)
            {
                medications = appCenterViewModel
                    .GroupFilteredMedicamentosList
                    .Where(x => x.IdGrupo == source.Id)
                    .ToList();

                cView.ItemsSource = medications;
            }
            else if (newCollection && string.IsNullOrEmpty(appCenterViewModel.EntryWeight))
            {
                medications = appCenterViewModel.GetMedications(source);
                cView.ItemsSource = medications;
            }
            else if (newCollection && !string.IsNullOrEmpty(appCenterViewModel.EntryWeight))
            {
                medications = appCenterViewModel.GetMedications(source);
                var itemsSource = appCenterViewModel.ExecuteCalculation(medications);
                cView.ItemsSource = medications;
            }
            else
            {
                var list = cView.ItemsSource.Cast<Medicamento>();
                var itemsSource = appCenterViewModel.ExecuteCalculation(list.ToList());
                cView.ItemsSource = medications;
            }

            Grid grid = (Grid)((Frame)sender).Content;
            Image image = (Image)grid.FindByName("image");

            double heightRequest = 0;

            if (source.IsSelected)
            {
                source.IsSelected = false;
                cView.IsVisible = false;
                heightRequest = 0;
                _ = Task.Run(async () => await image.RotateTo(0));
                source.Rotation = 0;
            }
            else
            {
                source.IsSelected = true;
                heightRequest = (medications?.Count ?? cView.ItemsSource.Cast<object>().Count()) * 60;
                _= Task.Run(async () =>
                {
                    await image.RotateTo(-180);
                });

                source.Rotation = -180;
            }

            cView.IsVisible = true;
            cView.HeightRequest = heightRequest;
        }
    }
}

