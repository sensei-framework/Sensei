using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Sensei.AspNet.Models
{
    /// <summary>
    ///     Result model with single value for HTTP responses
    /// </summary>
    public class SingleValue<T>
    {
        /// <summary>
        ///     The value returned
        /// </summary>
        [SwaggerSchema("The value returned")]
        public T Value { get; set; }
    }
}