using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sensei.AspNet.Tests.Utils
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : EntityController<TestModel, TestDbContext>
    {
        public TestController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}