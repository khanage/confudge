using System;

namespace Confudge.Errors
{
    internal class DefaultCreatorException : Exception
    {
        public DefaultCreatorException(string message, params object[] formatObjects) 
            : base(string.Format(message, formatObjects))
        {
            
        }
    }
}