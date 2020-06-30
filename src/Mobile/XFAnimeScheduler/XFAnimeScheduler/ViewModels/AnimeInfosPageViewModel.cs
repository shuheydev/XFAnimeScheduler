using Animescheduler;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFAnimeScheduler.Services;
using XFAnimeScheduler.Views;

namespace XFAnimeScheduler.ViewModels
{
    public class AnimeInfosPageViewModel : BaseViewModel
    {
        private bool _isInitialized = false;

        private readonly IDataService _dataService;

        private ObservableRangeCollection<AnimeInfo> _animeInfos = new ObservableRangeCollection<AnimeInfo>();
        public ObservableRangeCollection<AnimeInfo> AnimeInfos
        {
            get => _animeInfos;
            set => SetProperty(ref _animeInfos, value);
        }

        public ICommand ListItemTappedCommand => new MvvmHelpers.Commands.AsyncCommand<int>(async (id) =>
        {
            await Shell.Current.GoToAsync($"animeInfos/details?animeId={id}");
        });

        public AnimeInfosPageViewModel(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public async Task Init()
        {
            if (_isInitialized)
                return;

            AnimeInfos = new ObservableRangeCollection<AnimeInfo>(await _dataService.GetAnimeInfosAsync());

            _isInitialized = true;
        }
    }
}
