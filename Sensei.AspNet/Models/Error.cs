using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Sensei.AspNet.Models
{
    /// <summary>
    ///     Generic error response
    /// </summary>
    public class Error
    {
        /// <summary>
        ///     The code that identify the error
        /// </summary>
        [SwaggerSchema("The error code")]
        public int Code { get; set; }

        /// <summary>
        ///     The error message
        /// </summary>
        [SwaggerSchema("The error message")]
        public string Message { get; set; }

        /// <summary>
        ///     A detailed description of the error
        /// </summary>
        [SwaggerSchema("The error description")]
        public string Description { get; set; }
    }
}