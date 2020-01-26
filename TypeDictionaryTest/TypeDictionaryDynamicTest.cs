using System;
using TypeDictionary;

namespace TypeDictionaryTest
{
    public sealed class TypeDictionaryDynamicTest : Tester
    {
        public TypeDictionaryDynamicTest() : base(new TypeDictionaryD<Object>()) { }
    }
}