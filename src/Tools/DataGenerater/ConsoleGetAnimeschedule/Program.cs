using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Animescheduler
{
    class Program
    {
        static async Task Main(string[] args)
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

            var serviceProvider = services.BuildServiceProvider();

            var scraper = serviceProvider.GetService<IAnimeScraper>() ?? throw new InvalidOperationException("IAnimeScraperのインスタンス化に失敗しました");

            var doc = await scraper.GetHtmlDocumentAsync();
            var animeElems = scraper.GetAnimeElements(doc);

            var animeInfos = scraper.ToAnimeInfo(animeElems);


            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            var json = System.Text.Json.JsonSerializer.Serialize<IEnumerable<AnimeInfo>>(animeInfos, options);

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"AnimeSchedule_{Settings.Season}.json");
            using var sw = new StreamWriter(filePath).BaseStream;
            await JsonSerializer.SerializeAsync(sw, animeInfos, options);
        }
    }
}
