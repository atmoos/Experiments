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
                BenchmarkRunner.Run<TypeDictBenchmark<TypeDictionary<Object>>>(),
                BenchmarkRunner.Run<TypeDictBenchmark<TypeDictionaryC<Object>>>(),
                BenchmarkRunner.Run<TypeDictBenchmark<TypeDictionaryD<Object>>>()};
        }
    }
}
