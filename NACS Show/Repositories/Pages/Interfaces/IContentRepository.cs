using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NACSShow;
namespace NACSShow.Repositories.Pages.Interfaces
{
    public interface IContentRepository
    {
        Task<IEnumerable<DailyNews>> GetDailyNewContentItems();
        Task<IEnumerable<NewsArticle>> GetNewsArticleContentItemsAsync(string title, DateTime date, string path);
    }
}
