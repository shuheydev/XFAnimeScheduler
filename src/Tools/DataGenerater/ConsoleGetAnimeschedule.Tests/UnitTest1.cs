using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using System.Linq;

namespace Animescheduler.Tests
{
    public static class Setup
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddHttpClient(Settings.HttpClientKey, c =>
            {
                c.BaseAddress = new Uri(Settings.SiteUrl);
            });

#if Debug_Mock
            services.AddSingleton<IAnimeScraper, AnimeScraperMock>();
#else
            services.AddSingleton<IAnimeScraper, AnimeScraper>();
#endif

            ServiceProvider = services.BuildServiceProvider();
        }
    }

    public class UnitTestGeneral
    {
        private readonly IAnimeScraper _scraper;
        public UnitTestGeneral()
        {
            Setup.Init();

            var animeScraper = Setup.ServiceProvider;
            this._scraper = animeScraper.GetService<IAnimeScraper>();
        }

        [Fact(DisplayName = "対象のページのHTMLを取得できること")]
        public async Task TestGetHtml()
        {
            var html = await _scraper.GetHtmlAsync();

            Assert.True(!string.IsNullOrEmpty(html));
            Assert.Contains("div", html);
        }

        [Fact(DisplayName = "Parseできる")]
        public async Task TestParseDocument()
        {
            var doc = await _scraper.GetHtmlDocumentAsync();
            Assert.NotNull(doc);
        }
    }

    public class UnitTestForSummer
    {
        private readonly IAnimeScraper _scraper;
        public UnitTestForSummer()
        {
            Setup.Init();

            var animeScraper = Setup.ServiceProvider;
            this._scraper = animeScraper.GetService<IAnimeScraper>();
        }

        private IHtmlDocument _doc = default(IHtmlDocument);
        private IHtmlCollection<IElement> _animeElements;
        private async Task Init()
        {
            _doc ??= await _scraper.GetHtmlDocumentAsync();
            _animeElements ??= _scraper.GetAnimeElements(_doc);
        }

        [Fact(DisplayName = "タイトル32個取得のはず")]
        public async Task TestGetAnimeElements()
        {
            //var doc = await _scraper.ParseHtmlAsync();
            //var animeElements = _scraper.GetAnimeElements(doc);
            await Init();

            Assert.Equal(32, _animeElements.Length);
        }

        [Fact(DisplayName = "タイトルを取得できるはず")]
        public async Task TestGetTitle()
        {
            //var doc = await _scraper.ParseHtmlAsync();
            //var animeElements = _scraper.GetAnimeElements(doc);
            await Init();

            var anime = _animeElements[0];

            var title = _scraper.GetTitle(anime);

            Assert.True(!string.IsNullOrEmpty(title));
        }

        [Fact(DisplayName = "公式サイトURLを取得できるはず")]
        public async Task TestGetOfficialUrl()
        {
            await Init();

            var anime = _animeElements[0];

            var url = _scraper.GetOfficialUrl(anime);

            Assert.True(!string.IsNullOrEmpty(url));
        }

        [Fact(DisplayName = "スケジュールが取得できるはず")]
        public async Task TestGetSchedules()
        {
            await Init();

            var anime = _animeElements[0];

            var schedules = _scraper.GetSchedules(anime);

            Assert.True(schedules.Any());
        }

        [Fact(DisplayName = "アニメ情報のコレクションが取得できるはず")]
        public async Task TestToAnimeInfo()
        {
            await Init();

            var animeInfos = _scraper.ToAnimeInfo(_animeElements);

            Assert.True(animeInfos.Any());
            Assert.Equal(32, animeInfos.Count());
        }


        [Fact(DisplayName = "日時情報が日本時間のDatetimeoffsetに変換できること")]
        public async Task TestDateTimeInformation()
        {
            await Init();

            var animeInfos = _scraper.ToAnimeInfo(_animeElements);

            foreach (var schedule in animeInfos.SelectMany(a => a.Schedules))
            {
                var dt = schedule.GetDateTimeOffset();

                if (dt == DateTimeOffset.MinValue)
                    continue;

                Assert.Equal(TimeSpan.FromHours(9), dt.Offset);
            }
        }
    }
}
