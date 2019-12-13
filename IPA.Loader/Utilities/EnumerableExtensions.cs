﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPA.Utilities
{
    /// <summary>
    /// Extensions for <see cref="IEnumerable{T}"/> that don't currently exist in <c>System.Linq</c>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds a value to the beginning of the sequence.
        /// </summary>
        /// <typeparam name="T">the type of the elements of <paramref name="seq"/></typeparam>
        /// <param name="seq">a sequence of values</param>
        /// <param name="prep">the value to prepend to <paramref name="seq"/></param>
        /// <returns>a new sequence beginning with <paramref name="prep"/></returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> seq, T prep)
            => new PrependEnumerable<T>(seq, prep);

        private sealed class PrependEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> rest;
            private readonly T first;

            public PrependEnumerable(IEnumerable<T> rest, T first)
            {
                this.rest = rest;
                this.first = first;
            }

            public IEnumerator<T> GetEnumerator()
            {
                yield return first;
                foreach (var v in rest) yield return v;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        /// LINQ extension method that filters <see langword="null"/> elements out of an enumeration.
        /// </summary>
        /// <typeparam name="T">the type of the enumeration</typeparam>
        /// <param name="self">the enumeration to filter</param>
        /// <returns>a filtered enumerable</returns>
        public static IEnumerable<T> NonNull<T>(this IEnumerable<T> self) where T : class
            => self.Where(o => o != null);

        /// <summary>
        /// LINQ extension method that filters <see langword="null"/> elements out of an enumeration based on a converter.
        /// </summary>
        /// <typeparam name="T">the type of the enumeration</typeparam>
        /// <typeparam name="U">the type to compare to null</typeparam>
        /// <param name="self">the enumeration to filter</param>
        /// <param name="pred">the predicate to select for filtering</param>
        /// <returns>a filtered enumerable</returns>
        public static IEnumerable<T> NonNull<T, U>(this IEnumerable<T> self, Func<T, U> pred) where T : class where U : class
            => self.Where(o => pred(o) != null);

        /// <summary>
        /// LINQ extension method that filters <see langword="null"/> elements from an enumeration of nullable types.
        /// </summary>
        /// <typeparam name="T">the underlying type of the nullable enumeration</typeparam>
        /// <param name="self">the enumeration to filter</param>
        /// <returns>a filtered enumerable</returns>
        public static IEnumerable<T> NonNull<T>(this IEnumerable<T?> self) where T : struct
            => self.Where(o => o != null).Select(o => o.Value);

    }
}
