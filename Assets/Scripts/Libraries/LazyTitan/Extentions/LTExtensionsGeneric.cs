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
            using System.Collections.Generic;
            using UnityEngine;

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

                /// <summary>
                /// Return a component or if non existing add it and return it.
                /// </summary>
                /// <typeparam name="T"> The type to return. </typeparam>
                /// <param name="obj"> The GameObject to search on. </param>
                /// <returns> The component of that type. </returns>
                public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
                {
                    T component = obj.GetComponent<T>();

                    if (!obj.GetComponent<T>())
                    {
                        component = obj.AddComponent<T>() as T;
                    }

                    return component;
                }

                /// <summary>
                /// Returns components or if non existing adds them and returns them.
                /// </summary>
                /// <typeparam name="T"> The type to return. </typeparam>
                /// <param name="obj"> The GameObject to search on. </param>
                /// <param name="count"> The minimum required amount of the component the GameObject should have. </param>
                /// <returns></returns>
                public static T[] GetOrAddComponents<T>(this GameObject obj, int count) where T : Component
                {
                    // To store current components of type T.
                    T[] currentComponents = obj.GetComponents<T>();

                    // Determine the difference between the current amount of components and the required amount.
                    int addCount = count - currentComponents.Length;

                    // The components to be returned.
                    T[] components = new T[count];

                    // If the difference between the current components and the required amount is positive.
                    if (addCount > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            // Copy current components until the all current components have been retrieved then add new components to fill out the remaining amount.
                            components[i] = i < currentComponents.Length ? currentComponents[i] : (obj.AddComponent<T>() as T);
                        }
                    }

                    // If the difference between the current components and the requuired amount is negative.
                    else
                    {
                        for (int i = 0; i < currentComponents.Length; i++)
                        {
                            // Copy current components until the required amount has been retrieved then destroy the rest of the current components.
                            if (i < components.Length)
                            {
                                components[i] = currentComponents[i];
                            }

                            else
                            {
                                Object.Destroy(currentComponents[i]);
                            }
                        }
                    }

                    return components;
                }
            }
        }
    }
}