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
        /// Generic Helpers
        /// </summary>
        namespace Generic
        {
            using LazyTitan.Serialization.Attributes;
            using System;
            using System.Collections.Generic;
            using UnityEngine;

            /// <summary>
            /// A generic class for object pooling.
            /// </summary>
            /// <typeparam name="T"> Type the pool contains. </typeparam>
            [Serializable]
            public class ObjectPool<T>
            {
                [SerializeField]
                bool isMutable;
                [SerializeField]
                int initialSize;
                [SerializeField]
                int maxSize;
                [SerializeField]
                List<T> pool;

                T original;
                InstantiateCallback instantiateCallback;

                public delegate T InstantiateCallback(T original);

                /// <summary>
                /// Determines if the properties of the pool be changed after it is created.
                /// </summary>
                public bool GetIsMutable() { return isMutable; }

                /// <summary>
                /// The initial size of the pool.
                /// </summary>
                public int GetInitialSize() { return initialSize; }

                /// <summary>
                /// The maximum size of the pool.
                /// </summary>
                public int GetMaxSize() { return maxSize; }

                /// <summary>
                /// Return an object from the pool to be modified.
                /// </summary>
                /// <param name="index"> The index of the object in the pool. </param>
                public T GetPoolObject(int index)
                {
                    try
                    {
                        return pool[index];
                    }

                    catch (IndexOutOfRangeException e)
                    {
                        throw e;
                    }
                }

                /// <summary>
                /// Returns the pool. Useful for viewing the pool in the inspector.
                /// </summary>
                public List<T> GetPool() { return pool; }

                /// <summary>
                /// The initial size of the pool.
                /// </summary>
                /// <param name="initialSize"> The initial size to be set. </param>
                public void SetInitialSize(int initialSize)
                {
                    if (isMutable)
                    {
                        this.initialSize = initialSize;
                    }
                    
                    else
                    {
                        Debug.LogWarning("This pool is not mutable.");
                    }
                }

                /// <summary>
                /// The maximum size of the pool.
                /// </summary>
                /// <param name="maxSize"> The maximum size to be set. </param>
                public void SetMaxSize(int maxSize)
                {
                    if (isMutable)
                    {
                        this.maxSize = maxSize;
                    }

                    else
                    {
                        Debug.LogWarning("This pool is not mutable.");
                    }
                }

                /// <summary>
                /// The original object that is being duplicated in the pool.
                /// </summary>
                /// <param name="original"> The original object to be set. </param>
                public void SetOriginal(T original) { this.original = original; }

                /// <summary>
                /// Object Pool constructor.
                /// </summary>
                /// <param name="original"> The object that is to be duplicated for the pool. </param>
                /// <param name="initialSize"> The initial size of the pool. </param>
                /// <param name="maxSize"> The maximum size of the pool. Set to -1 for no maximum size. </param>
                /// <param name="instantiateCallback"> Add a callback to instantiate objects. </param>
                /// <param name="isMutable"> Determines if the properties of the pool be changed after it is created. </param>
                public ObjectPool(T original, int initialSize, int maxSize, InstantiateCallback instantiateCallback, bool isMutable = false)
                {
                    this.original = original;
                    this.isMutable = isMutable;
                    this.initialSize = initialSize;
                    this.maxSize = maxSize == -1 ? Mathf.FloorToInt(Mathf.Infinity) : maxSize;
                    this.instantiateCallback = instantiateCallback;

                    pool = new List<T>();

                    if (instantiateCallback != null)
                    {
                        for (int i = 0; i < initialSize; i++)
                        {
                            pool.Add(instantiateCallback(original));
                        }
                    }

                    else
                    {
                        Debug.LogException(new Exception("Instantiate callback was null."));
                    }
                }

                /// <summary>
                /// Return an object from the pool.
                /// </summary>
                /// <returns></returns>
                public T GetNext()
                {
                    try
                    {
                        for (int i = 0; i < pool.Count; i++)
                        {
                            if (pool[i] != null)
                            {
                                T nextPoolObject = pool[i];
                                pool[i] = default(T);

                                return nextPoolObject;
                            }
                        }

                        if (pool.Count < maxSize)
                        {
                            if (instantiateCallback != null)
                            {
                                pool.Add(default(T));

                                return instantiateCallback.Invoke(original);
                            }

                            else
                            {
                                pool.Add(original);
                                T nextPoolObject = pool[pool.Count - 1];
                                pool[pool.Count - 1] = default(T);

                                return original;
                            }
                        }

                        throw new Exception("No available pool object available and the maximum pool size has been reached.");
                    }

                    catch (Exception)
                    {
                        throw new Exception("No available pool object available and the maximum pool size has been reached.");
                    }
                }

                /// <summary>
                /// Return an object back to the pool.
                /// </summary>
                /// <param name="poolObject"> Object to return. </param>
                public void Return(T poolObject)
                {
                    for (int i = 0; i < pool.Count; i++)
                    {
                        if (pool[i] == null)
                        {
                            pool[i] = poolObject;
                        }
                    }
                }

                /// <summary>
                /// Clear the pull.
                /// </summary>
                /// <param name="destroyCallback"> Add a callback to destroy objects. </param>
                public void ClearPool(Action<T> destroyCallback = null)
                {
                    if (destroyCallback != null)
                    {
                        for (int i = 0; i < pool.Count; i++)
                        {
                            destroyCallback(pool[i]);
                        }
                    }

                    pool.Clear();
                }

                /// <summary>
                /// Reset the pool to the initial size.
                /// </summary>
                /// <param name="destroyCallback"> Add a callback to destroy objects. </param>
                public void ResetPool(Action<T> destroyCallback = null)
                {
                    if (destroyCallback != null)
                    {
                        for (int i = 0; i < pool.Count; i++)
                        {
                            destroyCallback(pool[i]);
                        }
                    }

                    pool.Clear();

                    for (int i = 0; i < initialSize; i++)
                    {
                        if (instantiateCallback != null)
                        {
                            pool.Add(instantiateCallback(original));
                        }

                        else
                        {
                            pool.Add(original);
                        }
                    }
                }
            }
        }
    }
}