﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 组合两个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="extendExpression">表达式2</param>
        /// <param name="mergeWay">组合方式</param>
        /// <returns>新的表达式</returns>
        public static Expression<TSource> Compose<TSource>(this Expression<TSource> expression, Expression<TSource> extendExpression, Func<Expression, Expression, Expression> mergeWay)
        {
            var parameterExpressionSetter = expression.Parameters
                .Select((u, i) => new { u, Parameter = extendExpression.Parameters[i] })
                .ToDictionary(d => d.Parameter, d => d.u);

            var extendExpressionBody = ParameterReplaceExpressionVisitor.ReplaceParameters(parameterExpressionSetter, extendExpression.Body);
            return Expression.Lambda<TSource>(mergeWay(expression.Body, extendExpressionBody), expression.Parameters);
        }

        /// <summary>
        /// 与操作合并两个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, bool>> And<TSource>(this Expression<Func<TSource, bool>> expression, Expression<Func<TSource, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        /// <summary>
        /// 与操作合并两个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, int, bool>> And<TSource>(this Expression<Func<TSource, int, bool>> expression, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.AndAlso);
        }

        /// <summary>
        /// 根据条件成立再与操作合并两个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, bool>> AndIf<TSource>(this Expression<Func<TSource, bool>> expression, bool condition, Expression<Func<TSource, bool>> extendExpression)
        {
            return condition ? expression.Compose(extendExpression, Expression.AndAlso) : expression;
        }

        /// <summary>
        /// 根据条件成立再与操作合并两个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, int, bool>> AndIf<TSource>(this Expression<Func<TSource, int, bool>> expression, bool condition, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return condition ? expression.Compose(extendExpression, Expression.AndAlso) : expression;
        }

        /// <summary>
        /// 或操作合并两个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, bool>> Or<TSource>(this Expression<Func<TSource, bool>> expression, Expression<Func<TSource, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.OrElse);
        }

        /// <summary>
        /// 或操作合并两个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, int, bool>> Or<TSource>(this Expression<Func<TSource, int, bool>> expression, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return expression.Compose(extendExpression, Expression.OrElse);
        }

        /// <summary>
        /// 根据条件成立再或操作合并两个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, bool>> OrIf<TSource>(this Expression<Func<TSource, bool>> expression, bool condition, Expression<Func<TSource, bool>> extendExpression)
        {
            return condition ? expression.Compose(extendExpression, Expression.OrElse) : expression;
        }

        /// <summary>
        /// 根据条件成立再或操作合并两个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式1</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="extendExpression">表达式2</param>
        /// <returns>新的表达式</returns>
        public static Expression<Func<TSource, int, bool>> OrIf<TSource>(this Expression<Func<TSource, int, bool>> expression, bool condition, Expression<Func<TSource, int, bool>> extendExpression)
        {
            return condition ? expression.Compose(extendExpression, Expression.OrElse) : expression;
        }

        /// <summary>
        /// 获取Lambda表达式属性名，只限 u=>u.Property 表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns>属性名</returns>
        public static string GetExpressionPropertyName<TSource>(this Expression<Func<TSource, object>> expression)
        {
            if (expression.Body is UnaryExpression unaryExpression)
            {
                return ((MemberExpression)unaryExpression.Operand).Member.Name;
            }
            else if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (expression.Body is ParameterExpression parameterExpression)
            {
                return parameterExpression.Type.Name;
            }

            throw new InvalidCastException(nameof(expression));
        }

        /// <summary>
        /// 是否是空集合
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <returns>是否为空集合</returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> sources)
        {
            return sources == null || !sources.Any();
        }
    }



    internal sealed class ParameterReplaceExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 参数表达式映射集合
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterExpressionSetter">参数表达式映射集合</param>
        public ParameterReplaceExpressionVisitor(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter)
        {
            this.parameterExpressionSetter = parameterExpressionSetter ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// 替换表达式参数
        /// </summary>
        /// <param name="parameterExpressionSetter">参数表达式映射集合</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的表达式</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionSetter, Expression expression)
        {
            return new ParameterReplaceExpressionVisitor(parameterExpressionSetter).Visit(expression);
        }

        /// <summary>
        /// 重写基类参数访问器
        /// </summary>
        /// <param name="parameterExpression"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            if (parameterExpressionSetter.TryGetValue(parameterExpression, out var replacement))
            {
                parameterExpression = replacement;
            }

            return base.VisitParameter(parameterExpression);
        }
    }
}
