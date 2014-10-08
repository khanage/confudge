using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Bearded.Monads;
using Confudge.Construction;
using Confudge.Errors;
using Confudge.Machinery;

namespace Confudge
{
    public static class Confudge
    {
        static Registrations registrations;
        static XDocument configurationDocument;
        static Creator creator = new DefaultCreator();

        static readonly BindingFlags flagsForPublicProperties = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance;

        public static void Bootstrap(object configurationSection, Action<Registrations> bootstrapping)
        {
            if (!(configurationSection is XDocument))
                throw new YouAreAClownException("Configuration object was not an XDocument");

            Bootstrap(configurationSection as XDocument, bootstrapping);
        }

        public static void Bootstrap(XDocument doc, Action<Registrations> bootstrapping)
        {
            configurationDocument = doc;

            registrations = new Registrations();
            bootstrapping(registrations);
        }

        public static Option<A> Load<A>()
        {
            return
                from registration in registrations.Find<A>()
                from creation in creator.Create<A>()
                from mapped in AppliedMappings(registration, creation)
                select mapped;
        }

        public static Creator Creator { set { creator = value; } }

        static Option<A> AppliedMappings<A>(RegistrationConfiguration<A> registration, A creation)
        {
            return TryFindSectionInDocument<A>().Map(elem =>
            {
                var publiclySettableProperties = typeof(A).GetProperties(flagsForPublicProperties);

                foreach (var property in publiclySettableProperties)
                {
                    // TODO: Test for collection here
                    MaybeSetValue(registration, creation, elem, property);
                }

                return creation;
            });
        }

        static void MaybeSetValue<A>(RegistrationConfiguration<A> registration, A creation, XElement elem, PropertyInfo property)
        {
            var propertyInElement = elem.Descendants(property.Name).SingleOrNone();
            var parsedProperty = propertyInElement.SelectMany(ParseForProperty(property, registration));

            parsedProperty.Do(value => property.SetValue(creation, value, null),
                              ()    => ThrowMaybe(registration.ShouldThrowOnFailure, property.Name));
        }

        static Func<XElement, Option<object>> ParseForProperty<A>(PropertyInfo property, RegistrationConfiguration<A> registration)
        {
            return item => registration
                .GetBinding(property.Name, property.PropertyType)
                .SelectMany(callback => callback(item.Value));
        }

        static Option<XElement> TryFindSectionInDocument<A>()
        {
            return configurationDocument.Document.Descendants(typeof (A).Name).FirstOrDefault().NoneIfNull();
        }

        static void ThrowMaybe(bool shouldThrowOnFailure, string propertyName)
        {
            if (shouldThrowOnFailure)
                throw new ConfudgeException("Found property {0} without a registration", propertyName);
        }
    }
}