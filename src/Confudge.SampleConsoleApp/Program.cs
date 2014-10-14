using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Bearded.Monads;

namespace Confudge.SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Confudger.Bootstrap(
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

                    registrations.Add<AutoType>();

                    registrations.Add<NestedType>();
                }
            );

            //var simpleType = Confudge.Load<SimpleType>().Value;
            //var anotherType = Confudge.Load<AnotherType>().Value;
            //var autoType = Confudge.Load<AutoType>().Value;
            var nestedType = Confudger.Load<NestedType>().Value;

            //Console.WriteLine("Simpletype: {0}", simpleType);
            //Console.WriteLine("Anothertype: {0}", anotherType);
            //Console.WriteLine("AutoType: {0}", autoType);
            Console.WriteLine("NestedType: {0}", nestedType);
        }
    }

    class AutoType
    {
        public string Role { get; set; }
        public Regard OpinionOfJavascript { get; set; }

        public override string ToString()
        {
            return string.Format("Role: {0}, OpinionOfJavascript: {1}", Role, OpinionOfJavascript);
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

    internal class NestedType
    {
        public int Property { get; set; }
        public Uri Address { get; set; }
        public AnotherType AnotherType { get; set; }
        public List<AutoType> Things { get; set; }

        public NestedType()
        {
            Things = new List<AutoType>();
        }

        public override string ToString()
        {
            var allThings = string.Join("; ", Things.Select(t => t.ToString()).ToArray());

            return string.Format("Property: {0}, Address: {1}, AnotherType: {2}, Things: [{3}]", Property, Address, AnotherType, allThings);
        }
    }

    enum Regard
    {
        Unknown, Low, Average, High, Worship
    }
}
