using System;
using System.Linq.Expressions;

namespace Generic.Abp.Extensions.Extensions;

public static class PredicateExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(Expression.Invoke(expr1, parameter), Expression.Invoke(expr2, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(Expression.Invoke(expr1, parameter), Expression.Invoke(expr2, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> OrIfNotTrue<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        return IsTruePredicate(expr1) ? expr2 : expr1.Or(expr2);
    }

    public static Expression<Func<T, bool>> AndIfNotTrue<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        return IsTruePredicate(expr1) ? expr2 : expr1.And(expr2);
    }

    private static bool IsTruePredicate<T>(Expression<Func<T, bool>>? expr)
    {
        return expr?.Body is ConstantExpression { Value: true };
    }
}