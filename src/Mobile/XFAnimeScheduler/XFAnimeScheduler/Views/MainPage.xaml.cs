using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFAnimeScheduler.ViewModels;

namespace XFAnimeScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = _viewModel = Startup.ServiceProvider.GetService<MainPageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.Init();
        }
    }
}