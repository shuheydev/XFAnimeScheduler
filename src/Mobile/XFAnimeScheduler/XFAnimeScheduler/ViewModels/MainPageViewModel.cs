using Animescheduler;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XFAnimeScheduler.Services;

namespace XFAnimeScheduler.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;

        private ObservableRangeCollection<AnimeInfo> _animeInfos = new ObservableRangeCollection<AnimeInfo>();
        public ObservableRangeCollection<AnimeInfo> AnimeInfos
        {
            get => _animeInfos;
            set => SetProperty(ref _animeInfos, value);
        }

        public MainPageViewModel(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public async Task Init()
        {
            AnimeInfos = new ObservableRangeCollection<AnimeInfo>(await _dataService.GetAnimeInfosAsync());
        }
    }
}
