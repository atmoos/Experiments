using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using TypeDictionary;

namespace TypeDictionaryPerformanceTest
{
    [RPlotExporter]
    public abstract class TypeDictBenchmark<TDict> where TDict : ITypeDictionary<Object>, new()
    {
        private const Int32 Size = 2 * 1024;
        private static readonly Int32[] _valueData = Enumerable.Range(0, Size).ToArray();
        private static readonly String[] _referenceData = Enumerable.Range(0, Size).Select(n => n.ToString()).ToArray();
        private readonly TDict _dict;
        protected TypeDictBenchmark() => _dict = new TDict();
        [Benchmark]
        public void AddValueType() => Add(_valueData);
        [Benchmark]
        public void AddReferenceType() => Add(_referenceData);
        [Benchmark]
        public void AddInterleavedTypes()
        {
            for(Int32 i = 0; i < _referenceData.Length; ++i) {
                _dict.Add(i);
                _dict.Add(_referenceData[i]);
            }
            _dict.Clear();
        }
        private void Add<TSub>(IEnumerable<TSub> data)
        {
            foreach(TSub datum in data) {
                _dict.Add(datum);
            }
            _dict.Clear();
        }
    }
}
