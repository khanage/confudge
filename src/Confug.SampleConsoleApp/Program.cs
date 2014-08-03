using System;
using System.Configuration;
using Bearded.Monads;

namespace Confudge.SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Confudge.Bootstrap(
                ConfigurationManager.GetSection("confudge"),
                registrations =>
                {
                    registrations.Add<SimpleType>(reg =>
                        reg.For(st => st.Age).Use(s => s.MaybeInt())
                    );

                    registrations.Add<AnotherType>(reg =>
                    {
                        reg.ShouldThrowOnFailure = true;
                        reg.For(at => at.OpinionOfJavascript)
                           .Use(s => s.MaybeEnum<Regard>(true));
                    });
                }
            );

            var simpleType = Confudge.Load<SimpleType>().Value;
            var anotherType = Confudge.Load<AnotherType>().Value;

            Console.WriteLine("Simpletype: {0}", simpleType);
            Console.WriteLine("Anothertype: {0}", anotherType);
        }
    }

    class SimpleType
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", Name, Age);
        }
    }

    class AnotherType
    {
        public string Role { get; set; }
        public Regard OpinionOfJavascript { get; set; }

        public override string ToString()
        {
            return string.Format("Role: {0}, OpinionOfJavascript: {1}", Role, OpinionOfJavascript);
        }
    }

    enum Regard
    {
        Unknown, Low, Average, High, Worship
    }
}
