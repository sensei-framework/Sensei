using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Sensei.AspNet.Models;
using Sensei.AspNet.Queries.Entities;
using Sensei.AspNet.Tests.Utils;
using Xunit;

namespace Sensei.AspNet.Tests
{
    /*
    public class QueryUnitTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _testServerFixture;

        public QueryUnitTest(TestServerFixture fixture)
        {
            _testServerFixture = fixture;
        }

        [Theory]
        [MemberData(nameof(QueryDataGenerator.GetTestModels), MemberType = typeof(QueryDataGenerator))]
        public async Task Crud(TestModel testModel)
        {
            var client = _testServerFixture.TestServer.CreateClient();

            // POST
            var testModelPostResult = await client.PostAsync<TestModel>("/test", testModel);
            if (testModel.Id == Guid.Empty)
            {
                Assert.NotEqual(Guid.Empty, testModelPostResult.Id);
                testModel.Id = testModelPostResult.Id;
            }
            else
                Assert.Equal(testModel.Id, testModelPostResult.Id);

            // GET
            var testModelResult = await client.GetAsync<TestModel>($"/test/{testModel.Id}");
            Assert.Equal(testModel.Name, testModelResult.Name);

            // PUT
            testModel.Name = null;
            await client.PutAsync<TestModel>($"/test/{testModel.Id}", testModel);

            // GET
            var testModelResult2 = await client.GetAsync<TestModel>($"/test/{testModel.Id}");
            Assert.Equal(testModel.Name, testModelResult2.Name);
            
            // DELETE
            var response = await client.DeleteAsync($"/test/{testModel.Id}");
            response.EnsureSuccessStatusCode();
            
            // GET
            var response2 = await client.GetAsync($"/test/{testModel.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }
        
        [Theory]
        [MemberData(nameof(QueryDataGenerator.GetListOfTestModels), MemberType = typeof(QueryDataGenerator))]
        public async Task Count(List<TestModel> testModels)
        {
            var client = _testServerFixture.TestServer.CreateClient();
            var ids = new List<Guid>();

            foreach (var testModel in testModels)
            {
                var model = await client.PostAsync<TestModel>("/test", testModel);
                ids.Add(model.Id);
            }

            var count = await client.GetAsync<SingleValue<long>>("/test/count");
            Assert.Equal(testModels.Count, count.Value);

            count = await client.GetAsync<SingleValue<long>>("/test/count?filters=Name@=test 1");
            Assert.Equal(testModels.Count(m => m.Name.Contains("test 1")), count.Value);

            foreach (var id in ids)
                await client.DeleteAsync($"/test/{id}");
        }
        
        [Theory]
        [MemberData(nameof(QueryDataGenerator.GetListOfTestModelsAndComparision), MemberType = typeof(QueryDataGenerator))]
        public async Task Filters(List<TestModel> testModels, KeyValuePair<string, Func<IEnumerable<TestModel>, long>> filter)
        {
            var client = _testServerFixture.TestServer.CreateClient();
            var ids = new List<Guid>();

            foreach (var testModel in testModels)
            {
                var model = await client.PostAsync<TestModel>("/test", testModel);
                ids.Add(model.Id);
            }

            var results = await client.GetAsync<Paginator<TestModel>>($"/test?filters={filter.Key}&pagesize=100");
            Assert.Equal(filter.Value(testModels), results.Items.Count());
            Assert.Equal(results.Total, results.Items.Count());

            foreach (var id in ids)
                await client.DeleteAsync($"/test/{id}");
        }
        
        [Theory]
        [MemberData(nameof(QueryDataGenerator.GetListOfTestModelsAndPagination), MemberType = typeof(QueryDataGenerator))]
        public async Task Pagination(List<TestModel> testModels, int pageSize)
        {
            var client = _testServerFixture.TestServer.CreateClient();
            var ids = new List<Guid>();

            foreach (var testModel in testModels)
            {
                var model = await client.PostAsync<TestModel>("/test", testModel);
                ids.Add(model.Id);
            }

            var pages = testModels.Count / pageSize;
            if (testModels.Count % pageSize > 0)
                pages++;
            
            for (var i = 0; i < pages; i++)
            {
                var results = await client.GetAsync<Paginator<TestModel>>($"/test?pagesize={pageSize}&page={i+1}");
                Assert.Equal(i + 1, results.Page);
                Assert.Equal(testModels.Count, results.Total);
                Assert.Equal(pageSize, results.PageSize);
                Assert.Equal(i > 0, results.HavePrev);
                Assert.Equal(i < pages - 1, results.HaveNext);
            }

            foreach (var id in ids)
                await client.DeleteAsync($"/test/{id}");
        }
        
        [Theory]
        [MemberData(nameof(QueryDataGenerator.GetListOfTestModels), MemberType = typeof(QueryDataGenerator))]
        public async Task Sort(List<TestModel> testModels)
        {
            var client = _testServerFixture.TestServer.CreateClient();
            var ids = new List<Guid>();

            foreach (var testModel in testModels)
            {
                var model = await client.PostAsync<TestModel>("/test", testModel);
                testModel.Id = model.Id;
                ids.Add(model.Id);
            }

            var result1 = await client.GetAsync<Paginator<TestModel>>("/test?pagesize=100&sorts=TestDateTime");
            Assert.Equal(testModels.Count, result1.Items.Count());
            var index = 0;
            var sortedList = testModels.OrderBy(t => t.TestDateTime).ToList();
            foreach (var item in result1.Items)
            {
                Assert.Equal(sortedList[index].Id, item.Id);
                index++;
            }

            var result2 = await client.GetAsync<Paginator<TestModel>>("/test?pagesize=100&sorts=-TestDateTime");
            Assert.Equal(testModels.Count, result2.Items.Count());
            index = 0;
            sortedList = testModels.OrderByDescending(t => t.TestDateTime).ToList();
            foreach (var item in result2.Items)
            {
                Assert.Equal(sortedList[index].Id, item.Id);
                index++;
            }

            foreach (var id in ids)
                await client.DeleteAsync($"/test/{id}");
        }
        
        //[Theory]
        //[MemberData(nameof(QueryDataGenerator.GetTestModels), MemberType = typeof(QueryDataGenerator))]
        [Fact]
        public async Task Include()
        {
            var client = _testServerFixture.TestServer.CreateClient();
            var testModel = new TestModel
            {
                Name = "Test A",
                Child1 = new TestChildModel
                {
                    Name = "Test B",
                    Child = new TestChildModel2
                    {
                        Name = "Test C"
                    }
                },
                Child2 = new TestChildModel2
                {
                    Name = "Test D"
                }
            };
            
            // POST
            var testModelPostResult = await client.PostAsync<TestModel>("/test", testModel);

            // GET
            var testModelResult = await client.GetAsync<TestModel>($"/test/{testModelPostResult.Id}?includes=Child1.Child,Child2");
            Assert.Equal(testModel.Name, testModelResult.Name);
            Assert.Equal(testModel.Child1.Name, testModelResult.Child1.Name);
            Assert.Equal(testModel.Child1.Child.Name, testModelResult.Child1.Child.Name);
            Assert.Equal(testModel.Child2.Name, testModelResult.Child2.Name);
        }
    }
    */
}