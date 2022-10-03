using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is from the Internet and NOT created by me
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance; // Backing variable

    public static T Instance // Property
    {
        // This property only has a getter, not a setter
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T; // Check if we already have an instance of this singleton somewhere else

                // If there is no instance in the hierarchy, create one in code
                if (_instance == null)
                {
                    T obj = new GameObject().AddComponent<T>();
                    _instance = obj as T;
                }

                // Make instance persistent between scenes
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
        
    }
}
