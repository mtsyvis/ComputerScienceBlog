using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Exceptions;
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

        public async Task Create(Article articleIn)
        {
            using(var cursor = await _articles.FindAsync(article=> article.Title == articleIn.Title))
            {
                if(await cursor.MoveNextAsync())
                {
                    throw new RequestedResourceHasConflictException("User can't create a post with already existing article title");
                }
            }

            _articles.InsertOne(articleIn);
        }

        public List<Article> GetAll() => _articles.Find(article => true).ToList();

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
