using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Animescheduler
{
    public class AnimeScraper : IAnimeScraper
    {
        private readonly HttpClient _httpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public AnimeScraper(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient(Settings.HttpClientKey);
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<string> GetHtmlAsync()
        {
            var response = await this._httpClient.GetAsync(Settings.Season);

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<IHtmlDocument> GetHtmlDocumentAsync()
        {
            var parser = new HtmlParser();

            var html = await GetHtmlAsync();

            var token = _cancellationTokenSource.Token;
            var doc = await parser.ParseDocumentAsync(html, token);

            return doc;
        }

        public IHtmlCollection<IElement> GetAnimeElements(IHtmlDocument doc)
        {
            var titles = doc.QuerySelectorAll("div.itemBox");

            return titles;
        }

        public IEnumerable<AnimeInfo> ToAnimeInfo(IHtmlCollection<IElement> animeElements)
        {
            List<AnimeInfo> animeInfos = new List<AnimeInfo>();

            foreach (var elem in animeElements.Select((s, i) => new { element = s, index = i }))
            {
                var animeInfo = new AnimeInfo();

                animeInfo.Id = elem.index;

                //タイトル取得
                //div.mTitle>h2>a>text
                animeInfo.Title = GetTitle(elem.element);

                //作品公式URL取得
                //a.officialSite
                animeInfo.OfficialUrl = GetOfficialUrl(elem.element);

                //放送局,放送開始日付,放送時間 取得
                //div.schedule
                //2,3番目のtr内にそれぞれ3つずつtdがある

                //放送局
                //td内の1つ目のspan.station
                //初回放送開始日時
                //td内の2つ目のspan

                //span.stationをすべて取得して,それとその次の要素で取得できそう
                animeInfo.Schedules = GetSchedules(elem.element);

                animeInfos.Add(animeInfo);
            }

            return animeInfos;
        }

        private Regex _regDate = new Regex(@"^\d+年\d+月\d+日", RegexOptions.Compiled);
        private Regex _regTime = new Regex(@"\d+:\d+", RegexOptions.Compiled);
        public IEnumerable<Schedule> GetSchedules(IElement elem)
        {
            //        //放送局,放送開始日付,放送時間 取得
            //        //div.schedule
            //        //2,3番目のtr内にそれぞれ3つずつtdがある

            //        //放送局
            //        //td内の1つ目のspan.station
            //        //初回放送開始日時
            //        //td内の2つ目のspan

            //        //span.stationをすべて取得して,それとその次の要素で取得できそう
            //        animeInfo.Schedules = GetSchedules(elem);    

            var stations = elem.QuerySelectorAll("span.station");
            var dateTimes = stations.Next("span");

            var schedules = stations.Zip(dateTimes, (s, d) =>
            {
                var schedule = new Schedule
                {
                    Station = s.Text(),
                    DateTimeFrom = d.Text(),
                };

                var dateMatch = _regDate.Match(schedule.DateTimeFrom);
                schedule.DateFrom = dateMatch.Success ? dateMatch.Value : string.Empty;
                var timeMatch = _regTime.Match(schedule.DateTimeFrom);
                schedule.TimeFrom = timeMatch.Success ? timeMatch.Value : string.Empty;

                return schedule;
            });

            return schedules;
        }

        public string GetOfficialUrl(IElement elem)
        {
            //        //作品公式URL取得
            //        //a.officialSite
            //        animeInfo.OfficialUrl = GetOfficialUrl(elem);
            string url = elem.QuerySelector("a.officialSite")?.GetAttribute("href") ?? string.Empty;

            return url;
        }

        public string GetTitle(IElement elem)
        {
            //        //タイトル取得
            //        //div.mTitle h2 a text
            //        animeInfo.Title = GetTitle(elem);
            string title = elem.QuerySelector("div.mTitle h2 a")?.Text() ?? string.Empty;

            return title;
        }
    }
}