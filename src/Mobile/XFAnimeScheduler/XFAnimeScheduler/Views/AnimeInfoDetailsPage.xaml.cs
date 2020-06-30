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
    public partial class AnimeInfoDetailsPage : ContentPage
    {
        private readonly AnimeInfoDetailsPageViewModel _viewModels;

        public AnimeInfoDetailsPage()
        {
            InitializeComponent();

            this.BindingContext = _viewModels = Startup.ServiceProvider.GetService<AnimeInfoDetailsPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModels.Init();
        }
    }
}