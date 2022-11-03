﻿using System.Linq;

namespace EFCoreProjections.Cmd
{
    public static class QueryExtensions
    {
        public static IQueryable<T> If<T>(
            this IQueryable<T> source,
            bool condition,
            System.Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }
    }
}
