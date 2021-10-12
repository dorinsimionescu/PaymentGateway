using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions
{
   public interface IWriteOperation<T>
    {
        void PerformOperation(T operation);
    }
}
