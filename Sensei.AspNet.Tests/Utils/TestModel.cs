using System;
using System.Security.Claims;
using Sensei.AspNet.Attributes;
using Sensei.AspNet.DbProcessor;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.Utils
{
    public class TestModel : BaseModel
    {
        public string Name { get; set; }
        
        public bool TestFlag { get; set; }
        
        public DateTime TestDateTime { get; set; }
        
        public int TestInt { get; set; }
        
        public int? TestInt2 { get; set; }

        public decimal TestDecimal { get; set; }
        
        public decimal? TestDecimal2 { get; set; }

        [PopulateWithUserId(DbEntityState.Added)]
        public Guid UserIdAdded { get; set; }

        [PopulateWithUserId(DbEntityState.Added, true)]
        public Guid UserIdAddedSkipIfExist { get; set; }

        [PopulateWithUserId(DbEntityState.Added, true)]
        public Guid UserIdAddedSkipIfExist2 { get; set; }

        [PopulateWithUserId(DbEntityState.Modified)]
        public Guid UserIdModified { get; set; }
        
        [PopulateWithClaim(DbEntityState.Added, ClaimTypes.Email)]
        public string ClaimAdded { get; set; }

        [PopulateWithClaim(DbEntityState.Added, ClaimTypes.Email, true)]
        public string ClaimAddedSkipIfExist { get; set; }

        [PopulateWithClaim(DbEntityState.Added, ClaimTypes.Email, true)]
        public string ClaimAddedSkipIfExist2 { get; set; }

        [PopulateWithClaim(DbEntityState.Modified, ClaimTypes.Email)]
        public string ClaimModified { get; set; }
        
        public TestChildModel Child1 { get; set; }
        
        public TestChildModel2 Child2 { get; set; }
    }
}