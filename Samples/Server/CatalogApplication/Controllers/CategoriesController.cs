using System;
using CatalogApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sensei.AspNet;
using Swashbuckle.AspNetCore.Annotations;

namespace CatalogApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [SwaggerTag("Create, read, update and delete categories")]
    public class CategoriesController : EntityController<Category, CatalogDbContext>
    {
        public CategoriesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}