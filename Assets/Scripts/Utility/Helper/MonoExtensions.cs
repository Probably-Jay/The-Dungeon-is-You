using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

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
        
            if(componentVariable == null || componentVariable.Equals(null))
            {
                var type = typeof(T);
                throw new NullReferenceException($"{type.BaseType} {type.Name} is null in {gameObject.name}");
            }
        }      
        
        /// <summary>
        /// Calls <see cref="GameObject.GetComponent{T}"/> and assigns the result to <paramref name="componentVariable"/> with the type of <typeparamref name="Ti"/>. Debug logs if component cannot be found.
        /// </summary>
        /// <param name="componentVariable">The variable to hold the component</param>
        /// <typeparam name="T">The type of the component to be found</typeparam>
        /// <typeparam name="Ti">The type of the variable being assigned to</typeparam>
        /// <exception cref="NullReferenceException">Throws if component cannot be found.</exception>
        public static void AssignGetComponentTo<T, Ti>(this MonoBehaviour gameObject, out Ti componentVariable) where T : Component , Ti where Ti : class
        {     
            componentVariable = gameObject.GetComponent(typeof(T)) as Ti;
        
            if(componentVariable == null || componentVariable.Equals(null))
            {
                var type = typeof(T);
                throw new NullReferenceException($"{type.BaseType} {type.Name} is null in {gameObject.name}");
            }
        }        
    
        /// <summary>
        /// Calls <see cref="GameObject.GetComponentInChildren{T}()"/> and assigns the result to <paramref name="componentVariable"/>. Debug logs if component cannot be found.
        /// </summary>
        /// <param name="componentVariable">The variable to hold the component</param>
        /// <typeparam name="T">The type of the component to be found</typeparam>
        /// <exception cref="NullReferenceException">Throws if component cannot be found.</exception>
        public static void GetComponentInChildrenAndAssignTo<T>(this MonoBehaviour gameObject, out T componentVariable) where T : Component
        {     
            componentVariable = gameObject.GetComponentInChildren<T>();
        
            if(componentVariable == null || componentVariable.Equals(null))
            {
                var type = typeof(T);
                throw new NullReferenceException($"{type.BaseType} {type.Name} is null in {gameObject.name}");
            }
        }
    }
}