using System;
using System.Linq;
using System.Collections.Generic;

namespace TypeDictionary
{
    public sealed class TypeDictionaryC<T> : ITypeDictionary<T>
    {
        private readonly Dictionary<Type, List<T>> _map = new Dictionary<Type, List<T>>();
        public void Add<TSub>(TSub item) where TSub : T
        {
            Type key = typeof(TSub);
            if(_map.TryGetValue(key, out var m)) {
                m.Add(item);
                return;
            }
            _map[key] = new List<T> { item };
        }
        public void Add<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            Type key = typeof(TSub);
            if(!_map.TryGetValue(key, out var m)) {
                m = new List<T>();
                _map[key] = m;

            }
            foreach(var item in items) {
                m.Add(item);
            }
        }
        public IEnumerable<TSub> Get<TSub>() where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m)) {
                return m.Cast<TSub>();
            }
            return Array.Empty<TSub>();
        }
        public void Remove<TSub>(TSub item) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m)) {
                m.Remove(item);
            }
        }
        public void Remove<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m)) {
                foreach(var item in items) {
                    m.Remove(item);
                }
            }
        }
        public void Clear() => _map.Clear();
    }
}