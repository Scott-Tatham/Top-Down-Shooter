/// <summary>
/// A collection of useful code pieces.
/// </summary>
namespace LazyTitan
{
    /// <summary>
    /// Helpers.
    /// </summary>
    namespace Helpers
    {
        /// <summary>
        /// JSON Helpers.
        /// </summary>
        namespace JsonHelpers
        {
            using System;
            using System.Collections.Generic;
            using UnityEngine;

            /// <summary>
            /// A generic class type that contains a list for using Unity's JSON Utilities with arrays/lists.
            /// </summary>
            /// <typeparam name="T"> Type the container is for. </typeparam>
            [Serializable]
            public class JsonArrayContainer<T>
            {
                [SerializeField]
                List<T> contained = new List<T>();

                public List<T> GetContained() { return contained; }

                public void SetContained(List<T> contents) { contained = contents; }
            }
        }
    }
}