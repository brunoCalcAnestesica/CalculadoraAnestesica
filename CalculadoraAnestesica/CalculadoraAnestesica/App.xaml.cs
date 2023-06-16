using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculadoraAnestesica.ApplicationHandler;
using CalculadoraAnestesica.Controls;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.View;
using CalculadoraAnestesica.View.Intercaces;
using CalculadoraAnestesica.ViewModel.Interfaces;
using DevExpress.XamarinForms.CollectionView;
using Syncfusion.Licensing;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalculadoraAnestesica
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitApp();
            InitNavigation();
        }

        private void InitNavigation()
        {
            MainPage = new NavigationPage(new TabbedPageView());
        }

        private void InitApp()
        {
            AppHandler.CreateIoC();
            AppHandler.AppStart();
        }
    }
}
