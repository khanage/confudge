using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    interface DefaultMapper
    {
        Option<BoxedParser> ParserFor(Type type);
    }
}