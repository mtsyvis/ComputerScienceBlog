using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Filters;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Mvc;

namespace ComputerScienceBlogBackEnd.Controllers
{
    [Route("api/articles")]
    [ApiController]
    [HttpGlobalExceptionFilter]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var articles = await _articleService.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet]
        [Route("{id:length(24)}")]
        public async Task<IActionResult> GetArticleAsync(string id)
        {
            var article = await _articleService.GetByIdAsync(id);

            return Ok(article);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Article>> CreateArticleAsync([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.CreateAsync(article);
           
            return CreatedAtRoute($"api/articles/{article.Id}", article);
        }

        [HttpPost]
        [Route("{articleId:length(24)}/comment")]
        public async Task<IActionResult> AddCommentAsync([FromRoute] string articleId, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.AddCommentAsync(articleId, comment);

            return Ok();
        }

        [HttpPut]
        [Route("{id:length(24)}")]
        public async Task<IActionResult> UpdateArticleAsync([FromRoute] string id, [FromBody] Article articleIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.UpdateAsync(id, articleIn);

            return CreatedAtAction($"api/articles/{id}", articleIn);
        }

        [HttpDelete]
        [Route("{id:length(24)}")]
        public async Task<IActionResult> DeleteArticleAsync(string id)
        {
            var user = await _articleService.GetByIdAsync(id);

            await _articleService.RemoveAsync(user.Id);

            return NoContent();
        }
    }
}