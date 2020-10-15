using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.Queries.Entities
{
    /// <summary>
    ///     Sorting model for HTTP requests
    /// </summary>
    public class Sorting
    {
        /// <summary>
        ///     A comma-delimited list with sorting rules.
        /// </summary>
        [SwaggerParameter(
            "Is a comma-delimited list.")]
        public string Sorts { get; set; }
    }
}