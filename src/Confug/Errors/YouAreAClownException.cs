using System;

namespace Confudge.Errors
{
    public class YouAreAClownException : Exception
    {
        public YouAreAClownException(string message, params object[] formatObjects)
            : base(string.Format(message, formatObjects))
        {

        }
    }
}