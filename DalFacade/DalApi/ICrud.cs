using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface ICrud<T> where T : class
    {
        int Create(T item);

        T? Read(int id);

        // stage 2 - read by condition
        T? Read(Func<T, bool> filter);

        //stage 2 - return ienumerator of objects by condition
        IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);
        void Update(T item);

        void Delete(int id);

        //reset for each entity collection
        void Reset();
    }
}
