using System;

namespace Sensei.AspNet.Queries.Exceptions
{
    public class EvaluateFilterException : Exception
    {
        internal EvaluateFilterException(string message)
            : base(message)
        {
        }
    }
}