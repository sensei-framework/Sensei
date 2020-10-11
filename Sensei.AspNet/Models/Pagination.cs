using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.Models
{
    /// <summary>
    ///     Pagination model for HTTP requests
    /// </summary>
    public class Pagination
    {
        /// <summary>
        ///     The number of page to return
        /// </summary>
        [SwaggerParameter("Is the number of page to return")]
        public int? Page { get; set; }

        /// <summary>
        ///     The number of items returned per page
        /// </summary>
        [SwaggerParameter("Is the number of items returned per page")]
        public int? PageSize { get; set; }
    }
}