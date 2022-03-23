using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Singleton
{
    /// <summary>
    /// Singleton base class. 
    /// <see cref="T"/> must be the type of the child inheriting from this class.
    /// </summary>
    /// <typeparam name="T">The class *inheriting* from this class, the one that will be a singleton</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
    
        /// <summary>
        /// The <see cref="Singleton{T}"/> instance of this class
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!InstanceExists)
                {
                    TryInitialiseSingleton();
                }
                // AssertInstanceExists();
                return instance;
            }

            private set
            {
                if (value == null)
                    throw new NullReferenceException();
                instance = value;
            }
        }

        /// <summary>
        /// Returns <see cref="Instance"/> if it exists, else returns <c>null</c>
        /// </summary>
        [CanBeNull]
        public static T TryInstance 
            => InstanceExists 
                ? Instance 
                : null;

        /// <summary>
        /// If an instance of this singleton currently exists
        /// </summary>
        public static bool InstanceExists => instance != null;
        
        /// <summary>
        /// Will output warning if instance doesnt exist
        /// </summary>d
        public static void WarnInstanceDoesNotExist() { if (!InstanceExists && Application.isPlaying) Debug.LogWarning(SingletonDoesNotExistException.DoesNotExistMessage); }
        
        /// <summary>
        /// Will throw <see cref="SingletonDoesNotExistException"/> if instance does not exist
        /// </summary>
        public static void AssertInstanceExists() { if (!InstanceExists && Application.isPlaying) throw new SingletonDoesNotExistException(); }

        private static void TryInitialiseSingleton()
        {
            var singleton = FindSingleton();
            SetSingleton(singleton);
            DontDestroyOnLoad(singleton.gameObject);
        }

        private static T FindSingleton()
        {
            var singletons = FindObjectsOfType<T>();
            if (singletons.Length == 0)
                throw new SingletonDoesNotExistException();
            if (singletons.Length > 1)
                throw new MultipleSingletonInSceneException(singletons);

            return singletons.First();
        }

        private static void SetSingleton([NotNull] T thisSingleton)
        {
            if (thisSingleton == null) 
                throw new ArgumentNullException(nameof(thisSingleton), "Cannot set singleton as singleton may not exist");
            
            if (thisSingleton.GetType() != typeof(T)) // this should never happen
                throw new Exception(
                    $"Singletons can only reference their own types, {typeof(Singleton<T>)} cannot be used to template typeof {thisSingleton.GetType()}"); // this is really bad

            if (instance != null && instance != thisSingleton)
            {
                throw new MultipleSingletonInSceneException(thisSingleton, instance);
            }

            Singleton<T>.Instance = thisSingleton;
            
            AssertInstanceExists();
        }

        
        [Serializable]
        protected class SingletonDoesNotExistException : Exception
        {
            public static string DoesNotExistMessage => $"{typeof(Singleton<T>)} is required by a script, but does not exist in (or has not been initialised in) scene \"{SceneManager.GetActiveScene().name}\".";
            public SingletonDoesNotExistException():base(DoesNotExistMessage) { }
        } 
        
        [Serializable]
        protected class MultipleSingletonInSceneException : Exception
        {
            public static string MultipleInSceneMessage(IEnumerable<T> singletons)
            {
                var message =
                    $"{typeof(Singleton<T>)} is a singleton, but multiple copies exist in the scene {SceneManager.GetActiveScene().name}: ";
                return message + singletons.ToListString(s => s.name);
            }

            public MultipleSingletonInSceneException(params T[] singletons):base(MultipleInSceneMessage(singletons)) { }
        }

    }
    
}
