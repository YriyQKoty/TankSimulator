using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"Singleton {typeof(T)} was not found!");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        var curObjectScripts = FindObjectsOfType<T>();
        if (curObjectScripts.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
    }
}
