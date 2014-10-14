using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    internal class ClassMapper : DefaultMapper
    {
        static readonly ClassMapper classMapper = new ClassMapper();
        static readonly Type lookupParserType = typeof (ConfudgeLookupConfudgeParser<>);
        public static DefaultMapper Instance { get { return classMapper; } }

        public Option<BoxedParser> ParserFor(Type type)
        {
            if (!type.IsClass)
                return Option<BoxedParser>.None;

            return CreateConfudgeLookupParserFor(type);
        }

        Option<BoxedParser> CreateConfudgeLookupParserFor(Type type)
        {
            var realisedType = lookupParserType.MakeGenericType(type);
            var realisedInstance = realisedType.GetConstructor(new Type[0])
                .Invoke(new object[0])
                .MaybeCast<BoxedParser>();

            return realisedInstance;
        }

        class ConfudgeLookupConfudgeParser<A> : BoxedParser
        {
            public Option<object> Parse(string value)
            {
                return Confudger.Load<A>().Select(a => (object) a);
            }
        }
    }
}