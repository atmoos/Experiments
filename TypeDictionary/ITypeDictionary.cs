using System.Collections.Generic;

namespace TypeDictionary
{
    public interface ITypeDictionary<in T>
    {
        void Add<TSub>(TSub item) where TSub : T;
        void Add<TSub>(IEnumerable<TSub> items) where TSub : T;
        IEnumerable<TSub> Get<TSub>() where TSub : T;
        void Remove<TSub>(TSub item) where TSub : T;
        void Remove<TSub>(IEnumerable<TSub> items) where TSub : T;
        void Clear();
    }
}
