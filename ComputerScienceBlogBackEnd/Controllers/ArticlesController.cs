using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Infrastructure.Exceptions;
using ComputerScienceBlogBackEnd.Infrastructure.Filters;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetAll();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(string id)
        {
            var article = await _articleService.GetById(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<Article>> Create(Article article)
        {
            await _articleService.Create(article);
           
            return CreatedAtRoute("", new { id = article.Id.ToString() }, article);
        }

        [HttpPost("{articleId:length(24)}")]
        public async Task<IActionResult> AddComment([FromRoute] string articleId, [FromBody] Comment comment)
        {
            await _articleService.AddComment(articleId, comment);
            //var article = _articleService.GetById(articleId);

            return Ok();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Article userIn)
        {
            var user = await _articleService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            await _articleService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _articleService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            _articleService.Remove(user.Id);

            return NoContent();
        }
    }
}