using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.QueryProcessor.Entities
{
    /// <summary>
    ///     Include model for HTTP requests
    /// </summary>
    public class Including
    {
        /// <summary>
        ///     A comma-delimited list of property to include as child
        /// </summary>
        [SwaggerParameter(
            "Is a comma-delimited list of property to include as child. To include sub properties just use the . character as separator")]
        public string Includes { get; set; }
    }
}