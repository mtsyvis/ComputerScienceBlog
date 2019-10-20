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

        public async Task AddCommentAsync(string articleId, Comment comment)
        {
            using (var cursor = await _articles.FindAsync(article => article.Id == articleId))
            {
                if (! await cursor.MoveNextAsync())
                {
                    throw new RequestedResourceNotFoundException();
                }
            }

            var filter = Builders<Article>.Filter.Eq(article => article.Id, articleId);

            var update = Builders<Article>.Update.AddToSet(article => article.Comments, comment);

            await _articles.UpdateOneAsync(filter, update);
        }

        public async Task CreateAsync(Article articleIn)
        {
            var dbArticles = await _articles.Find(article => article.Title == articleIn.Title).ToListAsync();

            if (dbArticles.Count > 0)
            {
                throw new RequestedResourceHasConflictException("User can't create a post with already existing article title");
            }

            _articles.InsertOne(articleIn);
        }

        public async Task<List<Article>> GetAllAsync() => await _articles.Find(article => true).ToListAsync();

        public Task<List<Article>> GetByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<Article> GetByIdAsync(string id)
        {
            var dbArticles = await _articles.Find(article => article.Id == id).ToListAsync();

            if (dbArticles.Count == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            return dbArticles[0];
        }

        public Task<List<Article>> GetByPartialTitleAsync(string title)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id)
        {
            await _articles.DeleteOneAsync(article => article.Id == id);
        }

        public async Task UpdateAsync(string id, Article articleIn)
        {
            await _articles.ReplaceOneAsync(article => article.Id == id, articleIn);
        }
    }
}
