using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class FloatParser : DefaultBoxedParserBase<float>, DefaultMapper
    {
        protected override Option<float> ParseWellTyped(string value)
        {
            return value.MaybeFloat();
        }

        static readonly FloatParser floatParser = new FloatParser();
        public static DefaultMapper Instance { get { return floatParser; } }
        public Option<BoxedParser> ParserFor(Type type)
        {
            if (type == typeof(float))
                return this;

            return Option<BoxedParser>.None;
        }
    }
}