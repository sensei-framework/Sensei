using System;

namespace Sensei.AspNet.Queries.Exceptions
{
    public class MissingFilterException : Exception
    {
        internal MissingFilterException()
        {
        }

        internal MissingFilterException(string message)
            : base(message)
        {
        }

        internal MissingFilterException(string message, Exception inner)
            : base(message, inner)
        {
        }    
    }
}