using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CalculadoraAnestesica.DependencyInjection.IoC;
using CalculadoraAnestesica.Shared.Interfaces;
using Xamarin.Forms;

namespace CalculadoraAnestesica.Controls
{
    public static class NavigationControl
    {
        public static async Task PushAsync<TPage, TViewModel>(
            Action<TViewModel> parameters = null,
            bool animation = false)
        {
            await InnerPushAsync<TPage, TViewModel>(parameters, animation);
        }

        private static async Task InnerPushAsync<TPage, TViewModel>(
            Action<TViewModel> parameters,
            bool animation)
        {
            try
            {
                TPage view = GetView<TPage>();
                TViewModel viewModel = GetViewModel<TViewModel>();

                parameters?.Invoke(viewModel);

                SetBindingContext(view, viewModel);

                await Application
                    .Current
                    .MainPage
                    .Navigation
                    .PushAsync(view as Page, animation);
            }
            catch (Exception ex)
            {
                //await CNavigation.ErrorHandler();
                //CNavigation.GetPage().IsBusy = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private static void SetBindingContext<TPage, TViewModel>(
            TPage view,
            TViewModel viewModel)
        {
            ((IViewContext)view).SetBinding((IViewModelBase)viewModel);
            ((IViewModelBase)viewModel).AfterBinding();
        }

        private static TPage GetView<TPage>()
        {
            return Resolver.Get<TPage>();
        }

        private static TViewModel GetViewModel<TViewModel>()
        {
            return Resolver.Get<TViewModel>();
        }

        public static Page GetPage<TPage, TViewModel>(Action<TViewModel> parameters = null)
        {
            TPage view = GetView<TPage>();
            TViewModel viewModel = GetViewModel<TViewModel>();

            parameters?.Invoke(viewModel);

            SetBindingContext(view, viewModel);
            ((IViewContext)view).AddComponents();

            return view as Page;
        }
    }
}

