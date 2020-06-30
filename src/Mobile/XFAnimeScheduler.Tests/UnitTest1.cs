using Animescheduler;
using System;
using System.Linq;
using System.Threading.Tasks;
using XFAnimeScheduler.Services;
using Xunit;

namespace XFAnimeScheduler.Tests
{
    public class DataServiceTest
    {
        [Fact]
        public async Task TestGetAnimeInfos()
        {
            IDataService dataService = new DataService();

            var animeInfos = await dataService.GetAnimeInfosAsync();

            Assert.Equal(32, animeInfos.Count());
        }
    }
}
