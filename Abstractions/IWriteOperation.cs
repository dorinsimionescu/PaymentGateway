//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Abstractions
//{
//    public interface IWriteOperation<TCommand> // IRequest<TCommand>
//    {
//        void PerformOperation(TCommand operation);
//    }
//}

namespace Abstractions
{
    public interface ITest<TCommand> // IRequest<TCommand>
    {
        void PerformOperation(TCommand operation);
    }
}