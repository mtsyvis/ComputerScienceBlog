using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Exceptions;
using MongoDB.Bson;
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

        public async Task AddComment(string articleId, Comment comment)
        {
            using (var cursor = await _articles.FindAsync(article => article.Id == articleId))
            {
                if (! await cursor.MoveNextAsync())
                {
                    throw new RequestedResourceNotFoundException("User can't create a post with already existing article title");
                }
            }

            var filter = Builders<Article>.Filter.Eq(article => article.Id, articleId);

            var update = Builders<Article>.Update.AddToSet(article => article.Comments, comment);

            await _articles.UpdateOneAsync(filter, update);
        }

        public async Task Create(Article articleIn)
        {
            //var dbArticles = await _articles.Find(article => article.Title == articleIn.Title).ToListAsync();

            //if (dbArticles.Count > 0)
            //{
            //    throw new RequestedResourceHasConflictException("User can't create a post with already existing article title");
            //}

            using (var cursor = await _articles.FindAsync(article => article.Title == articleIn.Title))
            {
                if (await cursor.MoveNextAsync())
                {
                    throw new RequestedResourceHasConflictException("User can't create a post with already existing article title");
                }
            }

            _articles.InsertOne(articleIn);
        }

        public async Task<List<Article>> GetAll() => await _articles.Find(article => true).ToListAsync();

        public Task<List<Article>> GetByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<Article> GetById(string id)
        {
            var dbArticles = await _articles.Find(article => article.Id == id).ToListAsync();

            if (dbArticles.Count == 0)
            {
                throw new RequestedResourceNotFoundException("User can't create a post with already existing article title");
            }

            return dbArticles[0];
        }

        public Task<List<Article>> GetByPartialTitle(string title)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(string id) =>
            _articles.DeleteOne(article => article.Id == id);

        public async Task Update(string id, Article articleIn) =>
            _articles.ReplaceOne(article => article.Id == id, articleIn);
    }
}
