using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class IntParser : DefaultBoxedParserBase<int>, DefaultMapper
    {
        protected override Option<int> ParseWellTyped(string value)
        {
            return value.MaybeInt();
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