using System;

namespace Sensei.AspNet.Queries.Exceptions
{
    public class MissingPropertyException : Exception
    {
        internal MissingPropertyException(string message)
            : base(message)
        {
        }
    }
}