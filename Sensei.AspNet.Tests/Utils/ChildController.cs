using System;
using Microsoft.AspNetCore.Mvc;

namespace Sensei.AspNet.Tests.Utils
{
    [Route("[controller]")]
    [ApiController]
    public class ChildController : EntityController<TestChildModel, TestDbContext>
    {
        public ChildController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}