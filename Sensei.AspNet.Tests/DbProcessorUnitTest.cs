using System;
using System.Threading.Tasks;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests
{
    public class DbProcessorUnitTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _testServerFixture;

        public DbProcessorUnitTest(TestServerFixture fixture)
        {
            _testServerFixture = fixture;
        }

        [Fact]
        public async Task DateTimeProcessor()
        {
            var client = _testServerFixture.TestServer.CreateClient();

            var result = await client.PostAsync<TestModel>("/test", new TestModel());
            Assert.True(result.CreatedAt > DateTime.MinValue);
            Assert.True(result.UpdatedAt > DateTime.MinValue);
            
            var result2 = await client.PutAsync<TestModel>($"/test/{result.Id}", result);
            Assert.Equal(result.CreatedAt, result2.CreatedAt);
            Assert.True(result2.UpdatedAt > result.UpdatedAt);
        }
        
        [Fact]
        public async Task ClaimProcessor()
        {
            var client = _testServerFixture.TestServer.CreateClient();

            var testModel = new TestModel
            {
                ClaimAdded = "test 1",
                ClaimAddedSkipIfExist = "test 2",
                ClaimAddedSkipIfExist2 = null,
                ClaimModified = "test 3"
            };

            var result = await client.PostAsync<TestModel>("/test", testModel);
            Assert.Equal("test.user@example.com", result.ClaimAdded);
            Assert.Equal("test 2", result.ClaimAddedSkipIfExist);
            Assert.Equal("test.user@example.com", result.ClaimAddedSkipIfExist2);
            Assert.Equal("test 3", result.ClaimModified);
            
            var result2 = await client.PutAsync<TestModel>($"/test/{result.Id}", result);
            Assert.Equal("test.user@example.com", result2.ClaimAdded);
            Assert.Equal("test 2", result2.ClaimAddedSkipIfExist);
            Assert.Equal("test.user@example.com", result.ClaimAddedSkipIfExist2);
            Assert.Equal("test.user@example.com", result2.ClaimModified);
        }
        
        [Fact]
        public async Task UserIdProcessor()
        {
            var client = _testServerFixture.TestServer.CreateClient();

            var testModel = new TestModel
            {
                UserIdAdded = Guid.Empty,
                UserIdAddedSkipIfExist = Guid.Empty,
                UserIdAddedSkipIfExist2 = Guid.NewGuid(),
                UserIdModified = Guid.Empty
            };

            var result = await client.PostAsync<TestModel>("/test", testModel);
            Assert.Equal(Guid.Parse("12345678-1234-1234-1234-123456789012"), result.UserIdAdded);
            Assert.Equal(Guid.Parse("12345678-1234-1234-1234-123456789012"), result.UserIdAddedSkipIfExist);
            Assert.Equal(testModel.UserIdAddedSkipIfExist2, result.UserIdAddedSkipIfExist2);
            Assert.Equal(Guid.Empty, result.UserIdModified);
            
            result.UserIdAdded = Guid.NewGuid();
            result.UserIdAddedSkipIfExist = Guid.NewGuid();
            result.UserIdAddedSkipIfExist2 = Guid.NewGuid();
            
            var result2 = await client.PutAsync<TestModel>($"/test/{result.Id}", result);
            Assert.Equal(result.UserIdAdded, result2.UserIdAdded);
            Assert.Equal(result.UserIdAddedSkipIfExist, result2.UserIdAddedSkipIfExist);
            Assert.Equal(result.UserIdAddedSkipIfExist2, result2.UserIdAddedSkipIfExist2);
            Assert.Equal(Guid.Parse("12345678-1234-1234-1234-123456789012"), result2.UserIdModified);
        }
    }
}