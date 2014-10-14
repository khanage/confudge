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
}