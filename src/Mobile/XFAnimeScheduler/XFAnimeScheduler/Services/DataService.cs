using Animescheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFAnimeScheduler.Services
{
    public class DataService : IDataService
    {
        private IEnumerable<AnimeInfo> _animeInfos = new List<AnimeInfo>();

        public DataService()
        {
            Init().GetAwaiter().GetResult();
        }

        public AnimeInfo GetAnimeInfoById(int animeId)
        {
            return _animeInfos.FirstOrDefault(a => a.Id == animeId);
        }

        private async Task Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("XFAnimeScheduler.Resources.AnimeSchedule.json");

            _animeInfos = await JsonSerializer.DeserializeAsync<IEnumerable<AnimeInfo>>(stream) ?? new List<AnimeInfo>();

            //画像リソースを割り当てる
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            foreach (var animeInfo in _animeInfos)
            {
                animeInfo.ImagePath = $"{assemblyName}.Resources.Images.{animeInfo.Id}.png";
            }

            //スケジュールを早い順に並べ替え
            foreach (var animeInfo in _animeInfos)
            {
                animeInfo.Schedules = animeInfo.Schedules.OrderBy(s => s.GetDateTimeOffset()).ToList();
            }
        }

        public async Task<IEnumerable<AnimeInfo>> GetAnimeInfosAsync()
        {
            await Init();

            //var a = _animeInfos.Select(a => a.Schedules.Min(s => $"{a.Title} {s.GetDateTimeOffset().ToString()}"));

            //放送日の早い順に並び替え用
            //var b = _animeInfos.OrderBy(anime => anime.Schedules.Min(s => s.GetDateTimeOffset()));
            //return _animeInfos.OrderBy(anime => anime.Schedules.Min(s => s.GetDateTimeOffset()));
            return _animeInfos.OrderBy(anime => anime.Schedules?.First().GetDateTimeOffset());
        }
    }
}
