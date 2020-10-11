using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.Models
{
    /// <summary>
    ///     Filters model for HTTP requests
    /// </summary>
    public class Filtering
    {
        /// <summary>
        ///     A comma-delimited ordered list of property names to sort
        /// </summary>
        [SwaggerParameter(
            "Is a comma-delimited ordered list of property names to sort by.")]
        public string Filters { get; set; }
    }
}