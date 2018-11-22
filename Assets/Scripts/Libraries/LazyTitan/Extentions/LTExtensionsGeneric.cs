using System;

/// <summary>
/// A collection of useful code pieces.
/// </summary>
namespace LazyTitan
{
    /// <summary>
    /// Extensions.
    /// </summary>
    namespace Extensions
    {
        /// <summary>
        /// Generic Extensions.
        /// </summary>
        namespace Generic
        {
            /// <summary>
            /// Extensions for generic types.
            /// </summary>
            public static class GenericExtensions
            {
                /// <summary>
                /// Generic parsing.
                /// </summary>
                /// <typeparam name="T"> The type to convert to. </typeparam>
                /// <param name="obj"> The object to convert. </param>
                /// <returns> The new object type. </returns>
                public static T Parse<T>(this object obj)
                {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
            }
        }
    }
}