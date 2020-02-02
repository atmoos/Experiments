using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace TypeDictionary
{
    public sealed class TypeDictionary<T> : ITypeDictionary<T>, IDisposable
    {
        private readonly List<Token> _tokens = new List<Token>();
        public void Add<TSub>(TSub item) where TSub : T => Add(Map<TSub>.Add(this, item));
        public void Add<TSub>(IEnumerable<TSub> items) where TSub : T => Add(Map<TSub>.Add(this, items));
        private void Add(Token token)
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
            foreach(Token token in _tokens) {
                token.Clear(this);
            }
            _tokens.Clear();
        }
        public void Dispose() => Clear();
        private abstract class Token
        {
            public abstract void Clear(TypeDictionary<T> dict);
        }
        private static class Map<TSub>
        {
            public static List<TSub> Empty { get; } = new List<TSub>(0);
            private static readonly ConcurrentDictionary<TypeDictionary<T>, List<TSub>> _dictionary = new ConcurrentDictionary<TypeDictionary<T>, List<TSub>>();
            public static Token Add(TypeDictionary<T> key, TSub item)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    list.Add(item);
                    return null;
                }
                _dictionary[key] = new List<TSub> { item };
                return new ClearToken();
            }
            public static Token Add(TypeDictionary<T> key, IEnumerable<TSub> items)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    list.AddRange(items);
                    return null;
                }
                _dictionary[key] = new List<TSub>(items);
                return new ClearToken();
            }
            public static List<TSub> Get(TypeDictionary<T> key)
            {
                if(_dictionary.TryGetValue(key, out var list)) {
                    return list;
                }
                return Empty;
            }
            private sealed class ClearToken : Token
            {
                public override void Clear(TypeDictionary<T> dict) => _dictionary.TryRemove(dict, out var _);
            }
        }
    }
}
