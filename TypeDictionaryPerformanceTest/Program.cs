using System;
using BenchmarkDotNet.Running;
using TypeDictionary;

namespace TypeDictionaryPerformanceTest
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var summaries = new[]{
                BenchmarkRunner.Run<AddBenchmark<TypeDictionary<Object>>>(),
                BenchmarkRunner.Run<AddBenchmark<TypeDictionaryC<Object>>>(),
                BenchmarkRunner.Run<AddBenchmark<TypeDictionaryD<Object>>>()};
        }
    }
}
