using Bearded.Monads;

namespace Confudge.Construction
{
    public interface Creator
    {
        Option<A> Create<A>();
    }
}