using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISingleton<T> : MonoBehaviour where T : Component
{

    protected static T _instance;

    private static object _lock = new object();

    private static bool applicationIsQuitting = false;

    public const string singletonPathing = "Prefabs/";

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
                        GameObject newGO2;
                        if (Resources.Load(singletonPathing + typeof(T)))
                        {
                            newGO2 = Instantiate(Resources.Load<GameObject>(singletonPathing + typeof(T)));
                            _instance = newGO2.GetComponent<T>();
                        }else
                        {
                            newGO2 = new GameObject();
                            newGO2.name = typeof(T).ToString() + " Singleton";
                            _instance = newGO2.AddComponent<T>();
                            //If its a prefab, if not i just dump this component into a new go
                        }

                        DontDestroyOnLoad(newGO2);
                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + newGO2 +
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
                Debug.Log("Destroyed this " + typeof(T) + " singleton");
                return;
            }
        }
    }
    //public void TurnOff()
    //{
    //    //When Change scene
    //    _instance.gameObject.SetActive(false);
    //    Refresh();
    //}

    public virtual void Refresh(bool x)
    {
        this.gameObject.SetActive(x);
    }

    void OnDisable()
    {
        _instance = null;
    }

    void OnDestroy()
    {
        applicationIsQuitting = true;
    }

}
