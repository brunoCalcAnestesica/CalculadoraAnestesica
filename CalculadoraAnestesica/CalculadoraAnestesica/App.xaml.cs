using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculadoraAnestesica.ApplicationHandler;
using CalculadoraAnestesica.DataAccess.Interfaces;
using CalculadoraAnestesica.DbContext.Tables;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Model.Interfaces;
using CalculadoraAnestesica.View;
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
            MainPage = new AppCenterView();
        }

        private void InitApp()
        {
            AppHandler.CreateIoC();
            AppHandler.InitDatabase();
            AppHandler.AppStart();
        }
    }
}
