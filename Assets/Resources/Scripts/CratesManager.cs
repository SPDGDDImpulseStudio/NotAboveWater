using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratesManager : ISingleton<CratesManager> {

    public List<Interact_CompulCrates> cratesToDestroy = new List<Interact_CompulCrates>();
	// Use this for initialization
	void Start () {
		
	}
	
    public void PopulateList(Interact_CompulCrates x)
    {
        cratesToDestroy.Add(x);
    }

   
    public void RemoveThisCrate(Interact_CompulCrates x)
    {
        if (cratesToDestroy.Count == 0) return;
        for (int i = 0; i < cratesToDestroy.Count; i++)
        {
            if (x == cratesToDestroy[i])
            {
                Debug.Log(cratesToDestroy[i]);

                cratesToDestroy.RemoveAt(i);
            }
        }
    } 

	void Update () {
		
	}
}
