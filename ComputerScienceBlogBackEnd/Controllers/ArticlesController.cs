using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Filters;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Mvc;

namespace ComputerScienceBlogBackEnd.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllAsync()
        {
            var articles = await _articleService.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleAsync(string id)
        {
            var article = await _articleService.GetByIdAsync(id);

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<Article>> CreateAsync(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.CreateAsync(article);
           
            return CreatedAtRoute("", new { id = article.Id.ToString() }, article);
        }

        [HttpPost("{articleId:length(24)}")]
        public async Task<IActionResult> AddCommentAsync([FromRoute] string articleId, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.AddCommentAsync(articleId, comment);
            //var article = _articleService.GetById(articleId);

            //return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] Article userIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _articleService.UpdateAsync(id, userIn);

            return Ok();
            //return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _articleService.GetByIdAsync(id);

            await _articleService.RemoveAsync(user.Id);

            return NoContent();
        }
    }
}