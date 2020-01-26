using System;
using System.Linq;
using Xunit;
using TypeDictionary;

namespace TypeDictionaryTest
{
    public abstract class Tester
    {
        private readonly ITypeDictionary<Object> _map;
        protected Tester(ITypeDictionary<Object> map) => _map = map;
        [Fact]
        public void AdditionAddsAnElement()
        {
            String item = "Hello Item!";
            _map.Add(item);
            Assert.Equal(new[] { item }, _map.Get<String>());
        }
        [Fact]
        public void AdditionAddsElements()
        {
            String chars = "Hello Items!";
            _map.Add<Char>(chars);
            Assert.Equal(chars, _map.Get<Char>());
        }
        [Fact]
        public void GetRetunrsEmptyEnumerableWhenNothingWasAdded() => AssertKeyIsEmpty<Action<Guid>>();
        [Fact]
        public void RemoveRemovesOnlyASingleElement()
        {
            String chars = "Hello Items!";
            _map.Add<Char>(chars);
            _map.Remove('l');
            String expected = "Helo Items!";
            Assert.Equal(expected, _map.Get<Char>());
        }
        [Fact]
        public void RemoveEnumerableRemovesAllElements()
        {
            String chars = "Hello Items!";
            _map.Add<Char>(chars);
            _map.Remove<Char>("elm");
            String expected = "Ho Its!";
            Assert.Equal(expected, _map.Get<Char>());
        }
        [Fact]
        public void ClearEmptiesMapCompletely()
        {
            _map.Add("Something");
            _map.Add(Math.PI);
            _map.Add<Object>("else");
            _map.Add<Func<Int32, Int64>>(i => i * i);
            _map.Clear();
            AssertKeyIsEmpty<Func<Int32, Int64>>();
            AssertKeyIsEmpty<Double>();
            AssertKeyIsEmpty<String>();
            AssertKeyIsEmpty<Object>();
        }
        [Fact]
        public void TypeArgumentIsUsedAsDictionaryKey()
        {
            String text = "Here be string";
            _map.Add(text);
            _map.Add<Object>("Some other text");
            Assert.Equal(new[] { text }, _map.Get<String>());
        }
        [Fact]
        public void GenericTypeArgumentsAreResolved()
        {
            Action<String> item = s => GC.KeepAlive(s);
            _map.Add<Action>(() => { });
            _map.Add(item);
            _map.Add<Action<String, Int64>>((s, i) => GC.KeepAlive($"{s},{i}"));
            Assert.Equal(new[] { item }, _map.Get<Action<String>>());
        }
        private void AssertKeyIsEmpty<TKey>() => Assert.Equal(Enumerable.Empty<TKey>(), _map.Get<TKey>());
    }
}
