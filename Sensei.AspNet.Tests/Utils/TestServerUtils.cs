using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Sensei.AspNet.Tests.FakeServer;

namespace Sensei.AspNet.Tests.Utils
{
    public static class TestServerUtils
    {
        public static TestServer Create(Func<WebHostBuilder, WebHostBuilder> builderAction = null)
        {
            var builder = new WebHostBuilder();
            if (builderAction != null)
                builder = builderAction(builder);
            builder.UseStartup<Startup>();

            return new TestServer(builder);
        }
    }
}