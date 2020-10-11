using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensei.AspNet.Tests.Utils
{
    public static class QueryDataGenerator
    {
        public static IEnumerable<object[]> GetListOfTestModels()
        {
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>())
            };
        }

        public static IEnumerable<object[]> GetListOfTestModelsAndComparision()
        {
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2>=243",
                    models => models.Count(m => m.TestInt2 >= 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2>243",
                    models => models.Count(m => m.TestInt2 > 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2==243",
                    models => models.Count(m => m.TestInt2 == 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2!=243",
                    models => models.Count(m => m.TestInt2 != 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2<243",
                    models => models.Count(m => m.TestInt2 < 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2>=243",
                    models => models.Count(m => m.TestInt2 >= 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "TestInt2<=243",
                    models => models.Count(m => m.TestInt2 <= 243)), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "Name==test 1",
                    models => models.Count(m => m.Name == "test 1")), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "Name@=test 1",
                    models => models.Count(m => m.Name.Contains("test 1"))), 
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                new KeyValuePair<string, Func<IEnumerable<TestModel>, long>>(
                    "Name!=test 1",
                    models => models.Count(m => m.Name != "test 1")), 
            };
        }
        
        public static IEnumerable<object[]> GetListOfTestModelsAndPagination()
        {
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                10
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                4
            };
            yield return new object[]
            {
                new List<TestModel>(GetTestModels().Select(m => m.FirstOrDefault()).Cast<TestModel>()),
                999
            };
        }

        public static IEnumerable<object[]> GetTestModels()
        {
            yield return new object[]
            {
                new TestModel
                {
                    Id = Guid.NewGuid(),
                    Name = "test 1",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 13, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 2",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 14, 14, 33, 44),
                    TestInt = 135,
                    TestInt2 = 94,
                    TestDecimal = 4.2m,
                    TestDecimal2 = null
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 3",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,4, 4, 15, 33, 44),
                    TestInt = 5764,
                    TestInt2 = null,
                    TestDecimal = 41.321m,
                    TestDecimal2 = 42m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 4",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 16, 33, 44),
                    TestInt = 13,
                    TestInt2 = 243,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 5",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 17, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 6",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 18, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 7",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 19, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 8",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 20, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 9",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 21, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 10",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 22, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 11",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 23, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 12",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 0, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 13",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 1, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
            yield return new object[]
            {
                new TestModel
                {
                    Name = "test 14",
                    TestFlag = true,
                    TestDateTime = new DateTime(2020,2, 4, 2, 33, 44),
                    TestInt = 13,
                    TestInt2 = 934,
                    TestDecimal = 444.34m,
                    TestDecimal2 = 355m
                }
            };
        }
    }
}