using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Joinrpg.Common.Helpers
{
    public static class TaskHelpers
    {
        public static Func<T1, Task<TOut>> SyncTask<T1, TOut>(Func<T1, TOut> mapper) where TOut : class
        {
            return i1 => Task.FromResult(mapper(i1));
        }
    }
}
