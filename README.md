# Experimental C#
This repository contains experiments revolving around the language C#. As it is experimental stuff, use in production code is discouraged.

The entire contents is licensed under the [MIT licence](./LICENSE), so these experiments are free for anybody to use as they may see fit.

## Type Dictionary
I worked on a project for a customer where there was need to have a dictionary that used a type argument as a dictionary. Thus mapping type arguments to dictionary keys. The interface [ITypeDictionary](./TypeDictionary/ITypeDictionary.cs) sums up the requirements. Justification for that interface shall not be given here, though.

In essence, an add and get method where required. But the key would not be an instance of a type T but the type or subtype of T. Illustrated next comparing the signature of a standard dictionary to the idea of a typed dictionary:

```language csharp
public void Add(TKey key, TValue value) // Standard dictionary Add

public void Add<TValue>(TValue) // Typed dictionary
```
The corresponding get method of the typed dictionary was chosen to return an enumerable of T. (In effect making the dictionary a "multi-map").

```language csharp
public IEnumearable<TValue> Get<TValue>()

```

This interface can be implemented in a number of ways. The interesting problem is how to map the keys to values, such that the compiler will accept the source code and the functionality can be implemented without introducing to much performance penalty.

Three solutions are provided here, all of which have interesting properties and tradeoffs.

### Completely type safe implementation [TypeDictionary&lt;T&gt;](./TypeDictionary/TypeDictionary.cs)
This implementation prompted me to publish this as a experiment. The trick is to transform the need to somehow get hold of a type key to the standard use case of dictionaries where an instance of a type is required as key. The most significant caveat is that we jeopardize C#'s memory management and have to do it ourselves. It's so bad that instances of [TypeDictionary](./TypeDictionary/TypeDictionary.cs) will **never be garbage collected** if Dispose is not called.

**Advantages**:
- Completely type-safe, no up- or down-casts.
- Comparatively simple implementation.
- Benchmarks show that is most efficient of the three.
- An interesting example on how to leverage C#'s powerful type system and implementation of generics.

**Caveats**:
- Memory has to managed by hand -&gt; IDisposable required.
- Implementation must be thread safe.
- Not trivial to understand why the solution works at all.
- Supporting nested classes required

### Implementation relying on downcasts [TypeDictionaryC&lt;T&gt;](./TypeDictionary/TypeDictionaryC.cs)

**Advantages**:
- Easy to read the code.
- Type safe implementation. (Even though down casts are required)

**Caveats**:
- Use of downcasts.
- Supporting nested classes required.
    - Required to upcast to base type.
- Slower than [TypeDictionary&lt;T&gt;](./TypeDictionary/TypeDictionary.cs), on par with [TypeDictionaryD&lt;T&gt;](./TypeDictionary/TypeDictionaryD.cs)

### Implementation circumventing the type system altogether [TypeDictionaryD&lt;T&gt;](./TypeDictionary/TypeDictionaryD.cs)

**Advantages**:
- Likely to be comparatively efficient.
    - C# has an efficient way of dealing with the dynamic type.
- No explicit casting required.
    - (Using "dynamic" may be considered casting non the less..)

**Caveats**:
- Code hard to read due to use of the dynamic pseudo type.
- Prone to refactoring errors.
- Slower than [TypeDictionary&lt;T&gt;](./TypeDictionary/TypeDictionary.cs), on par with [TypeDictionaryC&lt;T&gt;](./TypeDictionary/TypeDictionaryC.cs)

### Performance Measurements
Performance has been tested with Benchmark .Net where three benchmarks defined in [TypeDictBenchmark.cs](./TypeDictionaryPerformanceTest/TypeDictBenchmark.cs) were executed.

#### TypeDictionary&lt;T&gt;
|              Method |      Mean |    Error |   StdDev |    Median |
|-------------------- |----------:|---------:|---------:|----------:|
|        AddValueType |  90.78 us | 1.799 us | 4.239 us |  92.29 us |
|    AddReferenceType |  99.67 us | 1.405 us | 1.314 us |  99.84 us |
| AddInterleavedTypes | 178.89 us | 2.957 us | 2.766 us | 179.15 us |

#### TypeDictionaryC&lt;T&gt;
|              Method |     Mean |   Error |  StdDev |
|-------------------- |---------:|--------:|--------:|
|        AddValueType | 131.9 us | 2.03 us | 1.80 us |
|    AddReferenceType | 103.7 us | 1.15 us | 1.08 us |
| AddInterleavedTypes | 245.6 us | 2.02 us | 1.79 us |

#### TypeDictionaryD&lt;T&gt;
|              Method |     Mean |   Error |  StdDev |
|-------------------- |---------:|--------:|--------:|
|        AddValueType | 120.7 us | 1.22 us | 1.14 us |
|    AddReferenceType | 124.9 us | 1.61 us | 1.50 us |
| AddInterleavedTypes | 240.7 us | 2.22 us | 2.07 us |
