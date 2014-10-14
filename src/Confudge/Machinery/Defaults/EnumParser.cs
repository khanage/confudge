using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class EnumParser : DefaultMapper
    {
        static readonly Type parserType = typeof(TypedEnumParser<>);
        static readonly EnumParser enumMapper = new EnumParser();

        public static DefaultMapper EnumMapper { get { return enumMapper; } }

        public Option<BoxedParser> ParserFor(Type type)
        {
            if (typeof(Enum).IsAssignableFrom(type))
                return CreateParser(type).AsOption();

            return Option<BoxedParser>.None;
        }

        public static BoxedParser CreateParser(Type type)
        {
            var realisedType = parserType.MakeGenericType(type);

            return realisedType.GetConstructor(new Type[0]).Invoke(new object[0]) as BoxedParser;
        }

        class TypedEnumParser<A> : DefaultBoxedParserBase<A> where A : struct
        {
            protected override Option<A> ParseWellTyped(string value)
            {
                return value.MaybeEnum<A>();
            }
        }
    }
}