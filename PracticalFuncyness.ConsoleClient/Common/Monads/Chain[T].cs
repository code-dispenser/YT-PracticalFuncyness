namespace PracticalFuncyness.ConsoleClient.Common.Monads;

/*
    * This Chain<T> class represents a container that holds a single value, and it is designed to follow the Monad principles.
    *
    * A Monad is a container that allows chaining of operations, where each operation produces a new container 
    * while preserving the structure of the monad. In functional programming, Monads must adhere to three key laws:
    *
    *   1. Left Identity:   Return(a).Bind(f) == f(a)
    *      - A monadic value wrapped in `Return` (or a constructor) can be bound to a function `f` and the result 
    *        should be the same as if `f(a)` was directly applied to `a`.
    *
    *   2. Right Identity:  chain.Bind(Return) == chain
    *      - Binding a monadic value to the `Return` function (which re-wraps the value) should result in the same monad.
    *
    *   3. Associativity:   chain.Bind(f).Bind(g) == chain.Bind(x => f(x).Bind(g))
    *      - Chaining multiple `Bind` calls should be associative; i.e., the order of the function application 
    *        should not affect the result.
    *
    * The `Return` function is essential for creating the monad and must adhere to the Left Identity and Right Identity laws.
    *
    * The `Map` function is not strictly required for a Monad, but it is commonly added to provide an easy way to transform 
    * the value inside the monad. It allows for a more functional programming style, enabling the use of `Map` to apply 
    * transformations to the monadic value without breaking the monadic structure.
*/
internal record class Chain<T>
{
    public T Value { get; }


    public Chain(T value) => Value = value;


    public static Chain<T> Return(T value) => new Chain<T>(value);//<<< We could also use the implicit operator to satisfy this.

    public Chain<TOut> Bind<TOut>(Func<T, Chain<TOut>> bindFn)

        => bindFn(Value);

    public Chain<TOut> Map<TOut>(Func<T, TOut> mapFn)

        => new Chain<TOut>(mapFn(Value));
}
