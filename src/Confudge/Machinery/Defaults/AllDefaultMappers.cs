using System;
using System.Collections.Generic;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class AllDefaultMappers : DefaultMapper
    {
        readonly List<DefaultMapper> defaultMappers;

        public AllDefaultMappers()
        {
            defaultMappers = new List<DefaultMapper>
            {
                NoOpParser.Instance,
                FloatParser.Instance,
                IntParser.Instance,
                UriParser.Instance,
                EnumParser.EnumMapper,
                ClassMapper.Instance,
            };
        }

        public Option<BoxedParser> ParserFor(Type type)
        {
            return defaultMappers.FirstOrNone(mapper => mapper.ParserFor(type));
        }
    }

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
                return Confudge.Load<A>().Select(a => (object) a);
            }
        }
    }
}