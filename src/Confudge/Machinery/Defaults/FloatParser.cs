using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class FloatParser : BoxedParser, DefaultMapper
    {
        public Option<object> Parse(string value)
        {
            return value.MaybeFloat().Select(o => (object)o);
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