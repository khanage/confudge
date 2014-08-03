using System;

namespace Confudge.Errors
{
    class ConfudgeException : Exception
    {
        public ConfudgeException(string message, params object[] formatObjects)
            : base(string.Format(message, formatObjects))
        {

        }
    }
}