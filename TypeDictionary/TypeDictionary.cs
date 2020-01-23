using System;
using System.Collections.Generic;

namespace TypeDictionary
{
    public sealed class TypeDictionary<T> : ITypeDictionary<T>, IDisposable
    {
        private readonly List<IDisposable> _tokens = new List<IDisposable>();
        public void Add<TSub>(TSub item) where TSub : T => Add(Map<TSub>.Add(this, item));
        public void Add<TSub>(IEnumerable<TSub> items) where TSub : T => Add(Map<TSub>.Add(this, items));
        private void Add(IDisposable token)
        {
            if(token != null) _tokens.Add(token);
        }
        public IEnumerable<TSub> Get<TSub>() where TSub : T => Map<TSub>.Get(this);
        public void Remove<TSub>(TSub item) where TSub : T
        {
            List<TSub> store = Map<TSub>.Get(this);
            if(store == Map<TSub>.Empty) {
                return;
            }
            store.Remove(item);
        }
        public void Remove<TSub>(IEnumerable<TSub> items) where TSub : T
        {
            List<TSub> store = Map<TSub>.Get(this);
            if(store == Map<TSub>.Empty) {
                return;
            }
            foreach(TSub item in items) {
                store.Remove(item);
            }
        }
        public void Clear()
        {
            foreach(IDisposable token in _tokens) {
                token.Dispose();
            }
            _tokens.Clear();
        }
        public void Dispose() => Clear();
        private static class Map<TSub>
        {
            public static List<TSub> Empty { get; } = new List<TSub>(0);
            private static readonly Dictionary<TypeDictionary<T>, List<TSub>> _dictionary = new Dictionary<TypeDictionary<T>, List<TSub>>();
            public static IDisposable Add(TypeDictionary<T> key, TSub item)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    list.Add(item);
                    return null;
                }
                _dictionary[key] = new List<TSub> { item };
                return new ClearToken(key);
            }
            public static IDisposable Add(TypeDictionary<T> key, IEnumerable<TSub> items)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    list.AddRange(items);
                    return null;
                }
                _dictionary[key] = new List<TSub>(items);
                return new ClearToken(key);
            }
            public static List<TSub> Get(TypeDictionary<T> key)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    return list;
                }
                return Empty;
            }
            private sealed class ClearToken : IDisposable
            {
                private readonly TypeDictionary<T> _key;
                public ClearToken(TypeDictionary<T> key) => _key = key;
                public void Dispose() => _dictionary.Remove(_key);
            }
        }
    }
}
