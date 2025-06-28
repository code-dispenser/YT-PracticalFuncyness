using PracticalFuncyness.ConsoleClient.Common.Models;

namespace PracticalFuncyness.ConsoleClient.Common.Monads;

public record struct State<TState, TValue>
{
    private readonly Func<TState, (TValue Value, TState NewState)> _run;

    public State(Func<TState, (TValue Value, TState NewState)> runFunction)
        
        => _run = runFunction ?? throw new ArgumentNullException(nameof(runFunction));

    public (TValue Value, TState State) Run(TState initialState) 
        
        => _run(initialState);

    public State<TState, TResult> Map<TResult>(Func<TValue, TResult> mapFn)
    {
        var run = _run;
        return new State<TState, TResult>(state =>
        {
            var (value, newState) = run(state);
            return (mapFn(value), newState);
        });
    }

    public State<TState, TResult> Bind<TResult>(Func<TValue, State<TState, TResult>> bindFn)
    {
        var run = _run; // Need to capture because you can not use _run directly in lambda. Error CS1673
        return new State<TState, TResult>(state =>
        {
            var (value, intermediateState) = run(state);
            return bindFn(value)._run(intermediateState);
        });
    }

    public static State<TState, TValue> Return(TValue value)

        => new State<TState, TValue>(state => (value, state));

    public static State<TState, TState> Get

        => new State<TState, TState>(state => (state, state));

    public static State<TState, None> Put(TState newState)

        => new State<TState, None>(_ => (None.Value, newState));

    public static State<TState, None> Modify(Func<TState, TState> modifier)
        
        => Get.Bind(state => Put(modifier(state)));
}
