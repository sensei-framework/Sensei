using System;
using System.Threading.Tasks;
using CatalogApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sensei.AspNet;
using Swashbuckle.AspNetCore.Annotations;

namespace CatalogApplication.Controllers
{
    [Authorize(Policy = "read")]
    [Route("[controller]")]
    [SwaggerTag("Create, read, update and delete articles")]
    public class ArticlesController : EntityController<Article, CatalogDbContext>
    {
        public ArticlesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        //[Authorize(Policy = "create")]
        public override Task<ActionResult<Article>> Post(Article entity)
        {
            return base.Post(entity);
        }

        //[Authorize(Policy = "update")]
        public override Task<ActionResult<Article>> Put(Guid id, Article entity)
        {
            return base.Put(id, entity);
        }

        [Authorize(Policy = "delete")]
        public override Task<ActionResult> Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}