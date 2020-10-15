using System.Security.Claims;

namespace Sensei.AspNet
{
    public class SenseiOptions
    {
        public string UserIdClaim { get; set; } = ClaimTypes.NameIdentifier;
        
        public int PaginationDefaultPageSize { get; set; } = 20;

        public int PaginationMaxPageSize { get; set; } = 100;

        public bool EnableFiltersAsDefault { get; set; } = true;

        public bool EnableSortsAsDefault { get; set; } = true;

        public bool EnableIncludesAsDefault { get; set; } = true;
    }
}