using Animescheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XFAnimeScheduler.Services;

namespace XFAnimeScheduler.ViewModels
{
    [QueryProperty("AnimeId", "animeId")]
    public class AnimeInfoDetailsPageViewModel : BaseViewModel
    {
        public string AnimeId { get; set; } = string.Empty;

        private AnimeInfo _animeInfo = new AnimeInfo();
        public AnimeInfo AnimeInfo
        {
            get => _animeInfo;
            set => SetProperty(ref _animeInfo, value);
        }

        public ICommand OfficialSiteLabelTappedCommand => new MvvmHelpers.Commands.AsyncCommand<string>(async (url) =>
        {
            await Xamarin.Essentials.Browser.OpenAsync(url);
        });


        private readonly IDataService _dataService;

        public AnimeInfoDetailsPageViewModel(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public void Init()
        {
            AnimeInfo = _dataService.GetAnimeInfoById(int.Parse(AnimeId));
        }
    }
}
