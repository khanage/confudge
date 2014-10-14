using System;
using Bearded.Monads;

namespace Confudge.Machinery.Defaults
{
    class UriParser : DefaultBoxedParserBase<Uri>, DefaultMapper
    {
        protected override Option<Uri> ParseWellTyped(string value)
        {
            return value.MaybeUri(UriKind.RelativeOrAbsolute);
        }

        static readonly UriParser uriParser = new UriParser();
        public static DefaultMapper Instance { get { return uriParser; } }
        public Option<BoxedParser> ParserFor(Type type)
        {
            if (type == typeof(Uri))
                return this;

            return Option<BoxedParser>.None;
        }
    }
}