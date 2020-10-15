using System;

namespace Sensei.AspNet.Queries
{
    public class QueryProperty
    {
        private bool _canFilter;
        private bool _canSort;
        private bool _canInclude;

        internal bool GetValue(QueryType queryType)
        {
            switch (queryType)
            {
                case QueryType.Filters:
                    return _canFilter;
                case QueryType.Sorts:
                    return _canSort;
                case QueryType.Includes:
                    return _canInclude;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryType), queryType, null);
            }
        }

        public QueryProperty CanFilter(bool enabled = true)
        {
            _canFilter = enabled;
            return this;
        }
        
        public QueryProperty CanSort(bool enabled = true)
        {
            _canSort = enabled;
            return this;
        }

        public QueryProperty CanInclude(bool enabled = true)
        {
            _canInclude = enabled;
            return this;
        }
    }
}