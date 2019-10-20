using ComputerScienceBlogBackEnd.Controllers;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ComputerScienceBlogBackEnd.UnitTests
{
    public class ArticlesControllerTests
    {
        private readonly Mock<IArticleService> _articleServiceMock;

        public ArticlesControllerTests()
        {
            _articleServiceMock = new Mock<IArticleService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAActionResultWithAListOfArticles()
        {
            _articleServiceMock.Setup(services => services.GetAllAsync()).ReturnsAsync(GetFaceArticles());
            var controller = new ArticlesController(_articleServiceMock.Object);

            var result = await controller.GetAllAsync();
            var actionResult = Assert.IsType<ActionResult<List<Article>>>(result);
            var returnValue = Assert.IsType<List<Article>>(actionResult.Value);

            Assert.Equal(GetFaceArticles().Count, returnValue.Count());
        }

        [Fact]
        public async Task GetArticleAsync_ReturnsAActionResultWithAArticle()
        {
            string testArticleId = "1";
            var article = GetFaceArticles().FirstOrDefault(art => art.Id == testArticleId);
            _articleServiceMock.Setup(services => services.GetByIdAsync(testArticleId)).
                ReturnsAsync(article);

            var controller = new ArticlesController(_articleServiceMock.Object);

            var result = await controller.GetArticleAsync(testArticleId);

            var actionResult = Assert.IsType<ActionResult<Article>>(result);
            var returnValue = Assert.IsType<Article>(actionResult.Value);
            Assert.Equal(article.Text, returnValue.Text);
            Assert.Equal(article.Title, returnValue.Title);
            Assert.Equal(testArticleId, returnValue.Id);
        }

        [Fact]
        public async Task GetArticleAsync_ReturnsBadRequestResultWhenIdIsNull()
        {
            var controller = new ArticlesController(_articleServiceMock.Object);

            var result = await controller.GetArticleAsync(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateArticleAsync_ReturnsBadRequest_GivenInvalidModel()
        {
            var controller = new ArticlesController(_articleServiceMock.Object);
            controller.ModelState.AddModelError("error", "some error");

            var result = await controller.UpdateArticleAsync("test", new Article());

            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        private List<Article> GetFaceArticles()
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Id = "1",
                    Text = "bla1",
                    CreatedUser = "Misha",
                    Title = "Unit tessting",
                    CreatedDate = DateTime.Now,
                    Categories = new Category[]
                    {
                        new Category { Value = ".Net"}
                    },
                    Comments = new Comment[]
                    {
                        new Comment { CreatedUser = "Jon Skit", Text = "text"}
                    }
                }
            };

            return articles;
        }
    }
}
