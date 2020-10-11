using System;
using Microsoft.AspNetCore.Mvc;

namespace Sensei.AspNet.Tests.Utils
{
    [Route("[controller]")]
    [ApiController]
    public class Child2Controller : EntityController<TestChildModel2, TestDbContext>
    {
        public Child2Controller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}