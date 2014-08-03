using System;
using System.Collections.Generic;
using Bearded.Monads;

namespace Confudge.Machinery
{
    public class Registrations
    {
        readonly Dictionary<string, object> registrations;

        public Registrations()
        {
            registrations = new Dictionary<string, object>();
        }

        public void Add<A>(Action<RegistrationConfiguration<A>> callback = null)
        {
            var registration = new RegistrationConfiguration<A>();

            if (callback != null) callback(registration);

            registrations.Add(typeof (A).FullName, registration);
        }

        public class RegistrationWrapper
        {
            readonly object boxedRegistration;

            public RegistrationWrapper(object boxedRegistration)
            {
                this.boxedRegistration = boxedRegistration;
            }

            public RegistrationConfiguration<A> Unbox<A>()
            {
                return boxedRegistration as RegistrationConfiguration<A>;
            }
        }

        public Option<RegistrationConfiguration<RegisteredType>> Find<RegisteredType>()
        {
            return registrations
                .MaybeGetValue(typeof (RegisteredType).FullName)
                .Cast<RegistrationConfiguration<RegisteredType>>();
        }
    }
}