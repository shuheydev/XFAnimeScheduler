using Animescheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XFAnimeScheduler.Services
{
    public class DataService : IDataService
    {
        public async Task<IEnumerable<AnimeInfo>> GetAnimeInfosAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("XFAnimeScheduler.Resources.Summer_Anime.json");

            var animeInfos = await JsonSerializer.DeserializeAsync<IEnumerable<AnimeInfo>>(stream);

            //放送日の早い順に並び替え用

            return animeInfos.OrderBy(a =>
            {
                var validSchedules = a.Schedules.Where(s => s.GetDateTimeOffset() != DateTimeOffset.MinValue);

                if (!validSchedules.Any())
                    return DateTimeOffset.MaxValue;

                var earliestSchedule = validSchedules.Min(s =>
                  {
                      var min = s.GetDateTimeOffset();
                      return min;
                  });

                return earliestSchedule;
            });
        }
    }
}
