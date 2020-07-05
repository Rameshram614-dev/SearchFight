using System;

namespace SearchFight.Common.Exceptions
{
    public class SearchFightException : Exception
    {
        public SearchFightException(string message) : base(message) { }
        public SearchFightException(string message, Exception innerException) : base(message, innerException) { }
    }
}
