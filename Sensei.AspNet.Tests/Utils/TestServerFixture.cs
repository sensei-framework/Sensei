using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sensei.AspNet.Tests.Utils
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; }
        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();

            TestServer = new TestServer(builder);
        }
        
        public void Dispose()
        {
            TestServer.Dispose();
        }
    }
}