using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour {

    [Tooltip("The prefab i referred to")]
    public GameObject fishPrefab;
    [Tooltip("Cheap way to control spawning of fish, note its not control by Update function but a coroutine.")]
    public bool spawnFish = true;
    [Tooltip("Maximum number of fishes can be spawn")]
    public int maxNumberOfFish = 5;
    [Tooltip("How I check how many fish is spawned in")]
    public List<GameObject> fishes = new List<GameObject>();
    [Tooltip("How long does it take to spawn another fish")]
    public float spawnRate = 2.5f;
    [Tooltip("The area where I spawn fishes")]
    public float spawnArea;
    void Start () {
        GameObject fishParent = new GameObject();
        fishParent.name = "Fish Pool";
        PoolManager.RequestCreatePool(fishPrefab, maxNumberOfFish, fishParent.transform);
        Init();
	}
	public void Init()
    {
        SpawnDemFishes();
    }
    public void SpawnDemFishes()
    {
        StartCoroutine(SpawnFishes());
    }
    IEnumerator SpawnFishes()
    {
        if (spawnFish) yield break;
        spawnFish = true;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (!spawnFish)  break;
            if(timer> spawnRate)
            {
                timer = 0f;
                if (fishes.Count - 1 == maxNumberOfFish)
                    continue;

                Vector3 pos = this.transform.position+Random.insideUnitSphere * spawnArea;
                GameObject newFish = SpawnDahFish(fishPrefab, pos, Quaternion.identity);
                fishes.Add(newFish);
            }

            
            yield return null;
        }
        spawnFish = false;
    }
    GameObject SpawnDahFish(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject x = PoolManager.Instance.ReturnGOFromList(prefab);
        x.transform.position = pos;
        x.transform.rotation = rot;
        return x;
    }
#if UNITY_EDITOR

    [Header("EDITOR ATTRIBUTES")]
    public bool showEditor;
    public Color editorColor = Color.black;
    void OnDrawGizmos()
    {
        if (!showEditor) return;
        Gizmos.color = editorColor;
        Gizmos.DrawWireSphere(this.transform.position, spawnArea);
    } 
#endif
}
