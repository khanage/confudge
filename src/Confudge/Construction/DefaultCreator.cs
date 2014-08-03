using Bearded.Monads;
using Confudge.Errors;

namespace Confudge.Construction
{
    internal class DefaultCreator : Creator
    {
        public Option<A> Create<A>()
        {
            var constructors = typeof(A).GetConstructors();
            if (constructors.Length != 1)
                throw new DefaultCreatorException("You must only have a single parameterless constructor in your types");

            var createdType = (A)constructors[0].Invoke(new object[] { });

            return createdType;
        }
    }
}