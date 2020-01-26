using System;
using System.Collections.Generic;

namespace TypeDictionary
{
    public sealed class TypeDictionaryD<T> : ITypeDictionary<T>
    {
        // T -> List<T>
        private readonly Dictionary<Type, dynamic> _map = new Dictionary<Type, dynamic>();
        public void Add<TSub>(TSub item) where TSub : T
        {
            Type key = typeof(TSub);
            if(_map.TryGetValue(key, out dynamic m)) {
                m.Add(item);
                return;
            }
            _map[key] = new List<TSub> { item };
        }
        public void Add<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            Type key = typeof(TSub);
            if(_map.TryGetValue(key, out dynamic m)) {
                m.AddRange(items);
                return;
            }
            _map[key] = new List<TSub>(items);
        }
        public IEnumerable<TSub> Get<TSub>() where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out dynamic m)) {
                return m;
            }
            return Array.Empty<TSub>();
        }
        public void Remove<TSub>(TSub item) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out dynamic m)) {
                m.Remove(item);
            }
        }
        public void Remove<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out dynamic m)) {
                foreach(TSub item in items) {
                    m.Remove(item);
                }
            }
        }
        public void Clear() => _map.Clear();

    }
}