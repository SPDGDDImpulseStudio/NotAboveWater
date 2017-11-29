using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISingleton<T> : MonoBehaviour where T : Component
{

    protected static T _instance;

    private static object _lock = new object();

    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newGO = new GameObject();
                        _instance = newGO.AddComponent<T>();
                        newGO.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(newGO);
                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + newGO +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);

                    }

                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else
        {
            if (_instance.GetInstanceID() != this.GetInstanceID())
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    void Update()
    {

    }
}
