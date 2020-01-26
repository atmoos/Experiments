using System;
using TypeDictionary;

namespace TypeDictionaryTest
{
    public sealed class TypeDictionaryTest : Tester, IDisposable
    {
        private readonly TypeDictionary<Object> _map;
        public TypeDictionaryTest() : this(new TypeDictionary<Object>()) { }
        private TypeDictionaryTest(TypeDictionary<Object> map) : base(map) => _map = map;
        public void Dispose() => _map.Dispose();
    }
}
