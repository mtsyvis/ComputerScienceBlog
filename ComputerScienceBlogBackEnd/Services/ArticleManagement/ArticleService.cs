using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using MongoDB.Driver;

namespace ComputerScienceBlogBackEnd.Services.ArticleManagement
{
    public class ArticleService : IArticleService
    {
        private readonly IMongoCollection<Article> _articles;

        public ArticleService(IComputerScienceBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _articles = database.GetCollection<Article>(settings.ArticlesCollectionName);
        }

        public void AddComment(string articleId, Comment comment)
        {
            throw new NotImplementedException();
        }

        public void Create(Article articleIn)
        {
            _articles.InsertOne(articleIn);
        }

        public List<Article> GetAll() => _articles.Find(user => true).ToList();

        public List<Article> GetByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Article GetById(string id) =>
            _articles.Find<Article>(article => article.Id == id).FirstOrDefault();
       

        public List<Article> GetByPartialTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id) =>
            _articles.DeleteOne(article => article.Id == id);

        public void Update(string id, Article articleIn) =>
            _articles.ReplaceOne(article => article.Id == id, articleIn);
    }
}
