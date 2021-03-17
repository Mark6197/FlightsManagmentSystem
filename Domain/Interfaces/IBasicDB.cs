using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IBasicDB<T> where T : IPoco
    {
        T Get(int id);
        IList<T> GetAll();
        void Add(T t);
        void Remove(T t);
        void Update(T t);
        List<T1> Run_Generic_SP<T1>(string sp_name, object dataHolder) where T1 : new();
    }
}
