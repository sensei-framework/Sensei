using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.Queries.Entities
{
    /// <summary>
    ///     Paginator model for HTTP responses
    /// </summary>
    public class Paginator<T>
    {
        /// <summary>
        ///     The number of page returned
        /// </summary>
        [SwaggerSchema("Is the number of page returned")]
        public int Page { get; set; }

        /// <summary>
        ///     The number of items returned per page
        /// </summary>
        [SwaggerSchema("Is the number of items returned per page")]
        public int PageSize { get; set; }

        /// <summary>
        ///     The list of item returned
        /// </summary>
        [SwaggerSchema("Is a list of items returned")]
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        ///     The total number of items without filters
        /// </summary>
        [SwaggerSchema("Is the number of the total items")]
        public int Total { get; set; }

        /// <summary>
        ///     Have a next page
        /// </summary>
        [SwaggerSchema("Have a next page")]
        public bool HaveNext { get; set; }

        /// <summary>
        ///     Have a previous page
        /// </summary>
        [SwaggerSchema("Have a previous page")]
        public bool HavePrev { get; set; }
    }
}