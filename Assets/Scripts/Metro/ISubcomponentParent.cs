using System.Collections.Generic;

namespace Metro
{

    public interface ISubcomponentParent
    {

        IEnumerable<T> Components<T>();

    }

}
