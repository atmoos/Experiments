# Experimental C#
This repository contains experiments revolving around the language C#. As it is experimental stuff, use in production code is discouraged.

The entire contents is licensed under the [MIT licence](./LICENSE), so these experiments are free for anybody to use as they may see fit.

## Type Dictionary
I worked on a project for a customer where there was need to have a dictionary that used a type argument as a dictionary. Thus mapping type arguments to dictionary keys. The interface [ITypeDictionary](./TypeDictionary/ITypeDictionary.cs) sums up the requirements. Justification for that interface shall not be given here, though.

In essence, an add and get method where required. But the key would not be an instance of a type T but the type or subtype of T. Illustrated next comparing the signature of a standard dictionary to the idea of a typed dictionary:

```language csharp
public void Add(TKey key, TValue value) // Standard dictionary Add

public void Add<TValue>(TValue value) // Typed dictionary
```
The corresponding get method of the typed dictionary was chosen to return an enumerable of T. (In effect making the dictionary a "multi-map").

```language csharp
public IEnumearable<TValue> Get<TValue>()

```

This interface can be implemented in a number of ways. The interesting problem is how to map the keys to values, such that the compiler will accept the source code and the functionality can be implemented without introducing to much performance penalty.

Three solutions are provided here, all of which have interesting properties and tradeoffs.

### Completely type safe implementation TypeDictionary&lt;T&gt;
This [implementation](./TypeDictionary/TypeDictionary.cs) prompted me to publish this as a experiment. The trick is to transform the need to somehow get hold of a type key to the standard use case of dictionaries where an instance of a type is required as key. The most significant caveat is that we jeopardize C#'s memory management and have to do it ourselves. It's so bad that instances of [TypeDictionary](./TypeDictionary/TypeDictionary.cs) will **never be garbage collected** if Dispose is not called.

**Advantages**:
- Completely type-safe, no up- or down-casts.
- Comparatively simple implementation.
- likely to be efficient. (ToDo: Proof!).
- An interesting example on how to leverage C#'s powerful type system and implementation of generics.

**Caveats**:
- Memory has to managed by hand -&gt; IDisposable required.
- Implementation must be thread safe.
- Not trivial to understand why the solution works at all.
- Supporting nested classes required

### Implementation relying on downcasts TypeDictionaryC&lt;T&gt;

**Advantages**:
- Easy to read the code.
- Type safe implementation. (Even though down casts are required)

**Caveats**:
- Use of downcasts.
- Slow (ToDo: Proof!)

### Implementation circumventing the type system altogether TypeDictionaryD&lt;T&gt;

**Advantages**:
- Likely to be comparatively efficient.
    - C# has an efficient way of dealing with the dynamic type.
- No explicit casting required.
    - (Using "dynamic" may be considered casting non the less..)

**Caveats**:
- Code hard to read due to use of the dynamic pseudo type.
- Prone to refactoring errors.
