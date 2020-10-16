using System;

namespace Sensei.AspNet.Queries.Exceptions
{
    public class QueryContextInvalidException : Exception
    {
        internal QueryContextInvalidException(string message)
            : base(message)
        {
        }
    }
}