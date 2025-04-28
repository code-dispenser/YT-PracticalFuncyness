using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;

namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

internal static class ResultExtensions
{

    public static Result<T> OnFailure<T>(this Result<T> thisResult, Func<FailureBase, Result<T>> do_onFailure)

        => thisResult.Bind(do_onFailure);

    public static Result<T> OnFailure<T>(this Result<T> thisResult, Action<FailureBase> act_onFailure)//This is our Tap from the previous video
    {
        thisResult.Match(act_onFailure, _ => { });

        return thisResult;
    }
    public static Result<TOut> OnSuccess<TIn, TOut>(this Result<TIn> thisResult, Func<TIn, Result<TOut>> do_onSuccess) 
    
       => thisResult.Bind(do_onSuccess);

    public static Result<TOut> OnSuccess<TIn, TOut>(this Result<TIn> thisResult, Func<TIn, TOut> do_onSuccess)

        => thisResult.Map(do_onSuccess);

    public static TOut Finally<TIn, TOut>(this Result<TIn> thisResult, Func<FailureBase, TOut> do_onFailure, Func<TIn, TOut> do_onSuccess)

        => thisResult.Match(do_onFailure, do_onSuccess);

    public static Result<TOut> OnSuccessTry<TIn, TOut>(this Result<TIn> thisFlow, Func<TIn, Result<TOut>> operationToTry, Func<Exception, Result<TOut>> exceptionHandler)
    {
        try
        {
            return thisFlow.Bind<TOut>(operationToTry);
        }
        catch (Exception ex) { return exceptionHandler(ex); }
    }

}
