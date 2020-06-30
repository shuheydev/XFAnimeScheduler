using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Animescheduler
{
    public interface IAnimeScraper
    {
        Task<string> GetHtmlAsync();
        Task<IHtmlDocument> GetHtmlDocumentAsync();
        IHtmlCollection<IElement> GetAnimeElements(IHtmlDocument doc);
        IEnumerable<Schedule> GetSchedules(IElement elem);
        string GetTitle(IElement elem);
        string GetOfficialUrl(IElement elem);
        IEnumerable<AnimeInfo> ToAnimeInfo(IHtmlCollection<IElement> animeElements);
    }
}