using Animescheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XFAnimeScheduler.Services
{
    public interface IDataService
    {
        Task<IEnumerable<AnimeInfo>> GetAnimeInfosAsync();
    }
}
