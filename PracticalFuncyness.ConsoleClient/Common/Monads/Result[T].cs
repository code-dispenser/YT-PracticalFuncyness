using PracticalFuncyness.ConsoleClient.Common.Models;
namespace PracticalFuncyness.ConsoleClient.Common.Monads;
internal class Result<T>
{
    private FailureBase FailureValue { get; } = FailureBase.CreateNoFailure();
    private T?          SuccessValue { get; } = default;
    public bool         IsSuccess    { get; }
    public bool         IsFailure => !IsSuccess;

    private Result(T successValue)
    {
        if (successValue is null || successValue is FailureBase) throw new ArgumentException("A successful value cannot be null or a type of failure", nameof(successValue));
        SuccessValue = successValue;
        IsSuccess    = true;
    }
    private Result(FailureBase failureValue)
    {
        if (failureValue is null) throw new ArgumentNullException("A failure value cannot be null", nameof(failureValue));
        FailureValue = failureValue;
        SuccessValue = default!;
    }
    public static Result<T> Success(T successValue)                     => new Result<T>(successValue);
    public static Result<T> Failed(FailureBase failureValue)            => new Result<T>(failureValue);

    public static implicit operator Result<T>(T successValue)           => new Result<T>(successValue);

    public static implicit operator Result<T>(FailureBase failureValue) => new Result<T>(failureValue);

    public Result<TOut> Map<TOut>(Func<T, TOut> do_onSuccess)

        => IsSuccess ? new Result<TOut>(do_onSuccess(SuccessValue!)) : new Result<TOut>(FailureValue);

    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> do_onSuccess)

        => IsSuccess ? do_onSuccess(SuccessValue!) : new Result<TOut>(FailureValue);// Or just FailureValue as we have an implicit operator for it
    
    public Result<T> BindFailure(Func<FailureBase, Result<T>> do_onFailure)//Changed name to avoid potential resolution conflicts

        => IsSuccess ? new Result<T>(SuccessValue!) : do_onFailure(FailureValue);

    public TOut Match<TOut>(Func<FailureBase, TOut> do_onFailure, Func<T, TOut> do_onSuccess)

        => IsSuccess ? do_onSuccess(SuccessValue!) : do_onFailure(FailureValue);

    public void Match(Action<FailureBase> act_onFailure, Action<T> act_onSuccess)
    {
        if (true == IsSuccess) act_onSuccess(SuccessValue!); else act_onFailure(FailureValue);
    }
}
    

