using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    public interface BoxedParser
    {
        Option<object> Parse(string value);
    }

    public abstract class DefaultBoxedParserBase<A> : BoxedParser
    {
        protected abstract Option<A> ParseWellTyped(string value);

        public Option<object> Parse(string value)
        {
            var maybeRes = ParseWellTyped(value);

            // We don't want to set a default value to empty
            // in case the value has a default value
            if(maybeRes.ElseDefault().Equals(default(A)))
                return Option<object>.None;

            // This is neccesarry as classes don't support
            // Contravariance in their type args
            return maybeRes.Select(o => (object) o);
        }
    }
}