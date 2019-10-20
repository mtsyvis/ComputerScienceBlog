using ComputerScienceBlogBackEnd.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputerScienceBlogBackEnd.Services.ArticleManagement
{
    public interface IArticleService
    {
        Task Create(Article articleIn);

        Task Update(string id, Article articleIn);

        Task Remove(string id);

        Task<Article> GetById(string id);

        Task<List<Article>> GetAll();

        Task AddComment(string articleId, Comment comment);

        Task<List<Article>> GetByPartialTitle(string title);

        Task<List<Article>> GetByCategory(string category);
    }
}
