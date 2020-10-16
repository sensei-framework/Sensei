using System;

namespace Sensei.AspNet.Queries.Exceptions
{
    public class LuceneParserException : Exception
    {
        internal LuceneParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}