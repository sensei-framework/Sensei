using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.Utils
{
    public class TestChildModel : BaseModel
    {
        public string Name { get; set; }
        
        public TestChildModel2 Child { get; set; }
    }
}