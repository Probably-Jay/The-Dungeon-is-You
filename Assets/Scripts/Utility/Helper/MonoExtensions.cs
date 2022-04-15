using System;
using UnityEngine;

namespace Utility
{
    public static class MonoExtensions
    {
        /// <summary>
        /// Calls <see cref="GameObject.GetComponent{T}"/> and assigns the result to <paramref name="componentVariable"/>. Debug logs if component cannot be found.
        /// </summary>
        /// <param name="componentVariable">The variable to hold the component</param>
        /// <typeparam name="T">The type of the component to be found</typeparam>
        /// <exception cref="NullReferenceException">Throws if component cannot be found.</exception>
        public static void AssignGetComponentTo<T>(this MonoBehaviour gameObject, out T componentVariable) where T : Component
        {
            componentVariable = gameObject.GetComponent(typeof(T)) as T;

            gameObject.AssertNotNull(componentVariable);
        }

        /// <summary>
        /// Calls <see cref="GameObject.GetComponent{T}"/> and assigns the result to <paramref name="componentVariable"/> with the type of <typeparamref name="Ti"/>. Debug logs if component cannot be found.
        /// </summary>
        /// <param name="componentVariable">The variable to hold the component</param>
        /// <typeparam name="T">The type of the component to be found</typeparam>
        /// <typeparam name="Ti">The type of the variable being assigned to</typeparam>
        /// <exception cref="NullReferenceException">Throws if component cannot be found.</exception>
        // ReSharper disable once InvalidXmlDocComment
        public static void AssignGetComponentTo<T, Ti>(this MonoBehaviour gameObject, out Ti componentVariable) where T : Component , Ti where Ti : class
        {     
            componentVariable = gameObject.GetComponent(typeof(T)) as Ti;
        
            gameObject.AssertNotNull(componentVariable as T);
        }

        /// <summary>
        /// Calls <see cref="GameObject.GetComponentInChildren{T}()"/> and assigns the result to <paramref name="componentVariable"/>. Debug logs if component cannot be found.
        /// </summary>
        /// <param name="componentVariable">The variable to hold the component</param>
        /// <typeparam name="T">The type of the component to be found</typeparam>
        /// <exception cref="NullReferenceException">Throws if component cannot be found.</exception>
        // ReSharper disable once InvalidXmlDocComment
        public static void AssignGetComponentInChildrenTo<T>(this MonoBehaviour gameObject, out T componentVariable) where T : Component
        {     
            componentVariable = gameObject.GetComponentInChildren(typeof(T)) as T;
        
            gameObject.AssertNotNull(componentVariable);
        }

        public static void AssertNotNull<T>(this MonoBehaviour gameObject, T componentVariable) where T : Component
        {
            if (componentVariable == null || componentVariable.Equals(null))
            {
                var type = typeof(T);
                throw new NullReferenceException($"Component {type.Namespace}.{type.Name} is null in \"{gameObject.name}\"");
            }
        }
    }
}