using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bearded.Monads;
using Confudge.Machinery.Defaults;

namespace Confudge.Machinery
{
    public delegate Option<object> Parser(string data);

    public class RegistrationConfiguration<Registratee>
    {
        readonly Dictionary<string, BoxedParser> specificRegistrations;
        readonly DefaultMapper defaultMapper;

        public RegistrationConfiguration()
        {
            specificRegistrations = new Dictionary<string, BoxedParser>();
            defaultMapper = new AllDefaultMappers();
        }

        public SpecificParser<A> For<A>(Expression<Func<Registratee, A>> lookupExpression)
        {
            var name = DetermineNameOfProperty(lookupExpression);
            var specificParser = new SpecificParser<A>(this);

            specificRegistrations.Add(name, specificParser);

            return specificParser;
        }

        public Option<Parser> GetBinding(string name, Type propertyType)
        {
            var specificLookup = specificRegistrations
                .MaybeGetValue(name)
                .Select(AsParser);

            return specificLookup ? specificLookup 
                : UseDefault(propertyType);
        }

        Option<Parser> UseDefault(Type propertyType)
        {
            return defaultMapper.ParserFor(propertyType).Select(AsParser);
        }

        Parser AsParser(BoxedParser boxedParser)
        {
            return boxedParser.Parse;
        }

        public bool ShouldThrowOnFailure { get; set; }

        string DetermineNameOfProperty<A>(Expression<Func<Registratee, A>> lookupExpression)
        {
            return ((MemberExpression)lookupExpression.Body).Member.Name;
        }

        public class SpecificParser<A> : BoxedParser
        {
            readonly RegistrationConfiguration<Registratee> parent;
            Func<string, Option<A>> boxedFunction;

            public SpecificParser(RegistrationConfiguration<Registratee> parent)
            {
                this.parent = parent;
            }

            public RegistrationConfiguration<Registratee> Use(Func<string, Option<A>> func)
            {
                boxedFunction = func;
                return parent;
            }

            public Option<object> Parse(string value)
            {
                return boxedFunction(value)
                    .Select(val => (object)val);
            }
        }
    }
}