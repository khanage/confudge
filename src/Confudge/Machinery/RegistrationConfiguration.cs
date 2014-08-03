using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bearded.Monads;

namespace Confudge.Machinery
{
    public class RegistrationConfiguration<Registratee>
    {
        Dictionary<string, BoxedParser> specificRegistrations;

        public RegistrationConfiguration()
        {
            specificRegistrations = new Dictionary<string, BoxedParser>();
        }

        public SpecificParser<A> For<A>(Expression<Func<Registratee, A>> lookupExpression)
        {
            var name = DetermineNameOfProperty(lookupExpression);
            var specificParser = new SpecificParser<A>(this);

            specificRegistrations.Add(name, specificParser);

            return specificParser;
        }

        public Option<Func<string, Option<object>>> GetBinding(string name, Type propertyType)
        {
            if (propertyType == typeof(string)) return Option.Return<Func<string, Option<object>>>(Option.Return<object>);

            return specificRegistrations.MaybeGetValue(name)
                .Map<Func<string,Option<object>>>(parser => parser.Parse);
        }

        public bool ShouldThrowOnFailure { get; set; }

        string DetermineNameOfProperty<A>(Expression<Func<Registratee, A>> lookupExpression)
        {
            return ((MemberExpression)lookupExpression.Body).Member.Name;
        }

        public interface BoxedParser
        {
            Option<object> Parse(string value);
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
                    .Map(val => (object)val);
            }
        }
    }
}