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

namespace CalculadoraAnestesica.View
{	
	public partial class AppCenterView : ContentPage, IAppCenterView
    {
        public AppCenterView()
        {
			InitializeComponent();



            SetFirstAccess();
        }

        protected override void OnAppearing()
        {
            var medicamentos = Resolver
            .Get<IMedicamentosDataAccess>()
            .GetAllMedicamentos();

            cView.ItemsSource = medicamentos;
        }

        private void SetFirstAccess()
        {
            if (AppHandler.IsFirstAccess())
            {
                var userContext = Resolver.Get<IUserContext>();
                userContext.IsFirstAccess = true;

                Resolver
                .Get<IUserContextDataAccess>()
                .Insert(userContext);
            }
        }
    }
}

