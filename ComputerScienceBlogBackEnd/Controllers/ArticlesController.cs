using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScienceBlogBackEnd.DataAccess;
using ComputerScienceBlogBackEnd.Services.ArticleManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerScienceBlogBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var articles = _articleService.GetAll();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(string id)
        {
            var article = _articleService.GetById(id);

            if (article == null)
            {
                return NotFound();
            }

            // only allow admins to access other user records
            //var currentUserId = User.Identity.Name;
            //if (id != currentUserId && !User.IsInRole(Role.Admin))
            //{
            //    return Forbid();
            //}

            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult<Article>> Create(Article article)
        {
            _articleService.Create(article);

            return CreatedAtRoute("", new { id = article.Id.ToString() }, article);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Article userIn)
        {
            var user = _articleService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            _articleService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _articleService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            _articleService.Remove(user.Id);

            return NoContent();
        }
    }
}