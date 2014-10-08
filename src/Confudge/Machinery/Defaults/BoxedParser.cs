using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    public interface BoxedParser
    {
        Option<object> Parse(string value);
    }
}