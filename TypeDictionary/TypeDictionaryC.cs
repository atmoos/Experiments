using System;
using System.Collections;
using System.Collections.Generic;

namespace TypeDictionary
{
    public sealed class TypeDictionaryC<T> : ITypeDictionary<T>
    {
        private readonly Dictionary<Type, Map<T>> _map = new Dictionary<Type, Map<T>>();
        public void Add<TSub>(TSub item) where TSub : T
        {
            Type key = typeof(TSub);
            if(_map.TryGetValue(key, out var m) && m is SubMap<TSub> s) {
                s.Add(item);
                return;
            }
            _map[key] = new SubMap<TSub> { item };
        }
        public void Add<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            Type key = typeof(TSub);
            if(_map.TryGetValue(key, out var m) && m is SubMap<TSub> s) {
                s.Add(items);
                return;
            }
            _map[key] = new SubMap<TSub> { items };
        }
        public IEnumerable<TSub> Get<TSub>() where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m) && m is SubMap<TSub> s) {
                return s;
            }
            return Array.Empty<TSub>();
        }
        public void Remove<TSub>(TSub item) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m) && m is SubMap<TSub> s) {
                s.Remove(item);
            }
        }
        public void Remove<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            if(_map.TryGetValue(typeof(TSub), out var m) && m is SubMap<TSub> s) {
                s.Remove(items);
            }
        }
        public void Clear() => _map.Clear();
        private abstract class Map<TItem> { }
        private sealed class SubMap<TSub> : Map<T>, IEnumerable<TSub> where TSub : T
        {
            private readonly List<TSub> _subMap = new List<TSub>();
            public void Add(TSub item) => _subMap.Add(item);
            public void Add(IEnumerable<TSub> items) => _subMap.AddRange(items);
            public void Remove(TSub item) => _subMap.Remove(item);
            public void Remove(IEnumerable<TSub> items)
            {
                foreach(TSub item in items) {
                    _subMap.Remove(item);
                }
            }
            public IEnumerator<TSub> GetEnumerator() => _subMap.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}