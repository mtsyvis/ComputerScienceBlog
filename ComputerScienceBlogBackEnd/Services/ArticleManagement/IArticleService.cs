using ComputerScienceBlogBackEnd.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerScienceBlogBackEnd.Services.ArticleManagement
{
    public interface IArticleService
    {
        Task CreateAsync(Article articleIn);

        Task UpdateAsync(string id, Article articleIn);

        Task RemoveAsync(string id);

        Task<Article> GetByIdAsync(string id);

        Task<List<Article>> GetAllAsync();

        Task AddCommentAsync(string articleId, Comment comment);

        Task<List<Article>> GetByPartialTitleAsync(string title);

        Task<List<Article>> GetByCategoryAsync(string category);
    }
}
