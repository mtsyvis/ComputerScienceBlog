using ComputerScienceBlogBackEnd.DataAccess;
using System.Collections.Generic;

namespace ComputerScienceBlogBackEnd.Services.ArticleManagement
{
    public interface IArticleService
    {
        void Create(Article articleIn);

        void Update(string id, Article articleIn);

        void Remove(string id);

        Article GetById(string id);

        List<Article> GetAll();

        void AddComment(string articleId, Comment comment);

        List<Article> GetByPartialTitle(string title);

        List<Article> GetByCategory(string category);
    }
}
