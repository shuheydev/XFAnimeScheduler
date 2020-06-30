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
    public partial class AnimeInfosPage : ContentPage
    {
        private readonly AnimeInfosPageViewModel _viewModel;

        public AnimeInfosPage()
        {
            InitializeComponent();

            this.BindingContext = _viewModel = Startup.ServiceProvider.GetService<AnimeInfosPageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.Init();
        }
    }
}