using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Filters;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<List<Article>>> GetAllAsync()
        {
            return await _articleService.GetAllAsync();
        }

        [HttpGet]
        [Route("{id:length(24)}")]
        public async Task<ActionResult<Article>> GetArticleAsync(string id)
        {
            if (id is null)
            {
                return BadRequest(id);
            }

            return await _articleService.GetByIdAsync(id);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult> CreateArticleAsync([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            article.CreatedUser = User.Identity.Name;
            await _articleService.CreateAsync(article);

            return Ok(article);
        }

        [HttpPost]
        [Route("{articleId:length(24)}/comment")]
        [Authorize]
        public async Task<ActionResult> AddCommentAsync([FromRoute] string articleId, [FromBody] Comment comment)
        {
            if (articleId is null)
            {
                return BadRequest(articleId);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment.CreatedUser = User.Identity.Name;
            await _articleService.AddCommentAsync(articleId, comment);

            return Ok(comment);
        }

        [HttpPut]
        [Route("{id:length(24)}")]
        [Authorize]
        public async Task<ActionResult> UpdateArticleAsync([FromRoute] string id, [FromBody] Article articleIn)
        {
            if (id is null)
            {
                return BadRequest(id);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.UpdateAsync(id, articleIn);

            return Ok(articleIn);
        }

        [HttpDelete]
        [Route("{id:length(24)}")]
        public async Task<ActionResult> DeleteArticleAsync(string id)
        {
            if (id is null)
            {
                return BadRequest(id);
            }

            var user = await _articleService.GetByIdAsync(id);

            await _articleService.RemoveAsync(user.Id);

            return NoContent();
        }
    }
}