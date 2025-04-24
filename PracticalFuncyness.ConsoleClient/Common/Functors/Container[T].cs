namespace PracticalFuncyness.ConsoleClient.Common.Functors;   

/*
    * Example only to show that List's Select is Map but on IEnumerable.
    *
    * This Container<T> is a container that wraps a single value.
    * In functional programming terminology, a container that supports a mapping function is called a Functor.
    *
    * To be a proper Functor, there are laws it should follow:
    *   1. Identity:      container.Map(x => x) == container
    *   2. Composition:   container.Map(f).Map(g) == container.Map(x => g(f(x)))
    *
    * Our Container<T> is similar to List<T> in that it's a container, but instead of holding many values, it holds just one.
    * List<T> is monad, we could make our Container a monad by adding a Bind function and ensuring other Laws are followed.
    *
    * Note: C# LINQ uses the keyword `Select` instead of `Map`. 
    * While the naming differs from functional programming, the idea is the same: transformation.
    * For consistency with C# syntax and query expressions, `Select` here delegates to `Map`, you can use either,
    * and thanks to the compiler both forms of syntax i.e from x in or x => x etc.
    * 
    * If you take away the container, then you can see that the Map function is just a function that takes a value and returns a value.
    * And if used generically, it can be used to transform any value or to chain things together (function composition).
*/
internal record Container<T>
{   
    public T Content { get;}
   
    public Container(T content) => Content = content;
  
    public Container<TOut> Map<TOut>(Func<T, TOut> mapFn) 

        => new Container<TOut>(mapFn(Content));// <<< Apply function to the content of the container and return a new container with the result.

    public Container<TOut> Select<TOut>(Func<T, TOut> selectFn) 

        => Map(selectFn);
}

