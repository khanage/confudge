using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class IntParser : BoxedParser, DefaultMapper
    {
        public Option<object> Parse(string value)
        {
            return value.MaybeInt().Select(o => (object) o);
        }

        public Option<BoxedParser> ParserFor(Type type)
        {
            if (type == typeof(int))
                return this;

            return Option<BoxedParser>.None;
        }

        static readonly IntParser intParser = new IntParser();
        public static DefaultMapper Instance { get { return intParser; } }
    }
}