using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class NoOpParser : DefaultBoxedParserBase<string>, DefaultMapper
    {
        protected override Option<string> ParseWellTyped(string value)
        {
            return value;
        }

        public Option<BoxedParser> ParserFor(Type type)
        {
            if (type == typeof(string))
                return this;

            return Option<BoxedParser>.None;
        }

        static readonly NoOpParser noopParser = new NoOpParser();

        public static DefaultMapper Instance { get { return noopParser; } }
    }
}